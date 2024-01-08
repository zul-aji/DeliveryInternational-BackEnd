using DeliveryInternational.Models;

namespace DeliveryInternational.Interface
{
    public interface IBasketRepository
    {
        string GetUserId(string email);
        bool IsDishExist(string dishId);
        bool AddorUpdateBasket(string dishId, string userId);
    }
}
