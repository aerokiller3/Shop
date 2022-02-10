using System.Linq;
using System.Threading.Tasks;
using Shop.Database;

namespace Shop.Application.Admin.CategoriesAdmin
{
    public class UpdateCategory
    {
        private readonly ApplicationDbContext _ctx;

        public UpdateCategory(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response> Do(Request request)
        {
            var category = _ctx.Categories.FirstOrDefault(x => x.Id == request.Id);

            category.Title = request.Title;
            category.ParentCategoryId = request.ParentCategoryId;

            await _ctx.SaveChangesAsync();
            return new Response
            {
                Id = category.Id,
                Title = category.Title,
                ParentCategoryId = category.ParentCategoryId
            };
        }

        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int? ParentCategoryId { get; set; }
        }

        public class Request
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int? ParentCategoryId { get; set; }
        }
    }
}