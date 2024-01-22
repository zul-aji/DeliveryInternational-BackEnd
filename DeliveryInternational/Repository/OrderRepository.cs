using DeliveryInternational.Data;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using Microsoft.EntityFrameworkCore;
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

        public DateTime GetDeliverytime(Guid orderGuid)
        {
            return _context.Orders.FirstOrDefault(o => o.OrderId == orderGuid).DeliveryTime; 
        }

        public string GetAddress(Guid orderGuid)
        {
            return _context.Orders.FirstOrDefault(o => o.OrderId == orderGuid).Address;
        }

        public bool CreateOrder(Guid orderGuid, IEnumerable<BasketAndOrderDto> basketDtos, string email)
        {
            Order order = new()
            {
                OrderId = orderGuid,
                UserId = _context.Users.FirstOrDefault(u => u.Email == email).UserId,
                OrderTime = DateTime.UtcNow,
                DeliveryTime = DateTime.UtcNow.AddHours(1),
                Price = basketDtos.Sum(d => d.TotalPrice),
                Status = "InProcess",
                Dishes = basketDtos.Select(dto => new DishInOrder
                {
                    DishinId = Guid.NewGuid(),
                    DishId = dto.DishId,
                    DishName = dto.DishName,
                    DishPrice = dto.DishPrice,
                    TotalPrice = dto.TotalPrice,
                    Amount = dto.Amount,
                    DishImage = dto.DishImage
                }).ToList(),
                Address = _context.Users.FirstOrDefault(u => u.Email == email).Address
            };

            _context.Orders.Add(order);

            // Save changes to the database
            var changesSaved = _context.SaveChanges() > 0;

            // Return true if changes were saved, indicating the basket was added or updated
            return changesSaved;
        }

        public bool OrderExist(Guid id)
        {
            return _context.Orders.Any(o => o.OrderId == id);
        }

        public ICollection<Order> GetOrders(Guid userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Dishes)
                .OrderBy(o => o.OrderId)
                .ToList();
        }

        public Order GetOrder(Guid orderId)
        {
            return _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
