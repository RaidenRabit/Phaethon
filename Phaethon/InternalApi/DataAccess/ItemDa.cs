using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
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

        //for now remove later
        //internal List<int> GetSameItemIds(DatabaseContext db, int itemId)
        //{
        //    Item item = db.Items.SingleOrDefault(x => x.ID == itemId);

        //    return db.Items
        //        .Where(x => x.SerNumber.Equals(item.SerNumber) &&
        //                    x.IncomingPrice == item.IncomingPrice &&
        //                    x.IncomingTaxGroup_ID == item.IncomingTaxGroup_ID &&
        //                    x.Product_ID == item.Product_ID &&
        //                    (x.OutgoingPrice == 0 && x.IncomingTaxGroup_ID == null || x.ID == item.ID))
        //        .Select(x => x.ID)
        //        .ToList();
        //}

        internal void Delete(DatabaseContext db, Item item)
        {
            db.Items.Remove(item);
            db.SaveChanges();
        }
    }
}