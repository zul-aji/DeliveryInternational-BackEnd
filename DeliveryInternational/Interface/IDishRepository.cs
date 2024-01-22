using DeliveryInternational.Models;

namespace DeliveryInternational.Interface
{
    public interface IDishRepository
    {
        string GetUserId(string email);
        ICollection<Dish> GetDishes();
        IEnumerable<Dish> FilterDishes(string[] categories, bool? vegetarian);
        IEnumerable<Dish> SortDishes(IEnumerable<Dish> dishes, string sorting);
        Dish GetDish(Guid dishId);
        bool DishExist(Guid dishId);
        int GetTotalDishesCount();
        List<Dish> GetDishesPaginated(IEnumerable<Dish> sortedDish, int page, int pageSize);
        void UpdateRating(Dish dish);
        bool CanAddRating(Guid userId, Guid dishId);
        void AddRatingToDish(Guid dishId, Rating rating);
        bool IsValidCategory(string category);
        bool IsValidSortingCriteria(string criteria);
    }
}
