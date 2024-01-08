namespace DeliveryInternational.Models
{
    public class Basket
    {
        public Guid BasketId { get; set; }
        public Guid DishId { get; set; }
        public Guid UserId { get; set; }
        public int Count { get; set; }
    }
}
