using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Users;
using Shop.Database;
using Shop.Domain.Models;
using SmartBreadcrumbs.Attributes;

namespace Shop.UI.Pages
{
    [Breadcrumb("История заказов")]
    public class UserOrdersModel : PageModel
    {
        private readonly ApplicationDbContext _ctx;

        public UserOrdersModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<GetUserOrders.OrderViewModel> Orders { get; set; }

        public async Task OnGet([FromServices] UserManager<User> userManager)
        { 
            Orders = await new GetUserOrders(_ctx, userManager).Do(HttpContext.User.Identity.Name);

            if (Orders == null)
            {
                RedirectToPage("/PersonalArea");
            }
        }
    }
}
