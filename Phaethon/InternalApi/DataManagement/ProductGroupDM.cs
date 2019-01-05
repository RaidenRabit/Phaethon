using System.Collections.Generic;
using Core;
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

        public void Create(ProductGroup productGroup)
        {
            using (var db = new DatabaseContext())
            {
                _productGroupDa.Create(db, productGroup);
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