namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        City = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        Number = c.String(nullable: false),
                        Extra = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        RegNumber = c.String(nullable: false, maxLength: 100),
                        BankName = c.String(nullable: false, maxLength: 100),
                        BankNumber = c.String(nullable: false, maxLength: 100),
                        LegalAddress_ID = c.Int(),
                        ActualAddress_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Addresses", t => t.ActualAddress_ID)
                .ForeignKey("dbo.Addresses", t => t.LegalAddress_ID)
                .Index(t => t.Name, unique: true)
                .Index(t => t.RegNumber, unique: true)
                .Index(t => t.LegalAddress_ID)
                .Index(t => t.ActualAddress_ID);
            
            CreateTable(
                "dbo.Representatives",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Company_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.Company_ID)
                .Index(t => t.Company_ID);
            
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
                "dbo.Invoices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Incoming = c.Boolean(nullable: false),
                        Transport = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DocNumber = c.String(nullable: false),
                        RegNumber = c.String(nullable: false),
                        CheckoutDate = c.DateTime(nullable: false),
                        ReceptionDate = c.DateTime(nullable: false),
                        Sender_ID = c.Int(),
                        Receiver_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Representatives", t => t.Receiver_ID)
                .ForeignKey("dbo.Representatives", t => t.Sender_ID)
                .Index(t => t.Sender_ID)
                .Index(t => t.Receiver_ID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SerNumber = c.String(maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Product_ID = c.Int(),
                        IncomingTaxGroup_ID = c.Int(),
                        OutgoingTaxGroup_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TaxGroups", t => t.IncomingTaxGroup_ID)
                .ForeignKey("dbo.TaxGroups", t => t.OutgoingTaxGroup_ID)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .Index(t => t.Product_ID)
                .Index(t => t.IncomingTaxGroup_ID)
                .Index(t => t.OutgoingTaxGroup_ID);
            
            CreateTable(
                "dbo.TaxGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Tax = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Barcode = c.Int(nullable: false),
                        ProductGroup_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductGroups", t => t.ProductGroup_ID)
                .Index(t => t.Barcode, unique: true)
                .Index(t => t.ProductGroup_ID);
            
            CreateTable(
                "dbo.ProductGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Margin = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Customer_ID = c.Int(),
                        JobStatus = c.Int(nullable: false),
                        JobName = c.String(),
                        StartedTime = c.DateTime(nullable: false),
                        FinishedTime = c.DateTime(),
                        NotificationTime = c.DateTime(),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.Customer_ID)
                .Index(t => t.Customer_ID);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        Salt = c.Binary(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Username, unique: true);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RequestUrl = c.String(nullable: false),
                        RequestHeaders = c.String(nullable: false),
                        RequestBody = c.String(nullable: false),
                        ResponseBody = c.String(nullable: false),
                        UserToken = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Elements", "Item_ID", "dbo.Items");
            DropForeignKey("dbo.Products", "ProductGroup_ID", "dbo.ProductGroups");
            DropForeignKey("dbo.Items", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.Items", "OutgoingTaxGroup_ID", "dbo.TaxGroups");
            DropForeignKey("dbo.Items", "IncomingTaxGroup_ID", "dbo.TaxGroups");
            DropForeignKey("dbo.Elements", "Invoice_ID", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Sender_ID", "dbo.Representatives");
            DropForeignKey("dbo.Invoices", "Receiver_ID", "dbo.Representatives");
            DropForeignKey("dbo.Customers", "Address_ID", "dbo.Addresses");
            DropForeignKey("dbo.Representatives", "Company_ID", "dbo.Companies");
            DropForeignKey("dbo.Companies", "LegalAddress_ID", "dbo.Addresses");
            DropForeignKey("dbo.Companies", "ActualAddress_ID", "dbo.Addresses");
            DropIndex("dbo.Logins", new[] { "Username" });
            DropIndex("dbo.Jobs", new[] { "Customer_ID" });
            DropIndex("dbo.ProductGroups", new[] { "Name" });
            DropIndex("dbo.Products", new[] { "ProductGroup_ID" });
            DropIndex("dbo.Products", new[] { "Barcode" });
            DropIndex("dbo.TaxGroups", new[] { "Name" });
            DropIndex("dbo.Items", new[] { "OutgoingTaxGroup_ID" });
            DropIndex("dbo.Items", new[] { "IncomingTaxGroup_ID" });
            DropIndex("dbo.Items", new[] { "Product_ID" });
            DropIndex("dbo.Invoices", new[] { "Receiver_ID" });
            DropIndex("dbo.Invoices", new[] { "Sender_ID" });
            DropIndex("dbo.Elements", new[] { "Item_ID" });
            DropIndex("dbo.Elements", new[] { "Invoice_ID" });
            DropIndex("dbo.Customers", new[] { "Address_ID" });
            DropIndex("dbo.Representatives", new[] { "Company_ID" });
            DropIndex("dbo.Companies", new[] { "ActualAddress_ID" });
            DropIndex("dbo.Companies", new[] { "LegalAddress_ID" });
            DropIndex("dbo.Companies", new[] { "RegNumber" });
            DropIndex("dbo.Companies", new[] { "Name" });
            DropTable("dbo.Logs");
            DropTable("dbo.Logins");
            DropTable("dbo.Jobs");
            DropTable("dbo.ProductGroups");
            DropTable("dbo.Products");
            DropTable("dbo.TaxGroups");
            DropTable("dbo.Items");
            DropTable("dbo.Invoices");
            DropTable("dbo.Elements");
            DropTable("dbo.Customers");
            DropTable("dbo.Representatives");
            DropTable("dbo.Companies");
            DropTable("dbo.Addresses");
        }
    }
}
