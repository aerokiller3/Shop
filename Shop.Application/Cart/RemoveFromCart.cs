﻿using System.Linq;
using System.Threading.Tasks;
using Shop.Domain.Infrastructure;
using Shop.Database;

namespace Shop.Application.Cart
{
    public class RemoveFromCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly ApplicationDbContext _ctx;

        public RemoveFromCart(ApplicationDbContext ctx, ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            _ctx = ctx;
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
            public bool All { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            var stockOnHold = _ctx.StocksOnHold
                .FirstOrDefault(x => x.StockId == request.StockId
                                     && x.SessionId == _sessionManager.GetId());

            var stock = _ctx.Stock.FirstOrDefault(x => x.Id == request.StockId);

            if (request.All)
            {
                stock.Qty += stockOnHold.Qty;
                _sessionManager.RemoveProduct(request.StockId, stockOnHold.Qty);
                stockOnHold.Qty = 0;
            }
            else
            {
                stock.Qty += request.Qty;
                stockOnHold.Qty -= request.Qty;
                _sessionManager.RemoveProduct(request.StockId, request.Qty);
            }

            if (stockOnHold.Qty <= 0)
                _ctx.Remove(stockOnHold);

            await _ctx.SaveChangesAsync();

            return true;
        }
    }
}