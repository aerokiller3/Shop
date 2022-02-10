using System.Linq;
using System.Threading.Tasks;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly IStockManager _stockManager;

        public AddToCart(ISessionManager sessionManager, IStockManager stockManager)
        {
            _sessionManager = sessionManager;
            _stockManager = stockManager;
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            //service responsibility
            if (!_stockManager.EnoughStock(request.StockId, request.Qty))
            {
                return false;
            }

            await _stockManager
                .PutStockOnHold(request.StockId, request.Qty, _sessionManager.GetId());

            var stock = _stockManager.GetStockWithProduct(request.StockId);

            var cartProduct = new CartProduct()
            {
                ProductId = stock.ProductId,
                ProductName = stock.Product.Name,
                StockId = stock.Id,
                Qty = request.Qty,
                Value = stock.Product.Value,
                Image = stock.Product.Images.FirstOrDefault().Path
            };

            _sessionManager.AddProduct(cartProduct);

            return true;
        }
    }
}
