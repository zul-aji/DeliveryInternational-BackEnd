using DeliveryInternational.Models;

namespace DeliveryInternational.Interface
{
    public interface IRatingRepository
    {
        bool AddRating(Rating rating);
        int CalculateRatingAvg(Guid dishId);
        void UpdateRating(Rating rating);
        Rating isRatingExist(Guid userId, Guid dishId);
        bool Save();
    }
}
