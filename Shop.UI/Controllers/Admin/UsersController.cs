using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Admin.UsersAdmin;
using Shop.Database;

namespace Shop.UI.Controllers.Admin
{
    [Route("[controller]")]
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        private readonly CreateUser _createUser;
        private readonly ApplicationDbContext _ctx;

        public UsersController(CreateUser createUser, ApplicationDbContext ctx)
        {
            _createUser = createUser;
            _ctx = ctx;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser.Request request)
        {
            await _createUser.Do(request);

            return Ok();
        }

        [HttpGet("")]
        public IActionResult GetUsers() => Ok(new GetUsers(_ctx).Do());
    }
}
