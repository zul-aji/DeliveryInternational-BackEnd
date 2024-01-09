using DeliveryInternational.Models;

namespace DeliveryInternational.Dto
{
    public class GetOrdersDto
    {
        public Guid OrderId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; }
        public int Price { get; set; }
    }
}
