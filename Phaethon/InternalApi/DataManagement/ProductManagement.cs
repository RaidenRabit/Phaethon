using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    public class ProductManagement: IProductManagement
    {
        private readonly ProductDa _productDa;

        internal ProductManagement()
        {
            _productDa = new ProductDa();
        }

        public Product GetProduct(int barcode)
        {
            return _productDa.GetProduct(barcode);
        }
    }
}