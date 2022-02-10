using System.Threading.Tasks;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Admin.CategoriesAdmin
{
    public class CreateCategory
    {
        private readonly ApplicationDbContext _ctx;

        public CreateCategory(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response> Do(Request request)
        {
            var category = new Category
            {
                Title = request.Title,
                ParentCategoryId = request.ParentCategoryId
            };

            _ctx.Categories.Add(category);
            await _ctx.SaveChangesAsync();

            return new Response
            {
                Id = category.Id,
                Title = category.Title,
                ParentCategoryId = category.ParentCategoryId
            };
        }

        public class Request
        {
            public string Title { get; set; }
            public int? ParentCategoryId { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int? ParentCategoryId { get; set; }
        }
    }
}
