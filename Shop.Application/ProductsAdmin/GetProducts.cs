using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.ProductsAdmin
{
    public class GetProducts
    {
        private readonly ApplicationDbContext _ctx;

        public GetProducts(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ProductViewModel> Do() =>
            _ctx.Products
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Value = x.Value,
                    Categories = x.Categories
                });

        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Value { get; set; }
            public IEnumerable<CategoryProduct> Categories { get; set; }
        }
    }
}
