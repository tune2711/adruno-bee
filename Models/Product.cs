
namespace myapp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Price { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        public required string Category { get; set; }
    }
}
