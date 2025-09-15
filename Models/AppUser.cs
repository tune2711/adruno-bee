using System.ComponentModel.DataAnnotations;

namespace myapp.Models;

public class AppUser
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}
