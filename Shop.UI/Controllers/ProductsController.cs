using Microsoft.AspNetCore.Mvc;
using Shop.Database;
using Shop.Application.Products;

namespace Shop.UI.Controllers
{
    [Route("")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public ProductsController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("")]
        public IActionResult GetProducts() => Ok(new GetProducts(_ctx).Do());
    }
}
