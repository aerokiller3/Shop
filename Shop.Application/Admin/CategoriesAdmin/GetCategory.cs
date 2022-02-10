using System.Linq;
using Shop.Database;

namespace Shop.Application.Admin.CategoriesAdmin
{
    public class GetCategory
    {
        private readonly ApplicationDbContext _ctx;

        public GetCategory(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public CategoryViewModel Do(int id) =>
            _ctx.Categories.Where(x => x.Id == id).Select(x => new CategoryViewModel
            {
                Id = x.Id,
                Title = x.Title,
                ParentCategoryId = x.ParentCategoryId
            }).FirstOrDefault();

        public class CategoryViewModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int? ParentCategoryId { get; set; }
        }
    }
}
