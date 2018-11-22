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
        internal bool Create(DatabaseContext db, ProductGroup productGroup)
        {
            db.ProductGroups.Add(productGroup);
            return db.SaveChanges() > 0;
        }

        internal List<ProductGroup> GetProductGroups(DatabaseContext db)
        {
            return db.ProductGroups.ToList();
        }
    }
}