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

        public List<Item> GetItems(string serialNumber, string productName, int barcode)
        {
            using (var db = new DatabaseContext())
            {
                return _itemDa.GetItems(db, serialNumber, productName, barcode);
            }
        }
    }
}