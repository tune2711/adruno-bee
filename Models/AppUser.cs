using System.ComponentModel.DataAnnotations;

namespace myapp.Models;

public class AppUser
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty; // Warning: Passwords should be hashed in a real application.

    [Required]
    public string Role { get; set; } = string.Empty;
}
