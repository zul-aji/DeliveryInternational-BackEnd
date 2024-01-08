using DeliveryInternational.Models;

namespace DeliveryInternational.Interface
{
    public interface IDishRepository
    {
        ICollection<Dish> GetDishes();
        Dish GetDish(Guid id);
        bool DishExist(Guid id);
        int GetTotalDishesCount();
        List<Dish> GetDishesPaginated(int page, int pageSize);
    }
}
