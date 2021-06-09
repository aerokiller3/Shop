using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.CategoriesAdmin;
using Shop.Database;

namespace Shop.UI.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _ctx;

        public IndexModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<GetCategories.CategoryViewModel> Categories { get; set; }
        public void OnGet()
        {
            Categories = new GetCategories(_ctx).Do();
        }
    }
}
