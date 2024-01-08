using AutoMapper;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeliveryInternational.Controller
{
    [Route("api/[controller]"), Authorize, ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderInterface;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderInterface, IMapper mapper)
        {
            _orderInterface = orderInterface;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<GetOrdersDto>))]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetOrders() 
        {
            try
            {
                var orders = _mapper.Map<List<GetOrdersDto>>(_orderInterface.GetOrders());

                if (!ModelState.IsValid)
                    return BadRequest();

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

        [HttpGet("{id}")]
        [SwaggerResponse(200, "Success", Type = typeof(GetOrderDto))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetOrder(Guid id) 
        {
            if (!_orderInterface.OrderExist(id))
                return NotFound();

            try
            {
                var order = _orderInterface.GetOrder(id);

                // Map Order entity to OrderDto using AutoMapper
                var orderDto = _mapper.Map<GetOrderDto>(order);

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
    }
}
