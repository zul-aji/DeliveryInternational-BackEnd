using DeliveryInternational.Models;

namespace DeliveryInternational.Interface
{
    public interface IOrderRepository
    {
        ICollection<Order> GetOrders();
        Order GetOrder(Guid id);
        bool OrderExist(Guid id);
    }
}
