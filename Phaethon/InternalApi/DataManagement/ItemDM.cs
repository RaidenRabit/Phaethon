using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class ItemDM: IItemDM
    {
        private readonly ItemDa _itemDa;

        internal ItemDM()
        {
            _itemDa = new ItemDa();
        }

        public bool CreateOrUpdate(Item item)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    ProductDa productDa = new ProductDa();
                    productDa.CreateOrUpdate(db, item.Product);
                    item.Product_ID = item.Product.ID;
                    item.Product = null;
                    _itemDa.CreateOrUpdate(db, item);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public Item GetItem(int id)
        {
            using (var db = new DatabaseContext())
            {
                Item item = _itemDa.GetItem(db, id);
                if (item != null)
                {
                    item.Quantity = _itemDa.GetItemNotSoldItem(db, item).Count;
                }
                return item;
            }
        }

        public List<Item> GetItems(string serialNumber, string productName, int barcode, bool showAll)
        {
            using (var db = new DatabaseContext())
            {
                return _itemDa.GetItems(db, serialNumber, productName, barcode, showAll);
            }
        }

        public bool Delete(int id)
        {
            using (var db = new DatabaseContext())
            {
                Item item = _itemDa.GetItem(db, id);
                return _itemDa.Delete(db, item);
            }
        }
    }
}