using System.Linq;
using System.Threading.Tasks;
using Shop.Database;

namespace Shop.Application.CategoriesAdmin
{
    public class DeleteCategory
    {
        private readonly ApplicationDbContext _ctx;

        public DeleteCategory(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Do(int id)
        {
            var category = _ctx.Categories.FirstOrDefault(x => x.Id == id);
            _ctx.Categories.Remove(category);
            await _ctx.SaveChangesAsync();

            return true;
        }
    }
}
