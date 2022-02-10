using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;

namespace Shop.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly GetCart _getCart;

        public CartViewComponent(GetCart getCart)
        {
            _getCart = getCart;
        }
        public IViewComponentResult Invoke(string view = "Default")
        {
            if (view == "Small")
            {
                var totalValue = _getCart.Do().Sum(x => x.RealValue * x.Qty);

                return View(view, $"{totalValue}");
            }
            else if(view == "MobileCart")
            {
                return View(view, _getCart.Do());
            }

            return View(view, _getCart.Do());
        }
    }
}