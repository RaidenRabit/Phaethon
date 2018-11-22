namespace InternalApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        Street = c.String(),
                        Number = c.String(),
                        Extra = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        RegNumber = c.String(nullable: false),
                        Location = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        BankNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Representatives",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Company_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.Company_ID)
                .Index(t => t.Company_ID);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Transport = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DocNumber = c.String(nullable: false),
                        PrescriptionDate = c.DateTime(nullable: false),
                        ReceptionDate = c.DateTime(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        Item_ID = c.Int(),
                        Receiver_ID = c.Int(),
                        Sender_ID = c.Int(),
                        Representative_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Items", t => t.Item_ID)
                .ForeignKey("dbo.Representatives", t => t.Receiver_ID)
                .ForeignKey("dbo.Representatives", t => t.Sender_ID)
                .ForeignKey("dbo.Representatives", t => t.Representative_ID)
                .Index(t => t.Item_ID)
                .Index(t => t.Receiver_ID)
                .Index(t => t.Sender_ID)
                .Index(t => t.Representative_ID);
            
            CreateTable(
                "dbo.Elements",
                c => new
                    {
                        Invoice_ID = c.Int(nullable: false),
                        Item_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Invoice_ID, t.Item_ID })
                .ForeignKey("dbo.Invoices", t => t.Invoice_ID, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.Item_ID, cascadeDelete: true)
                .Index(t => t.Invoice_ID)
                .Index(t => t.Item_ID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SerNumber = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InStock = c.Boolean(nullable: false),
                        Product_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .Index(t => t.Product_ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Barcode = c.String(),
                        ProductGroup_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductGroups", t => t.ProductGroup_ID)
                .Index(t => t.ProductGroup_ID);
            
            CreateTable(
                "dbo.ProductGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Tax = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Address_ID = c.Int(),
                        GivenName = c.String(),
                        FamilyName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Addresses", t => t.Address_ID)
                .Index(t => t.Address_ID);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Customer_ID = c.Int(),
                        JobStatus = c.Int(nullable: false),
                        JobName = c.String(),
                        StartedTime = c.DateTime(nullable: false),
                        FinishedTime = c.DateTime(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.Customer_ID)
                .Index(t => t.Customer_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Customers", "Address_ID", "dbo.Addresses");
            DropForeignKey("dbo.Invoices", "Representative_ID", "dbo.Representatives");
            DropForeignKey("dbo.Invoices", "Sender_ID", "dbo.Representatives");
            DropForeignKey("dbo.Invoices", "Receiver_ID", "dbo.Representatives");
            DropForeignKey("dbo.Elements", "Item_ID", "dbo.Items");
            DropForeignKey("dbo.Products", "ProductGroup_ID", "dbo.ProductGroups");
            DropForeignKey("dbo.Items", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.Invoices", "Item_ID", "dbo.Items");
            DropForeignKey("dbo.Elements", "Invoice_ID", "dbo.Invoices");
            DropForeignKey("dbo.Representatives", "Company_ID", "dbo.Companies");
            DropIndex("dbo.Jobs", new[] { "Customer_ID" });
            DropIndex("dbo.Customers", new[] { "Address_ID" });
            DropIndex("dbo.Products", new[] { "ProductGroup_ID" });
            DropIndex("dbo.Items", new[] { "Product_ID" });
            DropIndex("dbo.Elements", new[] { "Item_ID" });
            DropIndex("dbo.Elements", new[] { "Invoice_ID" });
            DropIndex("dbo.Invoices", new[] { "Representative_ID" });
            DropIndex("dbo.Invoices", new[] { "Sender_ID" });
            DropIndex("dbo.Invoices", new[] { "Receiver_ID" });
            DropIndex("dbo.Invoices", new[] { "Item_ID" });
            DropIndex("dbo.Representatives", new[] { "Company_ID" });
            DropTable("dbo.Jobs");
            DropTable("dbo.Customers");
            DropTable("dbo.ProductGroups");
            DropTable("dbo.Products");
            DropTable("dbo.Items");
            DropTable("dbo.Elements");
            DropTable("dbo.Invoices");
            DropTable("dbo.Representatives");
            DropTable("dbo.Companies");
            DropTable("dbo.Addresses");
        }
    }
}
