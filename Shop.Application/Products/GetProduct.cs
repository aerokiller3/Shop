using System;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Domain.Models;

namespace Shop.Application.Products
{
    public class GetProduct
    {
        private readonly ApplicationDbContext _ctx;

        public GetProduct(ApplicationDbContext ctx)
        {
            _ctx = ctx;

        }

        public async Task<ProductViewModel> Do(string name)
        {
            var stocksOnHold = _ctx.StocksOnHold.Where(x => x.ExpiryDate < DateTime.Now).ToList();

            if (stocksOnHold.Count > 0)
            {
                var stocks = stocksOnHold.Select(x => x.StockId);
                var stockToReturn = _ctx.Stock.Where(x => stocks.Contains(x.Id));

                foreach (var stock in stockToReturn)
                {
                    stock.Qty = stock.Qty + stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id).Qty;
                }

                _ctx.StocksOnHold.RemoveRange(stocksOnHold);

                await _ctx.SaveChangesAsync();
            }

            

            return _ctx.Products
                .Include(x => x.Stock)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Where(x => x.Name == name)
                .Select(x => new ProductViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Images.Select(y=>y.Path),
                    Value = $"{x.Value:N2}",
                    Categories = x.Categories,

                    Stock = x.Stock.Select(y => new StockViewModel
                    {
                        Id = y.Id,
                        Description = y.Description,
                        Qty = y.Qty
                    })
                })
                .FirstOrDefault();
        }

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public IEnumerable<string> Image { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
            public IEnumerable<CategoryProduct> Categories { get; set; }
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }
}
