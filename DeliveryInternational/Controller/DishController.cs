using AutoMapper;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeliveryInternational.Controller
{
    [Route("api/[controller]"), ApiController]
    public class DishController : ControllerBase
    {
        private const int PageSize = 10;
        private readonly IDishRepository _dishInterface;
        private readonly IMapper _mapper;

        public DishController(IDishRepository dishInterface, IMapper mapper) 
        {
            _dishInterface = dishInterface;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(IEnumerable<GetDishesDto>))]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetDishes(int page)
        {
            var totalDishes = _dishInterface.GetTotalDishesCount();

            var totalPages = (int)Math.Ceiling((double)totalDishes / PageSize);

            if (page < 1 || page > totalPages)
            {
                return BadRequest("Invalid page number");
            }

            var dishes = _dishInterface.GetDishesPaginated(page, PageSize); 

            var dishDtos = new List<DishDto>();

            foreach (var dish in dishes)
            {
                var dishDto = _mapper.Map<DishDto>(dish);

                // Calculate average rating for the dish
                if (dish.UserRating != null && dish.UserRating.Any())
                {
                    dishDto.Rating = (int)dish.UserRating.Average(r => r.Value);
                }

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

        [HttpGet("{id}")]
        [SwaggerResponse(200, "Success", Type = typeof(DishDto))]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetDish(Guid id)
        {
            if (!_dishInterface.DishExist(id))
                return NotFound();

            var dish = _dishInterface.GetDish(id);
            try
            {
                // Map Dish entity to simplified DishDto
                var dishDto = _mapper.Map<DishDto>(dish);

                // Calculate average rating for the dish
                if (dish.UserRating != null && dish.UserRating.Any())
                {
                    dishDto.Rating = (int)dish.UserRating.Average(r => r.Value);
                }

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
    }
}
