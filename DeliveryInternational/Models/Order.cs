namespace DeliveryInternational.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public Status Status { get; set; }
        public int Price { get; set; }
        public ICollection<Dish> Dishes { get; set; }
        public string Address { get; set; }
    }
}
