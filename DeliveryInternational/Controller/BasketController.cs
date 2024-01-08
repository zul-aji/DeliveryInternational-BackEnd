using AutoMapper;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace DeliveryInternational.Controller
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketInterface;
        private readonly IMapper _mapper;

        public BasketController(IConfiguration configuration, IBasketRepository basketInterface, IMapper mapper)
        {
            _configuration = configuration;
            _basketInterface = basketInterface;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetBasket()
        {
            return Ok();
        }

        [HttpPost("dish/{dishId}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult AddDishtoBasket(string dishId) 
        {
            if (dishId == null)
                return BadRequest(ModelState);

            if (!_basketInterface.IsDishExist(dishId))
                return StatusCode(400, "Dish not found");

            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email).Value;
                var userId = _basketInterface.GetUserId(userEmail);
                if (_basketInterface.AddorUpdateBasket(userId, dishId))
                    return Ok("Dish Added");
                else
                    return StatusCode(500, "Unknown Error");

            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = ex.StackTrace,
                    Message = ex.Message

                };

                return StatusCode(500, errorResponse);
            }
        }
    }
}
