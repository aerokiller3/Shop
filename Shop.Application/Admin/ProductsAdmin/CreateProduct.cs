using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Admin.ProductsAdmin
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
            //var fileName = request.Name + ".jpg";
            //var savePath = Path.Combine(_env.ContentRootPath, "wwwroot", "images", fileName);
            //await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            //{
            //    await request.Image.CopyToAsync(fileStream);
            //}

            //using (var image = Image.Load(request.Image.OpenReadStream()))
            //{
            //    var newSize = ResizeImage(image, 800, 800);
            //    var aSize = newSize.Split(',');
            //    image.Mutate(h => h.Resize(Convert.ToInt32(aSize[1]), Convert.ToInt32(aSize[0])));
            //    await image.SaveAsync(savePath);
            //}

            //var index = 0;
            //foreach (var image in request.Images)
            //{
            //    var fileName = $"{image.FileName}_{index++}{Path.GetExtension(image.FileName)}";
            //    var savePath = Path.Combine(_env.ContentRootPath, "wwwroot", "images", fileName);

            //    using (var img = SixLabors.ImageSharp.Image.Load(image.OpenReadStream()))
            //    {
            //        var newSize = ResizeImage(img, 800, 800);
            //        var aSize = newSize.Split(',');
            //        img.Mutate(h => h.Resize(Convert.ToInt32(aSize[1]), Convert.ToInt32(aSize[0])));
            //        await img.SaveAsync(savePath);
            //    }
            //}

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Value = request.Value,
            };

            var categories = new List<Category>();

            foreach (var categoryId in request.CategoriesId)
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);
                categories.Add(category);
            }

            if (request.Images != null)
            {
                var results = await UploadImages(request.Images, request.Name);

                product.Images.AddRange(results.Select((path, index) => new Image
                {
                    Index = index,
                    Path = path
                }));

                //foreach (var image in request.Images)
                //{
                //    var fileName = request.Name + ".jpg";
                //    var savePath = Path.Combine(_env.ContentRootPath, "wwwroot", "images", fileName);
                //    await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                //    {
                //        await image.CopyToAsync(fileStream);
                //    }
                //}
            }

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

        private async Task<IEnumerable<string>> UploadImages(IEnumerable<IFormFile> files, string name)
        {
            var index = 0;
            var paths = new List<string>();
            //var fileName = $"{product.Name}_{index++}{Path.GetExtension(image.FileName)}";
            //var savePath = Path.Combine(_env.ContentRootPath, "wwwroot", "images", fileName);

            foreach (var image in files)
            {
                var fileName = name + $"_{index++}.jpg";
                var savePath = Path.Combine(_env.ContentRootPath, "wwwroot", "images", fileName);

                await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    await image.CopyToAsync(fileStream);
                }

                paths.Add(fileName);
            }

            //using (var img = SixLabors.ImageSharp.Image.Load(image.OpenReadStream()))
            //{
            //    var newSize = ResizeImage(img, 800, 800);
            //    var aSize = newSize.Split(',');
            //    img.Mutate(h => h.Resize(Convert.ToInt32(aSize[1]), Convert.ToInt32(aSize[0])));
            //    img.Save(savePath);
            //    paths.Add(fileName);
            //}

            return paths;
        }

        public class Request
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public IEnumerable<IFormFile> Images { get; set; }
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

        public string ResizeImage(SixLabors.ImageSharp.Image img, int height, int width)
        {
            if (img.Width > width || img.Height > height)
            {
                var widthRatio = (double)img.Width / (double)width;
                var heightRatio = (double)img.Height / (double)height;
                var ratio = Math.Max(widthRatio, heightRatio);
                var newWidth = (int)(img.Width / ratio);
                var newHeight = (int)(img.Height / ratio);

                return newHeight.ToString() + "," + newWidth.ToString();
            }
            else
            {
                return img.Height.ToString() + "," + img.Width.ToString();
            }
        }
    }
}
