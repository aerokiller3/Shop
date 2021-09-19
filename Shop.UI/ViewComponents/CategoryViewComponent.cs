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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _getCategories.DoWithoutParent();
            return await Task.FromResult((IViewComponentResult) View("Category", model));
        }
    }
}
