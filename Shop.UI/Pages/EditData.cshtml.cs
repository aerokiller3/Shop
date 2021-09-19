using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application;
using Shop.Application.Users;
using Shop.Domain.Models;
using SmartBreadcrumbs.Attributes;

namespace Shop.UI.Pages
{
    [Breadcrumb("Редактирование персональных данных")]
    public class EditDataModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public new GetUser.UserViewModel User { get; set; }
        public async Task<IActionResult> OnGetAsync([FromServices] UserManager<User> userManager)
        {
            User = await new GetUser(userManager).Do(HttpContext.User.Identity.Name);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromServices] UserManager<User> userManager,
            [FromServices] SignInManager<User> signInManager)
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            user.Name = User.Name;
            user.SurName = User.SurName;
            user.Patronymic = User.Patronymic;

            if (user.Email != User.Email.Trim())
            {
                await userManager.SetEmailAsync(user, User.Email.Trim());
                await userManager.SetUserNameAsync(user, User.Email.Trim());

                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Page(
                    "/ConfirmPage",
                    null,
                    new { userId = user.Id, code },
                    HttpContext.Request.Scheme);

                var emailService = new ServiceMail();

                await emailService.SendEmailAsync(User.Email.Trim(), "Подтвердите ваш новый адрес электронной почты",
                    $"Подтвердите изменение электронной почты, перейдя по ссылке: <a href='{link}'>link</a>");

                await signInManager.SignOutAsync();

                return RedirectToPage("/VerifyEmail");
            }

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToPage("/Accounts/PersonalArea");
            else
                return RedirectToPage("/Error");
        }
    }
}
