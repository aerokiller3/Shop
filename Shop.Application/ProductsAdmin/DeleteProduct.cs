﻿using System.Linq;
using System.Threading.Tasks;
using Shop.Database;

namespace Shop.Application.ProductsAdmin
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
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
