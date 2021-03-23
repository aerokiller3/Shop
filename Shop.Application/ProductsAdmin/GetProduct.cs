using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
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
                .Where(x => x.Id == id).Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value,
                    Image = x.Image,
                    Categories = x.Categories
                })
                .FirstOrDefault();

        public class ProductViewModel
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
