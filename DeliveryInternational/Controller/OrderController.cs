using AutoMapper;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace DeliveryInternational.Controller
{
    [Route("api/[controller]"), Authorize, ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderInterface;
        private readonly IBasketRepository _basketInterface;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderInterface, IBasketRepository basketInterface, IMapper mapper)
        {
            _orderInterface = orderInterface;
            _basketInterface = basketInterface;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetOrders() 
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email).Value;
                var userId = _basketInterface.GetUserId(userEmail);
                var userGuid = Guid.Parse(userId);
                var orders = _mapper.Map<List<GetOrdersDto>>(_orderInterface.GetOrders(userGuid));

                if (orders.Count == 0)
                    return NotFound();

                return Ok(orders);
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

        [HttpGet("{orderId}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetOrder(string orderId) 
        {
            var orderGuid = Guid.Parse(orderId);
            if (!_orderInterface.OrderExist(orderGuid))
                return NotFound();

            try
            {
                var order = _orderInterface.GetOrder(orderGuid);

                // Map Order entity to OrderDto using AutoMapper
                GetOrderDto orderDto = new()
                {
                    Id = orderGuid,
                    DeliveryTime = order.DeliveryTime,
                    OrderTime = order.OrderTime,
                    Status = order.Status,
                    Price = order.Price,
                    Dishes = order.Dishes,
                    Address = order.Address,
                };

                return Ok(orderDto);
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

        [HttpPost]
        [SwaggerResponse(200, "Success", Type = typeof(CreateOrderDto))]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult CreateOrder()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var userId = _basketInterface.GetUserId(userEmail);
                var userGuid = Guid.Parse(userId);
                Guid orderGuid = Guid.NewGuid();
                var baskets = _basketInterface.GetBasketList(userGuid);

                if (baskets == null)
                {
                    return StatusCode(403,"No item at the basket");
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



                    if (_orderInterface.CreateOrder(orderGuid, basketDtos, userEmail))
                    {
                        foreach (var basketItem in basketDtos)
                        {
                            Guid dishGuid = basketItem.DishId;
                            _basketInterface.DeleteDishFromBasket(dishGuid.ToString(), userId);
                        }
                    }
                    var createOrderDto = new CreateOrderDto
                    {
                        DeliveryTime = _orderInterface.GetDeliverytime(orderGuid),
                        Address = _orderInterface.GetAddress(orderGuid)
                    };
                    return Ok(createOrderDto);
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
