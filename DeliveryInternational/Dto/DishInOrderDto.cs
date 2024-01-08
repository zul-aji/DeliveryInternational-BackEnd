namespace DeliveryInternational.Dto
{
    public class DishInOrderDto
    {
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        public int Amount { get; set; }
        public string Image { get; set; }
    }
}
