using System.Collections.Generic;
using System.Linq;
using Shop.Database;

namespace Shop.Application.Admin.CategoriesAdmin
{
    public class GetCategories
    {
        private readonly ApplicationDbContext _ctx;

        public GetCategories(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<CategoryViewModel> Do() =>
            _ctx.Categories.ToList().Select(x => new CategoryViewModel
            {
                Id = x.Id,
                Title = x.Title,
                ParentCategoryId = x.ParentCategoryId
            });

        public class CategoryViewModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int? ParentCategoryId { get; set; }
        }
    }
}
