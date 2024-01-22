using DeliveryInternational.Data;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryInternational.Repository
{
    public class DishRepository : IDishRepository
    {
        private readonly DataContext _context;
        public DishRepository(DataContext context) { _context = context; }

        public string GetUserId(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user.UserId.ToString();
        }

        public int GetTotalDishesCount()
        {
            return _context.Dishes.Count();
        }

        public List<Dish> GetDishesPaginated(IEnumerable<Dish> sortedDish, int page, int pageSize)
        {
            return sortedDish
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Dish GetDish(Guid dishId)
        {
            return _context.Dishes.Where(d => d.DishId == dishId).FirstOrDefault();
        }

        public IEnumerable<Dish> FilterDishes(string[] categories, bool? vegetarian)
        {
            var query = _context.Dishes.AsQueryable();

            // Filter by categories
            if (categories != null && categories.Any())
            {
                query = query.Where(d => categories.Contains(d.Category));
            }

            // Filter by vegetarian
            if (vegetarian.HasValue)
            {
                query = query.Where(d => d.isVegetarian == vegetarian.Value);
            }

            return query.ToList();
        }

        public IEnumerable<Dish> SortDishes(IEnumerable<Dish> dishes, string sorting)
        {
            // Default sorting if not specified
            if (string.IsNullOrWhiteSpace(sorting))
            {
                return dishes.OrderBy(d => d.Name);
            }

            // Apply sorting based on the provided criteria
            switch (sorting)
            {
                case "NameAsc":
                    return dishes.OrderBy(d => d.Name);
                case "NameDesc":
                    return dishes.OrderByDescending(d => d.Name);
                case "PriceAsc":
                    return dishes.OrderBy(d => d.Price);
                case "PriceDesc":
                    return dishes.OrderByDescending(d => d.Price);
                case "RatingAsc":
                    return dishes.OrderBy(d => d.Rating);
                case "RatingDesc":
                    return dishes.OrderByDescending(d => d.Rating);
                default:
                    return dishes.OrderBy(d => d.Name);
            }
        }

        public ICollection<Dish> GetDishes()
        {
            return _context.Dishes.OrderBy(d => d.DishId).ToList();
        }

        public bool DishExist(Guid dishId)
        {
            return _context.Dishes.Any(d => d.DishId == dishId);
        }

        public bool CanAddRating(Guid userId, Guid dishId)
        {
            return _context.Orders
                .Any(order => order.UserId == userId
                            && order.Status == "Delivered"
                            && order.Dishes.Any(dish => dish.DishId == dishId));
        }

        public void AddRatingToDish(Guid dishId, Rating rating)
        {
            _context.Add(rating);
        }

        public void UpdateRating(Dish dish)
        {
            _context.Dishes.Update(dish);
            _context.SaveChanges();
        }

        public bool IsValidCategory(string category)
        {
            string[] validCategories = { "Wok", "Pizza", "Soup", "Dessert", "Drink" };
            return validCategories.Contains(category);
        }

        public bool IsValidSortingCriteria(string criteria)
        {
            string[] validSortingCriteria = { "NameAsc", "NameDesc", "PriceAsc", "PriceDesc", "RatingAsc", "RatingDesc" };
            return validSortingCriteria.Contains(criteria);
        }
    }
}
