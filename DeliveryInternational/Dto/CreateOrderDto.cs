using DeliveryInternational.Models;

namespace DeliveryInternational.Dto
{
    public class CreateOrderDto
    {
        public DateTime DeliveryTime { get; set; }
        public string Address { get; set; }
    }
}
