using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class ProductGroupDa
    {
        internal void CreateOrUpdate(DatabaseContext db, ProductGroup productGroup)
        {
            db.ProductGroups.AddOrUpdate(productGroup);
            db.SaveChanges();
        }

        internal List<ProductGroup> GetProductGroups()
        {
            using (var db = new DatabaseContext())
            {
                return db.ProductGroups.ToList();
            }
        }
    }
}