using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Admin.CategoriesAdmin;
using Shop.Database;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "Manager")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public CategoriesController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("")]
        public IActionResult GetCategories() => Ok(new GetCategories(_ctx).Do());

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id) => Ok(new GetCategory(_ctx).Do(id));

        [HttpPost("")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategory.Request request) =>
            Ok((await new CreateCategory(_ctx).Do(request)));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id) => Ok((await new DeleteCategory(_ctx).Do(id)));

        [HttpPut("")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategory.Request request) =>
            Ok((await new UpdateCategory(_ctx).Do(request)));
    }
}
