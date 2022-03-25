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

            for (var i = 0; i < product.Images.Count(); i++)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", product.Images[i].Path);

                if (File.Exists(path))
                {
                    File.Delete(path);
                    _context.Images.Remove(product.Images[i]);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
