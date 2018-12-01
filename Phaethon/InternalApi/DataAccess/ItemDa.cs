using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class ItemDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Item item)
        {
            db.Items.AddOrUpdate(item);
            db.SaveChanges();
        }

        internal Item GetItem(DatabaseContext db, int id)
        {
            Item item = db.Items
                .Include(x => x.Product)
                .SingleOrDefault(x => x.ID == id);

            item.Quantity = 1;

            return item;
        }

        internal List<Item> GetItems(DatabaseContext db, string serialNumber, string productName, int barcode)
        {
            return db.Items
                .Include(x => x.Product)
                .Where(x => x.SerNumber.Contains(serialNumber))
                .Where(x => x.Product.Name.Contains(productName))
                .Where(x => barcode == 0 || x.Product.Barcode == barcode)
                .Where(x => x.OutgoingPrice == 0)
                .Where(x => x.OutgoingTaxGroup_ID == null)
                .ToList();
        }

        internal void Delete(DatabaseContext db, Item item)
        {
            db.Items.Remove(item);
            db.SaveChanges();
        }
    }
}