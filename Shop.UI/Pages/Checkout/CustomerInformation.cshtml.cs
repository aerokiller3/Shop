using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Shop.Application.Cart;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Checkout
{
    public class CustomerInformationModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public CustomerInformationModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        [BindProperty]
        public AddCustomerInformation.Request CustomerInformation { get; set; }

        public async Task <IActionResult> OnGet(
            [FromServices] GetCustomerInformation getCustomerInformation,
            [FromServices] UserManager<User> userManager)
        {
            var information = getCustomerInformation.Do();

            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (information == null)
            {
                if (_env.IsDevelopment())
                {
                    CustomerInformation = new AddCustomerInformation.Request
                    {
                        FirstName = user.Name,
                        LastName = user.SurName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address1 = "A",
                        Address2 = "A",
                        City = "A",
                        PostCode = "A",

                    };
                }

                return Page();
            }
            else
            {
                return RedirectToPage("/Checkout/Payment");
            }
        }

        public IActionResult OnPost([FromServices] AddCustomerInformation addCustomerInformation)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            addCustomerInformation.Do(CustomerInformation);

            return RedirectToPage("/Checkout/Payment");
        }
    }
}
