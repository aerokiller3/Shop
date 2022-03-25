using System.Collections.Generic;
using System.Linq;
using Shop.Database;
using Shop.Domain.Infrastructure;

namespace Shop.Application.Cart
{
    public class GetCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly ApplicationDbContext _ctx;

        public GetCart(ISessionManager sessionManager, ApplicationDbContext ctx)
        {
            _sessionManager = sessionManager;
            _ctx = ctx;
        }

        public class Response
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public decimal RealValue { get; set; }
            public int Qty { get; set; }
            public string Image { get; set; }

            public int StockId { get; set; }
            public string StockName { get; set; }
        }

        public IEnumerable<Response> Do()
        {
            return _sessionManager.GetCart(x => new Response
            {
                Name = x.ProductName,
                Value = $"{x.Value:N2} руб.",
                RealValue = x.Value,
                StockId = x.StockId,
                Qty = x.Qty,
                Image = "/images/" + x.Image,
                StockName = _ctx.Stock.FirstOrDefault(y => y.Id == x.StockId).Description
            });
        }
    }
}
