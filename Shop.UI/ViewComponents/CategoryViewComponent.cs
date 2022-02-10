using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Categories;

namespace Shop.UI.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly GetCategories _getCategories;

        public CategoryViewComponent(GetCategories getCategories)
        {
            _getCategories = getCategories;
        }

        public async Task<IViewComponentResult> InvokeAsync(string view)
        {
            var model = _getCategories.Do();
            if (view == "CategoryMobile")
            {
                return await Task.FromResult((IViewComponentResult) View(view, model));
            }

            return await Task.FromResult((IViewComponentResult)View(view, model));
        }
    }
}
