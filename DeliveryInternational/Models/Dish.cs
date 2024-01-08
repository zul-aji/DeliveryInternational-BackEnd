namespace DeliveryInternational.Models
{
    public class Dish
    {
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool isVegetarian { get; set; }
        public string Image { get; set; }
        public Category Category { get; set; }
        public ICollection<Rating> UserRating { get; set; }
    }
}
