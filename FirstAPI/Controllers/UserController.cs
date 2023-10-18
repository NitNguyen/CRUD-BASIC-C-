using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models;
using FirstAPI.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BCrypt.Net;
using FirstAPI.Tool;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UserController> _logger;
        private readonly TokenGenerator tokenGenerator;

        public UserController(ApplicationDbContext dbContext, ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            tokenGenerator = new TokenGenerator();

        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                var existingUser = _dbContext.users.FirstOrDefault(u => u.userName == user.userName || u.userId == user.userId);
                if (existingUser != null)
                {
                    return BadRequest(new { Message = "User with the same username already exists." });
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password, salt);

                user.password = hashedPassword;
                user.salt = salt;

                _dbContext.users.Add(user);
                _dbContext.SaveChanges();

                user.password = null;
                user.salt = null;

                return Ok(new { Message = "User created successfully", User = user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user.");
                return StatusCode(500, new { Message = "An error occurred while creating the user." });
            }
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            try
            {
                var authenticatedUser = AuthenticateUser(user.userName, user.password);

                if (authenticatedUser != null)
                {
                    var tokenString = tokenGenerator.GenerateToken(authenticatedUser.userName, "role");
                    return Ok(new { AccessToken = tokenString });
                }

                return Unauthorized(new { Message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in.");
                return StatusCode(500, new { Message = "An error occurred while logging in." });
            }
        }

        private User AuthenticateUser(string username, string password)
        {
            var user = _dbContext.users.FirstOrDefault(u => u.userName == username);

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.password))
                {
                    return user;
                }
            }

            return null;
        }
    }
}
