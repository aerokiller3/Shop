using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly ApplicationDbContext _ctx;

        public AddToCart(ISessionManager sessionManager, ApplicationDbContext ctx)
        {
            _sessionManager = sessionManager;
            _ctx = ctx;
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public interface IStockManager
        {
            Stock GetStockWithProduct(int stockId);
            bool EnoughStock(int stockId, int qty);
            Task PutStockOnHold(int stockId, int qty, string sessionId);
        }

        public class StockManager : IStockManager
        {
            private ApplicationDbContext _ctx;
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
        }

        public async Task<bool> Do(Request request)
        {
            IStockManager stockManager = new StockManager(_ctx);

            //service responsibility
            if (!stockManager.EnoughStock(request.StockId, request.Qty))
            {
                return false;
            }

            await stockManager
                .PutStockOnHold(request.StockId, request.Qty, _sessionManager.GetId());

            var stock = stockManager.GetStockWithProduct(request.StockId);

            var cartProduct = new CartProduct()
            {
                ProductId = stock.ProductId,
                ProductName = stock.Product.Name,
                StockId = stock.Id,
                Qty = request.Qty,
                Value = stock.Product.Value,
                Image = stock.Product.Image
            };

            _sessionManager.AddProduct(cartProduct);

            return true;
        }
    }
}
