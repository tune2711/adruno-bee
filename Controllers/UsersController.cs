using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myapp.Data;
using myapp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace myapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize] // Allow any authenticated user to view all users
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.AppUsers.ToListAsync();
            return Ok(users);
        }

        // POST: api/Users (Register User)
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostUser(AppUser appUser)
        {
            // Set role based on email
            if (appUser.Email.ToLower() == "admin@gmail.com")
            {
                appUser.Role = "admin";
            }
            else
            {
                appUser.Role = "khách hàng";
            }

            // In a real application, you would hash the password before saving.
            _context.AppUsers.Add(appUser);
            await _context.SaveChangesAsync();

            // Return the created user, excluding the password for security
            var userResponse = new { id = appUser.Id, email = appUser.Email, role = appUser.Role };
            return CreatedAtAction(nameof(GetUsers), new { id = appUser.Id }, userResponse);
        }

        // PUT: api/Users/{id}/role
        [HttpPut("{id}/role")]
        [Authorize(Roles = "admin")] // Only admins can change the role
        public async Task<IActionResult> UpdateUserRole(int id, UpdateUserRoleRequest request)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Role = request.Role;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Users/{id}/password
        [HttpPut("{id}/password")]
        [Authorize(Roles = "admin")] // Only admins can change the password
        public async Task<IActionResult> UpdateUserPassword(int id, UpdatePasswordRequest request)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // In a real application, you would hash the new password before saving.
            user.Password = request.Password;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        // [Authorize(Roles = "admin")] // Temporarily removed for fixing data
        public async Task<IActionResult> DeleteUser(int id)
        {
            var appUser = await _context.AppUsers.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }

            _context.AppUsers.Remove(appUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("delete-by-email/{email}")]
        public async Task<IActionResult> DeleteUsersByEmail(string email)
        {
            var usersToDelete = _context.AppUsers.Where(u => u.Email == email);
            if (!usersToDelete.Any())
            {
                return NotFound(new { message = "No users found with this email." });
            }

            _context.AppUsers.RemoveRange(usersToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Successfully deleted all users with email {email}." });
        }

        private bool UserExists(int id)
        {
            return _context.AppUsers.Any(e => e.Id == id);
        }
    }

    public class UpdateUserRoleRequest
    {
        public string Role { get; set; } = string.Empty;
    }

    public class UpdatePasswordRequest
    {
        public string Password { get; set; } = string.Empty;
    }
}
