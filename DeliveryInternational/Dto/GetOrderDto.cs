namespace DeliveryInternational.Dto
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; }
        public int Price { get; set; }
        public List<DishinOrder> Dishes { get; set; }
        public string Address { get; set; }
    }

    public class DishinOrder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        public int Amount { get; set; }
        public string Image { get; set; }
    }
}
