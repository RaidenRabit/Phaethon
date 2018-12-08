using Core.Model;
using System.Data.Entity;

namespace InternalApi.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Representative> Representatives { get; set; } = null;
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Login> Login { get; set; }
    }
}
