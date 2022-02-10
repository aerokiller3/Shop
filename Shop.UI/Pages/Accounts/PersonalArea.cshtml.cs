using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application;
using Shop.Application.Users;
using Shop.Database;
using Shop.Domain.Models;
using SmartBreadcrumbs.Attributes;

namespace Shop.UI.Pages.Accounts
{
    [ValidateAntiForgeryToken]
    [Breadcrumb("Личный кабинет")]
    public class PersonalAreaModel : PageModel
    {
        private readonly ApplicationDbContext _ctx;

        [BindProperty(SupportsGet = true)]
        public new GetUser.UserViewModel User { get; set; }
        [BindProperty]
        public IEnumerable<GetUserOrders.OrderViewModel> Order { get; set; }

        public PersonalAreaModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> OnGetAsync([FromServices] UserManager<User> userManager)
        {
            User = await new GetUser(userManager).Do(HttpContext.User.Identity.Name);
            Order = await new GetUserOrders(_ctx, userManager).Do(HttpContext.User.Identity.Name);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromServices] UserManager<User> userManager, [FromServices] SignInManager<User> signInManager)
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            user.SurName = User.SurName;
            user.Name = User.Name;
            user.Patronymic = User.Patronymic;
            user.PhoneNumber = User.Phone;

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

        public async Task<IActionResult> OnPostChangePasswordAsync([FromServices] UserManager<User> userManager)
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var result = await userManager.ChangePasswordAsync(user, User.OldPassword,
                User.NewPassword);

            if (result.Succeeded)
            {
                ViewData["Message"] = "Пароль успешно изменён!";
                return Page();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }
    }
}
