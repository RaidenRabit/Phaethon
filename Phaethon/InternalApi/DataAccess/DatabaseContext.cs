using Core.Model;
using System.Data.Entity;

namespace InternalApi.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Representative> Representatives { get; set; }
    }
}
