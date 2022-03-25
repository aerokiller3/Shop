using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Admin.ProductsAdmin
{
    public class UpdateProduct
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _env;

        public UpdateProduct(ApplicationDbContext context, IHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<Response> Do(Request request)
        {
            var product = _context.Products
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Include(x => x.Images)
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

            if (product.Images.Any() && request.Images != null)
            {
                DeleteImages(product.Images);

                var images = await UploadImage(request.Images, request.Name);

                product.Images.AddRange(images.Select((path, index) => new Image
                {
                    Index = index,
                    Path = path
                }));
            }
            else if(!product.Images.Any() && request.Images != null)
            {
                var images = await UploadImage(request.Images, request.Name);

                product.Images.AddRange(images.Select((path, index) => new Image
                {
                    Index = index,
                    Path = path
                }));
            }

            product.Categories = categoryProduct;

            await _context.SaveChangesAsync();
            return new Response
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value,
                Images = product.Images,
                Categories = product.Categories
            };
        }

        private async Task<IEnumerable<string>> UploadImage(IEnumerable<IFormFile> images, string name)
        {
            var index = 0;
            var paths = new List<string>();

            foreach (var image in images)
            {
                var fileName = name + $"_{index++}.jpg";
                var savePath = Path.Combine(_env.ContentRootPath, "wwwroot", "images", fileName);

                await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    await image.CopyToAsync(fileStream);
                }

                paths.Add(fileName);
            }

            return paths;
        }

        private void DeleteImages(IEnumerable<Image> images)
        {
            foreach (var image in images)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", image.Path);

                if (File.Exists(path))
                {
                    File.Delete(path);
                    _context.Images.Remove(image);
                }
            }
        }

        public class Request
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public List<IFormFile> Images { get; set; }
            public IEnumerable<int> CategoriesId { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public List<Image> Images { get; set; }
            public List<CategoryProduct> Categories { get; set; }
        }
    }
}