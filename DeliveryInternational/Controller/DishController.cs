using AutoMapper;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace DeliveryInternational.Controller
{
    [Route("api/[controller]"), ApiController]
    public class DishController : ControllerBase
    {
        private const int PageSize = 10;
        private readonly IDishRepository _dishInterface;
        private readonly IRatingRepository _ratingInterface;
        private readonly IMapper _mapper;

        public DishController(IDishRepository dishInterface, IRatingRepository ratingInterface, IMapper mapper) 
        {
            _dishInterface = dishInterface;
            _ratingInterface = ratingInterface;
            _mapper = mapper;
        }

    [HttpGet]
    [SwaggerResponse(200, Type = typeof(IEnumerable<GetDishesDto>))]
    [SwaggerResponse(400, "Bad Request")]
    [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
    public IActionResult GetDishes(
        int page,
        [FromQuery] string[] categories,
        [FromQuery] bool? vegetarian,
        [FromQuery] string sorting)
    {
        var totalDishes = _dishInterface.GetTotalDishesCount();

        var totalPages = (int)Math.Ceiling((double)totalDishes / PageSize);

        if (page < 1 || page > totalPages)
            return BadRequest("Invalid page number");

        if (categories != null && categories.Any(c => !_dishInterface.IsValidCategory(c)))
            return BadRequest("Invalid categories");

        if (!_dishInterface.IsValidSortingCriteria(sorting))
            return BadRequest("Invalid sorting");

        // Apply filtering based on categories and vegetarian
        var filteredDishes = _dishInterface.FilterDishes(categories, vegetarian);

        // Apply sorting based on the sorting parameter
        var sortedDishes = _dishInterface.SortDishes(filteredDishes, sorting);

        var paginatedDishes = _dishInterface.GetDishesPaginated(sortedDishes, page, PageSize);

        var dishDtos = new List<DishDto>();

        foreach (var dish in paginatedDishes)
        {
            var dishDto = _mapper.Map<DishDto>(dish);
            dishDtos.Add(dishDto);
        }

        var responseDto = new GetDishesDto
        {
            Dishes = dishDtos,
            Pagination = new PaginationDto
            {
                Size = PageSize,
                Count = totalDishes,
                Current = page
            }
        };

        return Ok(responseDto);
    }


        [HttpGet("{dishId}")]
        [SwaggerResponse(200, "Success", Type = typeof(DishDto))]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetDish(Guid dishId)
        {
            if (!_dishInterface.DishExist(dishId))
                return NotFound();

            var dish = _dishInterface.GetDish(dishId);
            try
            {
                // Map Dish entity to simplified DishDto
                var dishDto = _mapper.Map<DishDto>(dish);
                return Ok(dishDto);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "500",
                    Message = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("{dishId}/rating/check"), Authorize]
        [SwaggerResponse(200, "Success", Type = typeof(bool))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult CheckRating(string dishId) 
        {
            Guid dishGuid = Guid.Parse(dishId);

            if (!_dishInterface.DishExist(dishGuid))
                return NotFound();

            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var userId = _dishInterface.GetUserId(userEmail);
                var userGuid = Guid.Parse(userId);

                var hasOrdered = _dishInterface.CanAddRating(userGuid, dishGuid);

                return Ok(hasOrdered);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "500",
                    Message = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("{dishId}/rating"), Authorize]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult AddRating(string dishId, [FromQuery] int ratingScore)
        {
            Guid dishGuid = Guid.Parse(dishId);

            if (!_dishInterface.DishExist(dishGuid))
                return NotFound();

            if (ratingScore < 0 || ratingScore > 10)
                return BadRequest("Rating score must be between 0 and 10.");
            

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userId = _dishInterface.GetUserId(userEmail);
            var userGuid = Guid.Parse(userId);

            try
            {
                var canAddRating = _dishInterface.CanAddRating(userGuid, dishGuid);

                if (canAddRating)
                { 
                    // Check if the user has already rated this dish
                    var existingRating = _ratingInterface.isRatingExist(userGuid, dishGuid);

                    if (existingRating != null)
                    {
                        // Update the existing rating
                        existingRating.Value = ratingScore;
                        _ratingInterface.UpdateRating(existingRating);

                        var dish = _dishInterface.GetDish(dishGuid);
                        dish.Rating = _ratingInterface.CalculateRatingAvg(dishGuid);

                        _dishInterface.UpdateRating(dish);

                        return Ok("Rating updated successfully.");
                    }

                    // Create a new Rating entity
                    var newRating = new Rating
                    {
                        RatingId = Guid.NewGuid(),
                        DishId = dishGuid,
                        Value = ratingScore,
                        UserId = userGuid
                    };

                    // Add the new rating to the dish
                    _ratingInterface.AddRating(newRating);

                    var newDish = _dishInterface.GetDish(dishGuid);
                    newDish.Rating = _ratingInterface.CalculateRatingAvg(dishGuid);
                    
                    _dishInterface.UpdateRating(newDish);

                    return Ok("Rating added successfully.");
                } 
                else
                {
                    return BadRequest("User must have already ordered this dish before");
                }
                
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "500",
                    Message = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }
    }
}
