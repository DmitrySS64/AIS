using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class ParseProductsContext : DbContext
    {
        public DbSet<Product> Products{ get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public ParseProductsContext() : base("DefaultConnection")
        { }
    }

    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Url { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }

        public int? CategoryId { get; set; }
        public ProductCategory Category { get; set; }
    }

    public partial class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ICollection<Product> Products { get; set; }
        public ProductCategory()
        {
            Products = new List<Product>();
        }
    }
}
