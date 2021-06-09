using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Accounts
{
    [ValidateAntiForgeryToken]
    public class ConfirmPasswordModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public ResetPasswordViewModel Model { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(
            [FromServices] UserManager<User> userManager,
            ResetPasswordViewModel model,
            string code,
            string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            var result = await userManager.ResetPasswordAsync(user, code, model.Password);

            if (result.Succeeded)
                return RedirectToPage("Accounts/Login");
            else
                return RedirectToPage("Error");
        }

        public class ResetPasswordViewModel
        {
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }
    }
}