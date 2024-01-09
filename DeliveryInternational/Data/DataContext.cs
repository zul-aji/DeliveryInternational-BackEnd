using DeliveryInternational.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryInternational.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<DishInOrder> Dishins { get; set; }

    }
}
