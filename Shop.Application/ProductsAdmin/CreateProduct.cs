using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class CreateProduct
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _env;

        public CreateProduct(ApplicationDbContext context, IHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<Response> Do(Request request)
        {
            var fileName = request.Name + ".jpg";
            var savePath = Path.Combine(_env.ContentRootPath, "wwwroot", "images", fileName);
            await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                await request.Image.CopyToAsync(fileStream);
            }

            var categories = new List<Category>();

            foreach (var categoryId in request.CategoriesId)
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);
                categories.Add(category);
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Value = request.Value,
                Image = fileName
            };

            var categoryProducts = new List<CategoryProduct>();

            foreach (var category in categories)
            {
                var line = new CategoryProduct
                {
                    ProductId = product.Id,
                    Product = product,

                    CategoryId = category.Id,
                    Category = category
                };

                categoryProducts.Add(line);
            }

            product.Categories = categoryProducts;

            _context.Products.Add(product);
            _context.CategoryProducts.AddRange(categoryProducts);

            await _context.SaveChangesAsync();

            return new Response
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value,
                Categories = product.Categories
            };
        }

        public class Request
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public IFormFile Image { get; set; }
            public IEnumerable<int> CategoriesId { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public List<CategoryProduct> Categories { get; set; }
        }
    }
}
