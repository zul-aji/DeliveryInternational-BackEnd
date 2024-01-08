using DeliveryInternational.Data;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using System.Security.Cryptography;

namespace DeliveryInternational.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;
        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public Order GetOrder(Guid id)
        {
            return _context.Orders.Where(o => o.OrderId == id).FirstOrDefault();
        }

        public ICollection<Order> GetOrders() 
        {
            return _context.Orders.OrderBy(o => o.OrderId).ToList();
        }

        public bool OrderExist(Guid id)
        {
            return _context.Orders.Any(o => o.OrderId == id);
        }
    }
}
