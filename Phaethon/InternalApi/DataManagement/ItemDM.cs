using System;
using System.Collections.Generic;
using System.Linq;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    public class ItemDM: IItemDM
    {
        private readonly ItemDa _itemDa;

        public ItemDM()
        {
            _itemDa = new ItemDa();
        }

        public void CreateOrUpdate(Item item)
        {
            using (var db = new DatabaseContext())
            {
                ProductDa productDa = new ProductDa();
                productDa.CreateOrUpdate(db, item.Product);
                item.Product_ID = item.Product.ID;
                item.Product = null;
                _itemDa.CreateOrUpdate(db, item);
            }
        }
        
        public Item GetItem(int id)
        {
            using (var db = new DatabaseContext())
            {
                Item item = _itemDa.GetItem(db, id);
                if (item != null)
                {
                    Tuple<List<Item>, decimal> tuple = GetSameIncomingPriceItems(db, item);
                    item.Quantity = tuple.Item1.Count;
                    item.Price = tuple.Item2;
                }
                return item;
            }
        }

        public List<Item> GetItems(string serialNumber, string productName, int barcode, bool showAll)
        {
            using (var db = new DatabaseContext())
            {
                List<Item> items = _itemDa.GetItems(db, serialNumber, productName, barcode, showAll);
                foreach (Item item in items)
                {
                    item.Price = CalculateIncomingPrice(db, item);
                }
                items = items.GroupBy(x =>
                        new
                        {
                            x.SerNumber,
                            x.Price,
                            x.Product_ID
                        })
                    .Select(g => new
                    {
                        item = g.Select(c => c).FirstOrDefault(),
                        count = g.Count()
                    })
                    .Select(x => { x.item.Quantity = x.count; return x.item; })
                    .ToList();

                return items;
            }
        }

        public bool Delete(int id)
        {
            using (var db = new DatabaseContext())
            {
                Item item = _itemDa.GetItem(db, id);
                if (item == null)
                {
                    return false;
                }
                return _itemDa.Delete(db, item);
            }
        }
        
        public Tuple<List<Item>, decimal> GetSameIncomingPriceItems(DatabaseContext db, Item item)
        {
            Tuple<List<Item>, decimal> tuple;
            List<Item> items = new List<Item>();
            decimal price = CalculateIncomingPrice(db, item);
            foreach (Item tempItem in _itemDa.GetNotSoldItems(db, item))
            {
                decimal tempPrice = CalculateIncomingPrice(db, tempItem);

                if (price == tempPrice)
                {
                    items.Add(tempItem);
                }
            }
            tuple = new Tuple<List<Item>, decimal>(items, price);
            return tuple;
        }

        public decimal CalculateIncomingPrice(DatabaseContext db, Item item)
        {
            InvoiceDa invoiceDa = new InvoiceDa();
            ElementDa elementDa = new ElementDa();

            Element element = elementDa.GetItemElement(db, item.ID, true);
            if (element != null)//if item was added with invoice
            {
                List<Element> elements = elementDa.GetInvoiceElements(db, element.Invoice_ID);
                decimal transport = invoiceDa.GetInvoice(db, element.Invoice_ID).Transport;

                decimal sum = elements.Sum(x =>
                    x.Item.Price + x.Item.Price * ((decimal)x.Item.IncomingTaxGroup.Tax / 100));
                decimal procent = decimal.Round((sum + transport) / sum, 4);
                return decimal.Round((item.Price + item.Price * ((decimal)item.IncomingTaxGroup.Tax / 100)) * procent, 2);
            }
            return item.Price;
        }
        public decimal CalculateOutgoingPrice(DatabaseContext db, Item item)
        {
            decimal price = CalculateIncomingPrice(db, item);
            price = price + price * ((decimal)item.Product.ProductGroup.Margin / 100);
            price = price + price * ((decimal)item.IncomingTaxGroup.Tax / 100);
            return decimal.Round(price, 2);
        }
    }
}