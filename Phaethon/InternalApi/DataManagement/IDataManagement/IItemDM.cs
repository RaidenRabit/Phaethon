using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace InternalApi.DataManagement.IDataManagement
{
    interface IItemDM
    {
        bool CreateOrUpdate(Item item);

        Item GetItem(int id);

        List<Item> GetItems(string serialNumber, string productName, int barcode, bool showAll);

        bool Delete(int id);
    }
}
