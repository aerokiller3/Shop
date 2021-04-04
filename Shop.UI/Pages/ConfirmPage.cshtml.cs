using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Domain.Models;

namespace Shop.UI.Pages
{
    public class ConfirmPageModel : PageModel
    {
        public async Task <IActionResult> OnGetAsync([FromServices] UserManager<User> userManager,string userId, string code)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return RedirectToPage("Error");

            var result = await userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return Page();

            return RedirectToPage("Error");
        }
    }
}
