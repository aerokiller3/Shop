using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Products
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
                .Include(x => x.Stock)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Select(x => new ProductViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Value = $"{x.Value:N2}",
                    Image = x.Image,
                    Categories = x.Categories,

                    StockCount = x.Stock.Sum(y => y.Qty)
                })
                .ToList();

        public IEnumerable<ProductViewModel> Do(List<int> categoryIds)
        {
            if (categoryIds.Count == 0)
            {
                var products = Do();
                return products;
            }
            else
            {
                var products = _ctx.Categories
                    .Where(c => categoryIds.Contains(c.Id))
                    .SelectMany(c => c.Products)
                    .Select(x => new ProductViewModel
                    {
                        Name = x.Product.Name,
                        Description = x.Product.Description,
                        Value = $"{x.Product.Value:N2}",
                        Image = x.Product.Image,

                        StockCount = x.Product.Stock.Sum(s => s.Qty),
                    })
                    .Distinct()
                    .ToList();

                return products;
            }
        }

        public IEnumerable<ProductViewModel> Do(string name)
        {
            var products = _ctx.Products.Where(p => name.Contains(p.Name))
                .Select(x => new ProductViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Value = $"{x.Value:N2}",
                    Image = x.Image,

                    StockCount = x.Stock.Sum(s => s.Qty),
                    Categories = x.Categories
                });

            return products;
        }

        public IEnumerable<ProductViewModel> ProductsFromCategory(int categoryId)
        {
            if (categoryId == 0)
                return null;

            var products = _ctx.Categories
                .Where(c => categoryId.Equals(c.Id))
                .SelectMany(c => c.Products)
                .Select(x => new ProductViewModel
                {
                    Name = x.Product.Name,
                    Description = x.Product.Description,
                    Value = $"{x.Product.Value:N2}",
                    Image = x.Product.Image,

                    StockCount = x.Product.Stock.Sum(s => s.Qty),
                })
                .Distinct()
                .ToList();

            return products;
        }

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public string Image { get; set; }
            public int StockCount { get; set; }
            public List<CategoryProduct> Categories { get; set; }
        }
    }
}
