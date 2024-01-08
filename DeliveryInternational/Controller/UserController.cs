using AutoMapper;
using DeliveryInternational.Dto;
using DeliveryInternational.Interface;
using DeliveryInternational.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DeliveryInternational.Controller
{
    [Route("api/[controller]"), ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userInterface;
        private readonly IMapper _mapper;

        public UserController(IConfiguration configuration, IUserRepository userInterface, IMapper mapper)
        {
            _configuration = configuration;
            _userInterface = userInterface;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [SwaggerResponse(200, Type = typeof(UserRegisterDto))]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult Register([FromBody] UserRegisterDto request)
        {
            if (request == null)
                return BadRequest(ModelState);

            if (!_userInterface.IsEmailValid(request.Email))
                return BadRequest("Email is in the wrong format");           

            if (_userInterface.UserExist(request.Email))
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "400",
                    Message = $"Username {request.Email} is already taken."
                };

                return BadRequest(errorResponse);
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Map UserRegisterDto to User
            var user = _mapper.Map<User>(request);

            // Set password hash and salt
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Save user to the database
            if (_userInterface.CreateUser(user))
            {
                // User creation successful, generate token or any other response as needed
                string token = GenerateToken(request.Email);
                return Ok(new { Token = token });
            }
            else
            {
                // Failed to save user to the database
                var errorResponse = new ErrorResponse
                {
                    Status = "500",
                    Message = "Internal Server Error. Failed to create user."
                };

                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("login")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult UserLogin(UserLoginDto loginDto)
        {
            try
            {
                if (loginDto == null)
                    return BadRequest(ModelState);
                
                if (!_userInterface.IsEmailValid(loginDto.Email)) 
                    return BadRequest("Email is in the wrong format");

                if (!_userInterface.UserExist(loginDto.Email))
                    return BadRequest("User not found.");

                var (passwordHash, passwordSalt) = _userInterface.GetPasswordHashAndSalt(loginDto.Email);

                if (!VerifyPasswordHash(loginDto.Password, passwordHash, passwordSalt))
                {
                    return BadRequest("Wrong Password");
                }
                string token = GenerateToken(loginDto.Email);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "500",
                    Message = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("logout"), Authorize]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult UserLogout()
        {
            try
            {
                var jtiClaim = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                var response = new ErrorResponse
                {
                    Status = "200",
                    Message = "Logged out"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "500",
                    Message = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("profile"), Authorize]
        [SwaggerResponse(200, "Success", Type = typeof(UserProfileDto))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(500, "InternalServerError", Type = typeof(ErrorResponse))]
        public IActionResult GetUserProfile()
        {
            try
            {
                // Retrieve user information based on the token
                var userEmail = User.FindFirst(ClaimTypes.Email).Value;

                // Use the userEmail to fetch the user profile from your data source
                var userProfile = _userInterface.GetUserProfile(userEmail);

                // Map the user profile to a DTO (Data Transfer Object) if needed
                var userProfileDto = _mapper.Map<UserProfileDto>(userProfile);

                return Ok(userProfileDto);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Status = "500",
                    Message = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }


        private string GenerateToken(string userEmail)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userEmail)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
