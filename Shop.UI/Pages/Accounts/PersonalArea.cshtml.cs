using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Users;
using Shop.Domain.Models;
using SmartBreadcrumbs.Attributes;

namespace Shop.UI.Pages.Accounts
{
    [ValidateAntiForgeryToken]
    [Breadcrumb("Личный кабинет")]
    public class PersonalAreaModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public new GetUser.UserViewModel User { get; set; }
        public async Task<IActionResult> OnGetAsync([FromServices] UserManager<User> userManager)
        {
            User = await new GetUser(userManager).Do(HttpContext.User.Identity.Name);

            return Page();
        }
    }
}
