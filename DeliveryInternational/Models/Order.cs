namespace DeliveryInternational.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; }
        public int Price { get; set; }
        public List<DishInOrder> Dishes { get; set; }
        public string Address { get; set; }
    }
}
