using DeliveryInternational.Dto;
using DeliveryInternational.Models;

namespace DeliveryInternational.Interface
{
    public interface IUserRepository
    {
        User GetUserProfile(string userEmail);
        bool CreateUser(User user);
        bool UserLogin(UserLoginDto loginDto);
        bool UserLogout();
        bool UserExist(string email);
        bool IsEmailValid(string email);
        bool Save();
        (byte[] PasswordHash, byte[] PasswordSalt) GetPasswordHashAndSalt(string email);
    }
}
