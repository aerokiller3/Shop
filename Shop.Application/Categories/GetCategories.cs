using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.Application.Categories
{
    public class GetCategories
    {
        private readonly ApplicationDbContext _ctx;

        public GetCategories(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<CategoryViewModel> Do() =>
            _ctx.Categories
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ParentCategoryId = x.ParentCategoryId,
                    Products = x.Products
                });

        public class CategoryViewModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int? ParentCategoryId { get; set; }
            public List<CategoryProduct> Products { get; set; }
        }
    }
}
