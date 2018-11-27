using System;
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

        internal List<Element> GetInvoiceElements(DatabaseContext db, int id)
        {
            return db.Elements
                .Include(x => x.Item.Product.ProductGroup)
                .Include(x => x.Item.IncomingTaxGroup)
                .Where(x => x.Invoice_ID == id)
                .AsEnumerable()
                .GroupBy(x =>
                    new
                    {
                        x.Item.SerNumber,
                        x.Item.Product_ID
                    })
                .Select(g => new
                {
                    element = g.Select(c => c).FirstOrDefault(),
                    count = g.Count()
                })
                .Select(x => { x.element.Item.Quantity = x.count; return x.element; })
                .ToList();
        }
    }
}