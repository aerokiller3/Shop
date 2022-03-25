using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Admin.ProductsAdmin
{
    public class GetProduct
    {
        private readonly ApplicationDbContext _ctx;

        public GetProduct(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public ProductViewModel Do(int id) =>
            _ctx.Products
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Include(x => x.Images)
                .Where(x => x.Id == id).Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value,
                    Images = x.Images.Select(y => new Image
                    {
                        Id = y.Id,
                        Path = "/images/" + y.Path,
                        Index = y.Index
                    }).ToList(),
                    Categories = x.Categories.Select(y => y.Category).ToList()
                })
                .FirstOrDefault();

        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public List<Image> Images { get; set; }
            public List<Category> Categories { get; set; }
        }
    }
}
