﻿using DeliveryInternational.Data;
using DeliveryInternational.Dto;
using DeliveryInternational.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryInternational
{
    public class Seed
    {
        private readonly DataContext _dataContext;

        public Seed(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void SeedDataContext()
        {
            if (!_dataContext.Dishes.Any() && !_dataContext.Users.Any() && !_dataContext.Orders.Any())
            {
                // Seed Dishes
                var dishes = new List<Dish>
                {
                    new Dish
                    {
                        DishId = Guid.NewGuid(),
                        Name = "Chicken Wok",
                        Description = "Delicious Chicken Wok Dish",
                        Price = 10,
                        isVegetarian = false,
                        Image = "wok_chicken.jpg",
                        Rating = 0,
                        Category = "Wok",
                    },
                    new Dish
                    {
                        DishId = Guid.NewGuid(),
                        Name = "Beef Wok",
                        Description = "Delicious Beef Wok Dish",
                        Price = 10,
                        isVegetarian = false,
                        Image = "wok_beef.jpg",
                        Rating = 0,
                        Category = "Wok",
                    },
                    new Dish
                    {
                        DishId = Guid.NewGuid(),
                        Name = "Choco Lava",
                        Description = "Delicious chocolate cake",
                        Price = 10,
                        isVegetarian = false,
                        Image = "Choco_lava.jpg",
                        Rating = 0,
                        Category = "Dessert",
                    },

                };
                _dataContext.Dishes.AddRange(dishes);

                // Seed Users
                var users = new List<User>
                {
                    new User
                    {
                        UserId = Guid.NewGuid(),
                        FullName = "John Doe",
                        BirthDate = new DateTime(1990, 1, 1),
                        Gender = "Male",
                        Phone = "1234567890",
                        Email = "john.doe@example.com",
                        Address = "123 Main Street",
                        PasswordHash = new byte[] { /* Password hash bytes */ },
                        PasswordSalt = new byte[] { /* Password salt bytes */ }
                    },
                    // Add more users as needed
                };
                _dataContext.Users.AddRange(users);

                // Seed Orders
                var orders = new List<Order>
                {
                    new Order
                    {
                        OrderId = Guid.NewGuid(),
                        DeliveryTime = DateTime.UtcNow.AddDays(2),
                        OrderTime = DateTime.UtcNow,
                        Status = "InProcess",
                        Price = 20,
                        Dishes = new List<DishInOrder>
                        {
                            new DishInOrder
                            {
                                DishinId = Guid.NewGuid(),
                                DishId = dishes.First().DishId,
                                DishName = dishes.First().Name,
                                DishPrice = dishes.First().Price,
                                TotalPrice = 0,
                                Amount = 0,
                                DishImage = dishes.First().Image,
                            },
                        },
                        Address = "456 Oak Street"
                    },
                    // Add more orders as needed
                };
                _dataContext.Orders.AddRange(orders);

                _dataContext.SaveChanges();
            }
        }
    }
}
