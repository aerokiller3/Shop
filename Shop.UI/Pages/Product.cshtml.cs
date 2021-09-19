using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Products;
using Shop.Database;
using SmartBreadcrumbs.Attributes;

namespace Shop.UI.Pages
{
    [Breadcrumb("ViewData.Title")]
    public class ProductModel : PageModel
    {
        private readonly ApplicationDbContext _ctx;

        public ProductModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        [BindProperty(SupportsGet = true)]
        public AddToCart.Request CartViewModel { get; set; }

        public GetProduct.ProductViewModel Product { get; set; }


        public async Task<IActionResult> OnGet(string name)
        {
            Product = await new GetProduct(_ctx).Do(name.Replace("-", " "));

            if (Product == null)
                return RedirectToPage("Index");
            else
            {
                ViewData["Title"] = Product.Name;
                return Page();
            }
        }

        public async Task<IActionResult> OnPost([FromServices] AddToCart addToCart, string name)
        {
            Product = await new GetProduct(_ctx).Do(name.Replace("-", " "));
            ViewData["Title"] = Product.Name;
            //ViewData["EnoughQty"] = "Такого количества товара нет";
            var stockAdded = await addToCart.Do(CartViewModel);

            if (stockAdded)
                return RedirectToPage("Cart");
            else
                return Page();
        }
    }
}
