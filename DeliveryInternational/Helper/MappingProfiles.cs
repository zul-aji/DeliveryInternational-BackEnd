﻿using AutoMapper;
using DeliveryInternational.Dto;
using DeliveryInternational.Models;

namespace DeliveryInternational.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Order, GetOrdersDto>();
            CreateMap<Order, GetOrderDto>();
            CreateMap<Dish, GetDishesDto>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<User, UserProfileDto>();
        }
    }
}
