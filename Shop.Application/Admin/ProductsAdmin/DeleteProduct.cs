using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shop.Database;

namespace Shop.Application.Admin.ProductsAdmin
{
    public class DeleteProduct
    {
        private readonly ApplicationDbContext _context;


        public DeleteProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Do(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);

            var fileName = product.Name + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
