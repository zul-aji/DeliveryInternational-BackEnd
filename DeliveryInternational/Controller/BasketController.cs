using AutoMapper;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.Security.Claims;

namespace DeliveryInternational.Controller
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketInterface;

        public BasketController( IBasketRepository basketInterface)
        {
            _basketInterface = basketInterface;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetBasket()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email).Value;
                var userId = _basketInterface.GetUserId(userEmail);
                var userGuid = Guid.Parse(userId);
                var baskets = _basketInterface.GetBasketList(userGuid);

                if (baskets == null)
                {
                    return Ok("No item at the basket");
                }

                else 
                {
                    var basketDtos = baskets.Select(basket => new BasketAndOrderDto
                    {
                        DishId = basket.DishId,
                        DishName = _basketInterface.GetDishNameById(basket.DishId),
                        DishPrice = _basketInterface.GetDishPriceById(basket.DishId),
                        TotalPrice = _basketInterface.GetDishPriceById(basket.DishId) * basket.Count,
                        Amount = basket.Count,
                        DishImage = _basketInterface.GetDishImageById(basket.DishId)
                    });

                    return Ok(basketDtos);
                }

            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "Error",
                    Message = ex.Message

                };

                return StatusCode(500, errorResponse);
            }
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
                if (_basketInterface.AddorUpdateBasket(dishId, userId))
                    return Ok("Dish added to basket");
                else
                    return StatusCode(500, "Unknown Error");

            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "Error",
                    Message = ex.Message

                };

                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("dish/{dishId}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult DeleteDish([FromRoute] string dishId, [FromQuery] bool isDecrease)
        {
            if (dishId == null)
                return BadRequest(ModelState);

            if (!_basketInterface.IsDishExist(dishId))
                return StatusCode(400, "Dish not found");

            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email).Value;
                var userId = _basketInterface.GetUserId(userEmail);
                if (isDecrease) 
                {
                    if (_basketInterface.DecreaseDishOnBasket(dishId, userId))
                        return Ok("Dish decreased");
                    else
                        return StatusCode(403, "Forbidden");
                }
                else
                {
                    if (_basketInterface.DeleteDishFromBasket(dishId, userId))
                        return Ok("Dish deleted");
                    else 
                        return StatusCode(403, "Forbidden");
                }
                    

            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "Error",
                    Message = ex.Message

                };

                return StatusCode(500, errorResponse);
            }
        }
    }
}
