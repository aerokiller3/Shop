using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Domain.Models;


//TODO: œ–Œ¬≈–»“‹!!!
namespace Shop.UI.Pages
{
    public class EditPasswordModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public ChangePasswordViewModel ChangePasswordModel { get; set; }
        public async Task<IActionResult> OnGetAsync([FromServices] UserManager<User> userManager)
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ChangePasswordModel.Id = user.Id;
            ChangePasswordModel.Email = user.Email;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromServices] UserManager<User> userManager)
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var result = await userManager.ChangePasswordAsync(user, ChangePasswordModel.OldPassword,
                ChangePasswordModel.NewPassword);

            if (result.Succeeded)
                return RedirectToPage("/Accounts/PersonalArea");
            else
                return Page();
        }

        public class ChangePasswordViewModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmNewPassword { get; set; }
        }
    }
}
