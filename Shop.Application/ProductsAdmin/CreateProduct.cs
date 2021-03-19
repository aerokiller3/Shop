using System.IO;
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

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Value = request.Value,
                Image = fileName
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return new Response
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value
            };
        }

        public class Request
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public IFormFile Image { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
