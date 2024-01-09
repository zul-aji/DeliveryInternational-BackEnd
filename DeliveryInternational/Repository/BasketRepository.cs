using DeliveryInternational.Data;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;

namespace DeliveryInternational.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly DataContext _context;
        public BasketRepository(DataContext context)
        {
            _context = context;
        }

        public string GetUserId(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user.UserId.ToString();
        }

        public bool IsDishExist(string dishId)
        {
            Guid dishGuid = Guid.Parse(dishId);
            return _context.Dishes.Any(d => d.DishId == dishGuid);
        }

        public bool AddorUpdateBasket(string dishId, string userId)
        {
            Guid dishGuid = Guid.Parse(dishId);
            Guid userGuid = Guid.Parse(userId);
            // Check if a basket already exists for the given DishId and UserId
            var existingBasket = _context.Baskets
                .FirstOrDefault(b => b.DishId == dishGuid && b.UserId == userGuid);

            if (existingBasket != null)
            {
                // If the basket exists, increase the count
                existingBasket.Count += 1;
            }
            else
            {
                // If the basket doesn't exist, create a new one
                var newBasket = new Basket
                {
                    BasketId = Guid.NewGuid(),
                    DishId = dishGuid,
                    UserId = userGuid,
                    Count = 1
                };

                _context.Baskets.Add(newBasket);
            }

            // Save changes to the database
            var changesSaved = _context.SaveChanges() > 0;

            // Return true if changes were saved, indicating the basket was added or updated
            return changesSaved;
        }

        public List<Basket> GetBasketList(Guid userId)
        {
            return _context.Baskets.Where(b => b.UserId == userId).ToList();
        }

        public string GetDishNameById(Guid dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.DishId == dishId);
            return dish.Name;
        }

        public int GetDishPriceById(Guid dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.DishId == dishId);
            return dish.Price;
        }

        public string GetDishImageById(Guid dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.DishId == dishId);
            return dish.Image;
        }

        public bool DecreaseDishOnBasket(string dishId, string userId)
        {
            Guid dishGuid = Guid.Parse(dishId);
            Guid userGuid = Guid.Parse(userId);
            var existingBasket = _context.Baskets
                .FirstOrDefault(b => b.DishId == dishGuid && b.UserId == userGuid);

            if (existingBasket != null)
                existingBasket.Count -= 1;

            var changesSaved = _context.SaveChanges() > 0;
            return changesSaved;
        }

        public bool DeleteDishFromBasket(string dishId, string userId)
        {
            Guid dishGuid = Guid.Parse(dishId);
            Guid userGuid = Guid.Parse(userId);
            // Find the basket entry to delete
            var basketEntry = _context.Baskets.FirstOrDefault(b => b.DishId == dishGuid && b.UserId == userGuid);

            if (basketEntry != null)
            {
                // Remove the entry from the context
                _context.Baskets.Remove(basketEntry);

                // Save changes to the database
                return _context.SaveChanges() > 0;
            }

            return false; // Entry not found
        }
    }
}
