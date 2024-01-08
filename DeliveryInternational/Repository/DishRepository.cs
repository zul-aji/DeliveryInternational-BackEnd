using DeliveryInternational.Data;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;

namespace DeliveryInternational.Repository
{
    public class DishRepository : IDishRepository
    {
        private readonly DataContext _context;
        public DishRepository(DataContext context)
        {
            _context = context;
        }

        public int GetTotalDishesCount()
        {
            return _context.Dishes.Count();
        }

        public List<Dish> GetDishesPaginated(int page, int pageSize)
        {
            return _context.Dishes
                .OrderBy(d => d.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Dish GetDish(Guid id)
        {
            return _context.Dishes.Where(d => d.DishId == id).FirstOrDefault();
        }

        public ICollection<Dish> GetDishes()
        {
            return _context.Dishes.OrderBy(d => d.DishId).ToList();
        }

        public bool DishExist(Guid id)
        {
            return _context.Dishes.Any(d => d.DishId == id);
        }
    }
}
