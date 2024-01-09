using DeliveryInternational.Models;
namespace DeliveryInternational.Dto
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; }
        public int Price { get; set; }
        public List<DishInOrder> Dishes { get; set; }
        public string Address { get; set; }
    }
}
