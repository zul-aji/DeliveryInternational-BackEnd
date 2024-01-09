using DeliveryInternational.Data;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using System.Text.RegularExpressions;

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

        public User GetUserProfile(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
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

        public bool IsPasswordValid(string password)
        {
            // Check if the password is at least 6 characters long
            if (password.Length < 6)
                return false;

            // Check if the password contains at least one letter and one number
            bool hasLetter = false;
            bool hasNumber = false;

            foreach (char character in password)
            {
                if (char.IsLetter(character))
                    hasLetter = true;
                else if (char.IsDigit(character))
                    hasNumber = true;

                // Break the loop early if both conditions are met
                if (hasLetter && hasNumber)
                    break;
            }

            // Return true if the password meets both criteria
            return hasLetter && hasNumber;
        }

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$";
            return Regex.IsMatch(phoneNumber, pattern);
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
