namespace DeliveryInternational.Models
{
    public class Rating
    {
        public Guid RatingId { get; set; }
        public int? Value { get; set; }
        public Guid DishId { get; set; }
        public Guid UserId { get; set; }
    }
}
