using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application;
using Shop.Domain.Models;

namespace Shop.UI.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty]
        public ForgotPasswordViewModel Model { get; set; }

        public async Task<IActionResult> OnPostAsync([FromServices] UserManager<User> userManager)
        {
            var user = await userManager.FindByEmailAsync(Model.Email);

            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                return RedirectToPage("Error");

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Page(
                "/Accounts/ConfirmPassword",
                null,
                new { userId = user.Id, code },
                HttpContext.Request.Scheme);

            var emailService = new ServiceMail();

            await emailService.SendEmailAsync(user.Email,"Сброс пароля",
                $"Для сброса пароля пройдите по ссылке: <a href='{link}'>link</a>");

            return RedirectToPage("VerifyEmail");
        }

        public class ForgotPasswordViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}
