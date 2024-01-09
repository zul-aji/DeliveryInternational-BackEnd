using System.ComponentModel.DataAnnotations;

namespace DeliveryInternational.Models
{
    public class DishInOrder
    {
        [Key] public Guid DishinId { get; set; }
        public Guid DishId { get; set; }
        public string DishName { get; set; }
        public int DishPrice { get; set; }
        public int TotalPrice { get; set; }
        public int Amount { get; set; }
        public string DishImage { get; set; }
    }
}
