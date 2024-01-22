using DeliveryInternational.Data;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;

namespace DeliveryInternational.Repository
{
    public class RatingRepository : IRatingRepository
    {
        public readonly DataContext _context;
        public RatingRepository(DataContext context) { _context = context; }

        public bool AddRating(Rating rating)
        {
            _context.Add(rating);
            return Save();
        }

        public int CalculateRatingAvg(Guid dishId)
        {
            var ratings = _context.Ratings
                .Where(r => r.DishId == dishId)
                .ToList();

            if (ratings.Any())
            {
                // Calculate the average rating value
                var averageRating = (int)ratings.Average(r => r.Value);
                return averageRating;
            }

            // If no ratings are found, return a default value (e.g., 0 or -1)
            return 0;
        }

        public Rating isRatingExist(Guid userId, Guid dishId)
        {
            return _context.Ratings.FirstOrDefault(r => r.UserId == userId && r.DishId == dishId);
        }

        public void UpdateRating(Rating rating)
        {
            _context.Ratings.Update(rating);
            _context.SaveChanges();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
