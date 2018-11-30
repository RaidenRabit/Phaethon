using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class ItemManagement: IItemManagement
    {
        private readonly ItemDa _itemDa;

        internal ItemManagement()
        {
            _itemDa = new ItemDa();
        }

        public Item GetItem(int id)
        {
            using (var db = new DatabaseContext())
            {
                return _itemDa.GetItem(db, id);
            }
        }
    }
}