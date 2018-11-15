using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class ProductDa
    {
        internal Product GetProduct(int barcode)
        {
            using (var db = new DatabaseContext())
            {
                return db.Products
                    .Include(x => x.ProductGroup)
                    .Include(x => x.Items)
                    .SingleOrDefault(x => x.Barcode == barcode);
            }
        }
    }
}