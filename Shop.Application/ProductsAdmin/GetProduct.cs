﻿using System.Linq;
using Shop.Database;

namespace Shop.Application.ProductsAdmin
{
    public class GetProduct
    {
        private readonly ApplicationDbContext _ctx;

        public GetProduct(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public ProductViewModel Do(int id) =>
            _ctx.Products.Where(x => x.Id == id).Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value,
                    Image = x.Image
                })
                .FirstOrDefault();

        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public string Image { get; set; }
        }
    }
}
