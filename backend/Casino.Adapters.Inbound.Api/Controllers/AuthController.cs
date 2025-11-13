using Casino.Adapters.Inbound.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Adapters.Inbound.Api.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok("Auth Service is healthy");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Placeholder for login logic
            if (request.Email == "admin@admin.com" && request.Password == "password")
            {
                return Ok(new { Token = "fake-jwt-token" });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Placeholder for registration logic
            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Placeholder for logout logic
            return Ok(new { Message = "User logged out successfully" });
        }
    }
}
