using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParkingAPI.Models;
using ParkingAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        private static readonly Dictionary<string, string> RefreshTokens = new Dictionary<string, string>(); // Simple in-memory store for demo purposes

        public AuthController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login login)
        {
            // Validate the user credentials
            var user = _userService.ValidateUser(login.Username, login.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Generate refresh token
            var refreshToken = Guid.NewGuid().ToString();
            RefreshTokens[refreshToken] = user.Username; // Store the refresh token

            return Ok(new
            {
                Token = tokenString,
                RefreshToken = refreshToken,
                User = new
                {
                    user.Username,
                    user.Email,
                    user.DateOfBirth,
                    user.FirstName,
                    user.LastName
                    // Add other user properties as needed
                }
            });
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Validate refresh token
            if (!RefreshTokens.TryGetValue(request.RefreshToken, out var username))
            {
                return Unauthorized();
            }

            // Validate user
            var user = _userService.ValidateUser(username, null); // You might want to fetch user by username directly or use a different method to validate
            if (user == null)
            {
                return Unauthorized();
            }

            // Generate new JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = tokenString
            });
        }

        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] string refreshToken)
        {
            // Remove refresh token
            RefreshTokens.Remove(refreshToken);

            return Ok(new { message = "Logged out successfully" });
        }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }
}
