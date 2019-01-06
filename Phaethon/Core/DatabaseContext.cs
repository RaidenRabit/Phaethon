using System.Data.Entity;
using Core.Model;

namespace Core
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
        public DbSet<TaxGroup> TaxGroups { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<Logs> Logs { get; set; }
    }
}
