﻿using System.Collections.Generic;
using System.Linq;
using Shop.Domain.Infrastructure;

namespace Shop.Application.Cart
{
    public class GetCart
    {
        private readonly ISessionManager _sessionManager;

        public GetCart(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public class Response
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public decimal RealValue { get; set; }
            public int Qty { get; set; }
            public string Image { get; set; }

            public int StockId { get; set; }
        }

        public IEnumerable<Response> Do()
        {
            return _sessionManager.GetCart().Select(x => new Response
            {
                Name = x.ProductName,
                Value = $"€ {x.Value:N2}",
                RealValue = x.Value,
                StockId = x.StockId,
                Qty = x.Qty,
                Image = x.Image
            });
        }
    }
}
