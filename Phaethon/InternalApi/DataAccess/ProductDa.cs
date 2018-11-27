using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class ProductDa
    {
        public void CreateOrUpdate(DatabaseContext db, Product product)
        {
            db.Products.AddOrUpdate(product);
            db.SaveChanges();
        }

        public Product GetProduct(DatabaseContext db, int barcode)
        {
            return db.Products
                .Include(x => x.ProductGroup)
                .Include(x => x.Items)
                .SingleOrDefault(x => x.Barcode == barcode);
        }
    }
}