using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Database
{
    public class StockManager :IStockManager
    {
        private readonly ApplicationDbContext _ctx;
        public StockManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public Stock GetStockWithProduct(int stockId)
        {
            return _ctx.Stock
                .Include(x => x.Product)
                .FirstOrDefault(x => x.Id == stockId);
        }

        public bool EnoughStock(int stockId, int qty)
        {
            if (stockId == 0)
            {
                return false;
            }

            return _ctx.Stock.FirstOrDefault(x => x.Id == stockId).Qty >= qty;
        }

        public Task PutStockOnHold(int stockId, int qty, string sessionId)
        {
            //database responsibility to update stock
            _ctx.Stock.FirstOrDefault(x => x.Id == stockId).Qty -= qty;

            var stockOnHold = _ctx.StocksOnHold
                .Where(x => x.SessionId == sessionId)
                .ToList();

            if (stockOnHold.Any(x => x.StockId == stockId))
            {
                stockOnHold.Find(x => x.StockId == stockId).Qty += qty;
            }
            else
            {
                _ctx.StocksOnHold.Add(new StockOnHold
                {
                    StockId = stockId,
                    SessionId = sessionId,
                    Qty = qty,
                    ExpiryDate = DateTime.Now.AddMinutes(20)
                });
            }

            foreach (var stock in stockOnHold)
            {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            return _ctx.SaveChangesAsync();
        }

        public Task RemoveStockFromHold(int stockId, int qty, string sessionId)
        {
            var stockOnHold = _ctx.StocksOnHold
                .FirstOrDefault(x => x.StockId == stockId
                                     && x.SessionId == sessionId);

            var stock = _ctx.Stock.FirstOrDefault(x => x.Id == stockId);
            stock.Qty += qty;
            stockOnHold.Qty -= qty;

            if (stockOnHold.Qty <= 0)
            {
                _ctx.Remove(stockOnHold);
            }

            return _ctx.SaveChangesAsync();
        }
    }
}
