using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    public class ProductGroupManagement: IProductGroupManagement
    {
        private readonly ProductGroupDa _productGroupDa;

        internal ProductGroupManagement()
        {
            _productGroupDa = new ProductGroupDa();
        }

        public bool Create(ProductGroup productGroup)
        {
            using (var db = new DatabaseContext())
            {
                if (productGroup == null)
                {
                    return false;
                }
                return _productGroupDa.Create(db, productGroup);
            }
        }

        public List<ProductGroup> GetProductGroups()
        {
            using (var db = new DatabaseContext())
            {
                return _productGroupDa.GetProductGroups(db);
            }
        }
    }
}