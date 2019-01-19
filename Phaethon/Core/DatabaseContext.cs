using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace Core
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("PhaethonDB")
        {
            //Before every run recreates database
            Database.SetInitializer(new PhaethonDBInitializer());
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

    public class PhaethonDBInitializer : DropCreateDatabaseAlways<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            #region Login
            // strip off "0x" prefix
            string hex = "0xD0E37B9A16E43A01006693B7710EE85F0908DC9F7CBEDBB7EFDA05C32B8F9D1F".Substring(2);

            // add leading 0 if length is odd
            if (hex.Length % 2 == 1) hex = "0" + hex;

            // convert hex string to bytes
            byte[] bytes = System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary.Parse(hex).Value;

            context.Login.AddOrUpdate(
                new Login()
                {
                    Username = "test",
                    Password = "we3unzpp73x19fEH177omGzM+4HbLc/cxzFtSHLfplU=",
                    Salt = bytes
                }
            );//password= 12345
            #endregion
            context.SaveChanges();
            #region Tax group
            context.TaxGroups.AddOrUpdate(
                new TaxGroup
                {
                    Name = "IT Germany",
                    Tax = 19
                }
            );
            #endregion
            context.SaveChanges();
            #region Product group
            context.ProductGroups.AddOrUpdate(
                new ProductGroup
                {
                    Margin = 10,
                    Name = "Phone"
                }
            );
            #endregion
            context.SaveChanges();
            #region Product
            context.Products.AddOrUpdate(
                new Product
                {
                    Barcode = 1,
                    Name = "IPhone X",
                    ProductGroup_ID = 1
                }
            );
            #endregion
            context.SaveChanges();
            #region Item
            context.Items.AddOrUpdate(
                new Item
                {
                    Price = 100,
                    SerNumber = "123",
                    IncomingTaxGroup_ID = 1,
                    OutgoingTaxGroup_ID = 1,
                    Product_ID = 1
                },
                new Item
                {
                    Price = 100,
                    SerNumber = "124",
                    IncomingTaxGroup_ID = 1,
                    Product_ID = 1
                }
            );
            #endregion
            context.SaveChanges();
            #region Address
            context.Addresses.AddOrUpdate(
                new Address
                {
                    City = "Washington",
                    Street = "Pennsylvania Ave NW",
                    Number = "1600",
                    Extra = "President's Bedroom"
                },
                new Address
                {
                    City = "Salisbury",
                    Street = "SP4",
                    Number = "7DE",
                    Extra = ""
                }
            );
            #endregion
            context.SaveChanges();
            #region Company
            context.Companies.AddOrUpdate(
                new Company
                {
                    ActualAddress_ID = 1,
                    LegalAddress_ID = 2,
                    Name = "SpanishHopa",
                    RegNumber = "HKR4323",
                    BankName = "SpaniGri",
                    BankNumber = "SP45997345238"
                },
                new Company
                {
                    ActualAddress_ID = 1,
                    LegalAddress_ID = 2,
                    Name = "Pedegri",
                    RegNumber = "GR482288",
                    BankName = "Dansk Bank",
                    BankNumber = "DK93457773733"
                }
            );
            #endregion
            context.SaveChanges();
            #region Representative
            context.Representatives.AddOrUpdate(
                new Representative
                {
                    Name = "Adam Guntar",
                    Company_ID = 1
                },
                new Representative
                {
                    Name = "Iwo Jima",
                    Company_ID = 1
                },
                new Representative
                {
                    Name = "Herlem The Spaniard",
                    Company_ID = 2
                }
            );
            #endregion
            context.SaveChanges();
            #region Invoice
            context.Invoices.AddOrUpdate(
                new Invoice
                {
                    Incoming = true,
                    DocNumber = "TR221",
                    RegNumber = "JHKK2111",
                    CheckoutDate = new DateTime(2019, 1, 8),
                    ReceptionDate = new DateTime(2019, 1, 9),
                    Transport = 10,
                    Sender_ID = 1,
                    Receiver_ID = 3
                },
                new Invoice
                {
                    Incoming = false,
                    DocNumber = "JH29910",
                    RegNumber = "PRD2422",
                    CheckoutDate = new DateTime(2019, 1, 4),
                    ReceptionDate = new DateTime(2019, 1, 7),
                    Transport = 0,
                    Sender_ID = 3,
                    Receiver_ID = 2
                }
            );
            #endregion
            context.SaveChanges();
            #region Element
            context.Elements.AddOrUpdate(
                new Element
                {
                    Item_ID = 1,
                    Invoice_ID = 1
                }, new Element
                {
                    Item_ID = 2,
                    Invoice_ID = 1
                },
                new Element
                {
                    Item_ID = 1,
                    Invoice_ID = 2
                }
            );
            #endregion
            context.SaveChanges();
            #region Customer
            context.Customers.AddOrUpdate(
                new Customer
                {
                    Address_ID = 1,
                    GivenName = "Brad",
                    FamilyName = "Pit",
                    Email = "Pit@hotmail.com",
                    Phone = "+45 20546643"
                },
                new Customer
                {
                    Address_ID = 2,
                    GivenName = "Angelina",
                    FamilyName = "Jolie",
                    Email = "TheAngelina@gmail.com",
                    Phone = "+1 456302086"
                }
            );
            #endregion
            context.SaveChanges();
            #region Job
            context.Jobs.AddOrUpdate(
                new Job
                {
                    Customer_ID = 1,
                    JobStatus = JobStatus_enum.Completed,
                    JobName = "Fix broken Samsung galaxy Note 7",
                    StartedTime = new DateTime(2019, 1, 5),
                    FinishedTime = new DateTime(2019, 1, 6),
                    NotificationTime = new DateTime(2019, 1, 6),
                    Cost = 21,
                    Description = "Phone just exploded!!"
                },
                new Job
                {
                    Customer_ID = 2,
                    JobStatus = JobStatus_enum.InProgress,
                    JobName = "Window installation",
                    StartedTime = new DateTime(2019, 1, 6),
                    FinishedTime = new DateTime(2019, 1, 6),
                    Cost = 10,
                    Description = "Windows 10 has to be installed, saving data"
                },
                new Job
                {
                    Customer_ID = 1,
                    JobStatus = JobStatus_enum.Unassigned,
                    JobName = "Set up router",
                    StartedTime = new DateTime(2019, 1, 7),
                    FinishedTime = new DateTime(2019, 1, 8),
                    Cost = 5,
                    Description = "New router has to be set up, last one left"
                }
            );
            #endregion
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
