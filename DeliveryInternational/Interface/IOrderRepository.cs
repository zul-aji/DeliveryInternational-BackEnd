using DeliveryInternational.Dto;
using DeliveryInternational.Models;

namespace DeliveryInternational.Interface
{
    public interface IOrderRepository
    {
        ICollection<Order> GetOrders(Guid userId);
        bool CreateOrder(Guid orderGuid, IEnumerable<BasketAndOrderDto> basketDtos, string email);
        DateTime GetDeliverytime(Guid orderGuid);
        string GetAddress(Guid orderGuid);
        Order GetOrder(Guid orderId);
        bool OrderExist(Guid id);
        bool Save();
    }
}
