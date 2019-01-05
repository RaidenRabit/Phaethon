using Core;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    public class ProductDM: IProductDM
    {
        private readonly ProductDa _productDa;

        internal ProductDM()
        {
            _productDa = new ProductDa();
        }

        public Product GetProduct(int barcode)
        {
            using (var db = new DatabaseContext())
            {
                return _productDa.GetProduct(db, barcode);
            }
        }
    }
}