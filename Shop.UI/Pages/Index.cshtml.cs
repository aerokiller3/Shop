using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Categories;
using Shop.Application.Products;
using Shop.Database;

namespace Shop.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _ctx;
        public IndexModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<GetProducts.ProductViewModel> Products { get; set; }
        public IEnumerable<GetCategories.CategoryViewModel> Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        public void OnGet()
        {
            Products = Name == null ? new GetProducts(_ctx).Do() : new GetProducts(_ctx).Do(Name); 
            Categories = new GetCategories(_ctx).Do();
        }

        public void OnPost(List<int> categoryId)
        {
            if (categoryId.Count != 0 && Name == null)
            {
                Products = new GetProducts(_ctx).Do(categoryId);
            }
            else if (Name != null)
            {
                Products = new GetProducts(_ctx).Do(Name);
            }

            Categories = new GetCategories(_ctx).Do();
            RedirectToPage("/Index");
        }
    }
}
