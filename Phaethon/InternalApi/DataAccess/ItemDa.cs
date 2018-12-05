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
            return db.Items
                .Include(x => x.Product)
                .SingleOrDefault(x => x.ID == id);
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
                .AsEnumerable()
                .GroupBy(x =>
                    new
                    {
                        x.SerNumber,
                        x.IncomingPrice,
                        x.Product_ID
                    })
                .Select(g => new
                {
                    item = g.Select(c => c).FirstOrDefault(),
                    count = g.Count()
                })
                .Select(x => { x.item.Quantity = x.count; return x.item; })
                .ToList();
        }

        internal void Delete(DatabaseContext db, Item item)
        {
            db.Items.Remove(item);
            db.SaveChanges();
        }

        internal List<Item> GetItemNotSoldItem(DatabaseContext db, Item item)
        {
            return db.Items
                .Where(x => x.SerNumber.Equals(item.SerNumber) &&
                        x.IncomingPrice == item.IncomingPrice &&
                        x.OutgoingPrice == 0 &&
                        x.Product_ID == item.Product_ID &&
                        x.IncomingTaxGroup_ID == item.IncomingTaxGroup_ID &&
                        x.OutgoingTaxGroup_ID == null)
                .ToList();
        }
    }
}