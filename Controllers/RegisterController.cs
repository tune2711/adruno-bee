using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.Models;
using System.Threading.Tasks;

namespace myapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Register
        [HttpPost]
        public async Task<IActionResult> RegisterUser(AppUser appUser)
        {
            // Check if a user with the same email already exists
            if (await _context.AppUsers.AnyAsync(u => u.Email == appUser.Email))
            {
                return Conflict(new { message = "An account with this email already exists." });
            }

            // All new registrations will have the "khách hàng" role by default.
            appUser.Role = "khách hàng";

            // WARNING: In a real-world application, you MUST hash the password before saving it.
            _context.AppUsers.Add(appUser);
            await _context.SaveChangesAsync();

            // For security, don't return the password in the response.
            var userResponse = new { id = appUser.Id, email = appUser.Email, role = appUser.Role };

            return Ok(userResponse);
        }
    }
}
