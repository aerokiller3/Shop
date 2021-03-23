using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class UpdateProduct
    {
        private readonly ApplicationDbContext _context;

        public UpdateProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Do(Request request)
        {
            var product = _context.Products
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .FirstOrDefault(x => x.Id == request.Id);
            var categories = new List<Category>();

            foreach (var categoryId in request.CategoriesId)
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);
                categories.Add(category);
            }
            var categoryProduct = new List<CategoryProduct>();
            foreach (var category in categories)
            {
                var line = new CategoryProduct
                {
                    ProductId = product.Id,
                    Product = product,
                    CategoryId = category.Id,
                    Category = category
                };

                categoryProduct.Add(line);
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Value = request.Value;
            product.Image = request.Image;
            product.Categories = categoryProduct;

            await _context.SaveChangesAsync();
            return new Response
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value,
                Image = product.Image,
                Categories = product.Categories
            };
        }

        public class Request
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public string Image { get; set; }
            public IEnumerable<int> CategoriesId { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public string Image { get; set; }
            public List<CategoryProduct> Categories { get; set; }
        }
    }
}