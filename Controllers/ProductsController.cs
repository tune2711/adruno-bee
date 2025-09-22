
using Microsoft.AspNetCore.Mvc;
using myapp.Models;
using System.Collections.Generic;
using System.Linq;

namespace myapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Mì xào trứng", Price = 35000, Description = "Mì gói xào với trứng, hành lá và gia vị đậm đà.", ImageUrl = "https://cdn2.fptshop.com.vn/unsafe/1920x0/filters:format(webp):quality(75)/2023_12_12_6383797141668849", Category = "Món chính" },
            new Product { Id = 2, Name = "Cơm rang dưa bò", Price = 45000, Description = "Cơm rang thơm lừng với thịt bò mềm và dưa cải chua.", ImageUrl = "https://i-giadinh.vnecdn.net/2023/10/17/Buoc-8-Thanh-pham-1-8-2323-1697527935.jpg", Category = "Món chính" },
            new Product { Id = 3, Name = "Chân gà sả tắc", Price = 50000, Description = "Chân gà giòn sần sật ngâm sả tắc chua cay.", ImageUrl = "https://file.hstatic.net/200000700229/article/lam-chan-ga-ngam-sa-tac-xoai-thumb_dafa66b5c7044a26bf70955c9c8f5ce6.jpg", Category = "Món ăn vặt" },
            new Product { Id = 4, Name = "Xôi xéo", Price = 25000, Description = "Xôi dẻo thơm với đỗ xanh, hành phi và mỡ hành.", ImageUrl = "https://statics.vinwonders.com/xoi-xeo-01%20(2)_1632322118.jpg", Category = "Món chính" },
            new Product { Id = 5, Name = "Bánh mì que", Price = 15000, Description = "Bánh mì giòn tan với pate cay đặc trưng.", ImageUrl = "https://daylambanh.edu.vn/wp-content/uploads/2019/09/banh-mi-que-nho-xinh-600x400.jpg", Category = "Món ăn vặt" },
            new Product { Id = 6, Name = "Trà tắc", Price = 15000, Description = "Thức uống giải nhiệt sảng khoái từ trà và tắc tươi.", ImageUrl = "https://tiki.vn/blog/wp-content/uploads/2023/10/tra-tac-2.jpg", Category = "Đồ uống" },
            new Product { Id = 7, Name = "Phở cuốn", Price = 60000, Description = "Bánh phở mềm cuốn thịt bò xào và rau thơm, chấm nước mắm chua ngọt.", ImageUrl = "https://golook.vn/blog/wp-content/uploads/2024/09/cach-lam-mon-pho-cuon-thit-bo-1-1.png", Category = "Món ăn vặt" },
            new Product { Id = 8, Name = "Nem chua rán", Price = 40000, Description = "Nem chua rán giòn rụm, béo ngậy, ăn kèm tương ớt.", ImageUrl = "https://rubissdetox.vn/wp-content/uploads/2023/01/Cach-lam-nem-chua-ran-1.jpg", Category = "Món ăn vặt" },
            new Product { Id = 9, Name = "Chè khúc bạch", Price = 25000, Description = "Chè thanh mát với khúc bạch phô mai, nhãn và hạnh nhân.", ImageUrl = "https://cdn2.fptshop.com.vn/unsafe/1920x0/filters:format(webp):quality(75)/2023_9_28_638315335535725712_che-khuc-bach-thumb.jpg", Category = "Tráng miệng" }
        };

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return Products;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            product.Id = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
            Products.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null || updatedProduct.Id != id)
            {
                return BadRequest();
            }

            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Description = updatedProduct.Description;
            product.ImageUrl = updatedProduct.ImageUrl;
            product.Category = updatedProduct.Category;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            Products.Remove(product);
            return NoContent();
        }
    }
}
