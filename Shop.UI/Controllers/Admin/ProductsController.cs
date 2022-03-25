using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Admin.ProductsAdmin;
using Shop.Database;

namespace Shop.UI.Controllers.Admin
{
    [Route("[controller]")]
    [Authorize(Policy = "Manager")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext ctx, IWebHostEnvironment env)
        {
            _ctx = ctx;
            _env = env;
        }

        [HttpGet("")]
        public IActionResult GetProducts() => Ok(new GetProducts(_ctx).Do());

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) => Ok(new GetProduct(_ctx).Do(id));

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProduct.Request request) => Ok((await new CreateProduct(_ctx, _env).Do(request)));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) => Ok((await new DeleteProduct(_ctx).Do(id)));

        [HttpPut("")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProduct.Request request) =>
            Ok((await new UpdateProduct(_ctx, _env).Do(request)));
    }
}
