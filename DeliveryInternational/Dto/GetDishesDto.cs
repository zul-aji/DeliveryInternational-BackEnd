namespace DeliveryInternational.Dto
{
    public class GetDishesDto
    {
        public List<DishDto> Dishes { get; set; }
        public PaginationDto Pagination { get; set; }
    }

    public class DishDto
    {
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public bool Vegetarian { get; set; }
        public int Rating { get; set; }
        public string Category { get; set; }
    }

    public class PaginationDto
    {
        public int Size { get; set; }
        public int Count { get; set; }
        public int Current { get; set; }
    }
}
