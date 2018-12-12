using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    public class ProductGroupDM: IProductGroupDM
    {
        private readonly ProductGroupDa _productGroupDa;

        internal ProductGroupDM()
        {
            _productGroupDa = new ProductGroupDa();
        }

        public bool Create(ProductGroup productGroup)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    _productGroupDa.Create(db, productGroup);
                    return true;
                }
                catch
                {
                    return false;
                }
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