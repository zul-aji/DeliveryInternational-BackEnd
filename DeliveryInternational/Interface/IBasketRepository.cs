using DeliveryInternational.Models;

namespace DeliveryInternational.Interface
{
    public interface IBasketRepository
    {
        string GetUserId(string email);
        bool IsDishExist(string dishId);
        bool AddorUpdateBasket(string dishId, string userId);
        List<Basket> GetBasketList(Guid userId);
        string GetDishNameById(Guid dishId);
        int GetDishPriceById(Guid dishId);
        string GetDishImageById(Guid dishId);
        bool DecreaseDishOnBasket(string dishId, string userId);
        bool DeleteDishFromBasket(string dishId, string userId);
    }
}
