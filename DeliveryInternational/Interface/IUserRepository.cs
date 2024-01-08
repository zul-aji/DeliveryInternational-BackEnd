using DeliveryInternational.Dto;
using DeliveryInternational.Models;

namespace DeliveryInternational.Interface
{
    public interface IUserRepository
    {
        bool CreateUser(User user);
        bool UserLogin(UserLoginDto loginDto);
        User GetUserProfile(string userEmail);
        void UpdateUser(User user);
        bool UserExist(string email);
        bool IsEmailValid(string email);
        bool IsPasswordValid(string password);
        bool IsValidPhoneNumber(string phoneNumber);
        bool Save();
        (byte[] PasswordHash, byte[] PasswordSalt) GetPasswordHashAndSalt(string email);
    }
}
