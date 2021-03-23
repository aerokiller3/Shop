using System.Collections.Generic;

namespace Shop.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public List<Category> Categories { get; set; }
        public List<CategoryProduct> Products { get; set; }
    }
}
