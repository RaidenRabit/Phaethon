using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class ElementDa
    {
        public void CreateOrUpdate(DatabaseContext db, Element element)
        {
            db.Elements.AddOrUpdate(element);
            db.SaveChanges();
        }

        public List<Element> GetInvoiceElements(DatabaseContext db, int id)
        {
            return db.Elements
                .Include(x => x.Item.Product.ProductGroup)
                .Include(x => x.Item.IncomingTaxGroup)
                .Where(x => x.Invoice_ID == id)
                .ToList();
        }
    }
}