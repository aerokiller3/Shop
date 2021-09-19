using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Categories;
using Shop.Application.Products;
using Shop.Database;
using SmartBreadcrumbs.Attributes;

namespace Shop.UI.Pages
{
    [DefaultBreadcrumb("Главная")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IEnumerable<GetProducts.ProductViewModel> _products;
        public IndexModel(ApplicationDbContext ctx, IEnumerable<GetProducts.ProductViewModel> products)
        {
            _ctx = ctx;
            _products = products;
        }
        public IEnumerable<GetProducts.ProductViewModel> Products { get; set; }

        [BindNever]
        public IEnumerable<GetProducts.ProductViewModel> ProductsFromCategory { get; set; }

        public IEnumerable<GetCategories.CategoryViewModel> Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        public void OnGet(int categoryId)
        {
            var url1 = Request.Headers["Referer"].ToString();

            Categories = new GetCategories(_ctx).Do();
            Products = new GetProducts(_ctx).Do();
            ProductsFromCategory = new GetProducts(_ctx).ProductsFromCategory(categoryId);

            if (ProductsFromCategory != null)
            {
                Products = ProductsFromCategory;
            }
            else
            {
                Products = Name == null ? new GetProducts(_ctx).Do() : new GetProducts(_ctx).Do(Name);
            }
        }

        public void OnPost()
        {
            if (_products.Count() != 0)
                Products = _products;
        }

        public void OnPostCategoriesProducts(List<int> categoryId)
        {
            Products = new GetProducts(_ctx).Do(categoryId);
            Categories = new GetCategories(_ctx).Do();
            RedirectToPage("/Index");
        }

        public void OnPostSearchProducts(string name)
        {
            Products = new GetProducts(_ctx).Do(name);
            Categories = new GetCategories(_ctx).Do();
            RedirectToPage("/Index");
        }
    }
}
