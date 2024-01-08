using DeliveryInternational.Data;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DeliveryInternational.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();

        }

        public bool UserLogin(UserLoginDto loginDto) 
        {
            _context.Add(loginDto);
            return Save();
        }

        public bool UserExist(string email) 
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsEmailValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public User GetUserProfile(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public (byte[] PasswordHash, byte[] PasswordSalt) GetPasswordHashAndSalt(string email)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            return (user.PasswordHash, user.PasswordSalt);
        }
    }
}
