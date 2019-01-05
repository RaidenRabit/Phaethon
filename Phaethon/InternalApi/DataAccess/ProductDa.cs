using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class ProductDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Product product)
        {
            db.Products.AddOrUpdate(product);
            db.SaveChanges();
        }

        internal Product GetProduct(DatabaseContext db, int barcode)
        {
            return db.Products
                .Include(x => x.ProductGroup)
                .Include(x => x.Items)
                .SingleOrDefault(x => x.Barcode == barcode);
        }
    }
}