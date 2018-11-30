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
                .Include(x => x.Item.Product)
                .Where(x => x.Invoice_ID == id)
                .AsEnumerable()
                .GroupBy(x =>
                    new
                    {
                        x.Item.SerNumber,
                        x.Item.IncomingPrice,
                        x.Item.Product_ID,
                        x.Item.IncomingTaxGroup_ID
                    })
                .Select(g => new
                {
                    element = g.Select(c => c).FirstOrDefault(),
                    count = g.Count()
                })
                .Select(x => { x.element.Item.Quantity = x.count; return x.element; })
                .ToList();
        }

        internal List<int> GetSameItemIdsInInvoice(DatabaseContext db, int itemId)
        {
            Element element = db.Elements
                .Include(x => x.Item)
                .SingleOrDefault(x => x.Item_ID == itemId);

            return db.Elements
                .Where(x => x.Invoice_ID == element.Invoice_ID &&
                            x.Item.SerNumber.Equals(element.Item.SerNumber) &&
                            x.Item.IncomingPrice == element.Item.IncomingPrice &&
                            x.Item.Product_ID == element.Item.Product_ID &&
                            x.Item.IncomingTaxGroup_ID == element.Item.IncomingTaxGroup_ID)
                .Select(x => x.Item_ID)
                .ToList();
        }
    }
}