using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class ElementDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Element element)
        {
            db.Elements.AddOrUpdate(element);
            db.SaveChanges();
        }

        internal bool Delete(DatabaseContext db, Element element)
        {
            db.Elements.Remove(element);
            return db.SaveChanges() > 0;
        }

        internal List<Element> GetInvoiceElements(DatabaseContext db, int id)
        {
            return db.Elements
                .Include(x => x.Item.Product.ProductGroup)
                .Include(x => x.Item.IncomingTaxGroup)
                .Include(x => x.Item.OutgoingTaxGroup)
                .Where(x => x.Invoice_ID == id)
                .ToList();
        }

        internal Element GetItemElement(DatabaseContext db, int id, bool incoming)
        {
            return db.Elements
                .Where(x => (incoming && x.Invoice.Incoming) || (!incoming && !x.Invoice.Incoming))
                .SingleOrDefault(x => x.Item_ID == id);
        }

        //gets all similar item, ids in invoice
        internal List<int> GetSameItemIdsFromInvoice(DatabaseContext db, Item item, int invoiceId, bool incoming)
        {
            var b = db.Elements
                .Where(x => x.Invoice_ID == invoiceId &&
                            x.Item.SerNumber.Equals(item.SerNumber) &&
                            x.Item.Price == item.Price &&
                            x.Item.Product_ID == item.Product_ID &&
                            (incoming && x.Item.Product_ID == item.Product_ID || !incoming && x.Item.OutgoingTaxGroup_ID == item.OutgoingTaxGroup_ID))
                .Select(x => x.Item_ID)
                .ToList();
            return b;
        }
    }
}