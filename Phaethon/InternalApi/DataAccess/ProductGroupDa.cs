using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class ProductGroupDa
    {
        internal List<ProductGroup> GetProductGroups()
        {
            using (var db = new DatabaseContext())
            {
                return db.ProductGroups.ToList();
            }
        }
    }
}