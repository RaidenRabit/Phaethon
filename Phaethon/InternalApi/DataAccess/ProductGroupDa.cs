using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class ProductGroupDa
    {
        internal void Create(DatabaseContext db, ProductGroup productGroup)
        {
            db.ProductGroups.Add(productGroup);
            db.SaveChanges();
        }

        internal List<ProductGroup> GetProductGroups(DatabaseContext db)
        {
            return db.ProductGroups.ToList();
        }
    }
}