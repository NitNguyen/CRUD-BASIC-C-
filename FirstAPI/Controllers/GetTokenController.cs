using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTokenController : ControllerBase
    {
        [HttpGet]
        public IActionResult getToken()
        {
            var secretKey = "zaCELgL.0imfnc8mVLWwsAawjYr4Rx-Af50DDqtlx";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Token expiration time
            var expiration = DateTime.UtcNow.AddHours(1);

            // Claims: These can include user-related information
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "username123"),
            new Claim(ClaimTypes.Role, "user123"),
            // Add other claims as needed
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }
    }
}
