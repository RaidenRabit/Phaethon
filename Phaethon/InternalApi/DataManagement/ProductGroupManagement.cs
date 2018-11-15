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

        public List<ProductGroup> GetProductGroups()
        {
            return _productGroupDa.GetProductGroups();
        }
    }
}