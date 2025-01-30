using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebBackendAPI.Data;
using WebBackendAPI.Models;


namespace WebBackendAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] Data.User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest(new { message = "Email already exists" });
            }

            user.Password = HashPassword(user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Signup successful" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Check if the user exists by email
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Hash the input password and compare with the stored hash
            string hashedPassword = HashPassword(request.Password);
            if (user.Password != hashedPassword)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Return success message (you may also return a JWT token here)
            return Ok(new { message = "Login successful" });
        }

        // Helper method to hash passwords
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
