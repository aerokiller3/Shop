using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application;
using Shop.Domain.Models;

namespace Shop.UI.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public RegisterViewModel RegisterModel { get; set; }
        public void OnGet(string returnUrl)
        {
            RegisterModel = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
        }

        public async Task<IActionResult> OnPostAsync(
            [FromServices] UserManager<User> userManager)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new User
            {
                SurName = RegisterModel.SurName,
                Name = RegisterModel.Name,
                Patronymic = RegisterModel.Patronymic,
                UserName = RegisterModel.Email,
                Email = RegisterModel.Email,
                Birthday = RegisterModel.BirthDay,
                PhoneNumber = RegisterModel.Phone
            };

            var result = await userManager.CreateAsync(user, RegisterModel.Password);

            var userClaim = new Claim("Role", "User");
            await userManager.AddClaimAsync(user, userClaim);

            if (result.Succeeded)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Page(
                    "ConfirmPage",
                    "",
                    new {userId = user.Id, code},
                    HttpContext.Request.Scheme);

                var emailService = new ServiceMail();

                await emailService.SendEmailAsync(user.Email, "����������� ���� ����������� �����",
                    $"����������� �����������, ������� �� ������: <a href='{link}'>link</a>");

                return RedirectToPage("VerifyEmail");
            }

            return Page();
        }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "�������")]
        public string SurName { get; set; }
        [Required]
        [Display(Name = "���")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "��������")]
        public string Patronymic { get; set; }
        [Required]
        [Display(Name = "����� ��������")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "����������� �����")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "���� ��������")]
        public DateTime BirthDay { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "������")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "������ �� ���������")]
        [Display(Name = "������� ������ ��� ���")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
