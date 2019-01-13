using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Model;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class InvoiceDM: IInvoiceDM
    {
        private readonly InvoiceDa _invoiceDa;

        internal InvoiceDM()
        {
            _invoiceDa = new InvoiceDa();
        }

        public bool CreateOrUpdate(Invoice invoice)
        {
            AddressDa addressDa = new AddressDa();
            CompanyDa companyDa = new CompanyDa();
            RepresentativeDa representativeDa = new RepresentativeDa();
            ProductDa productDa = new ProductDa();
            ItemDa itemDa = new ItemDa();
            ElementDa elementDa = new ElementDa();

            using (var db = new DatabaseContext())
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region Invoice

                        #region Receiver
                        addressDa.CreateOrUpdate(db, invoice.Receiver.Company.ActualAddress);
                        invoice.Receiver.Company.ActualAddress_ID = invoice.Receiver.Company.ActualAddress.ID;
                        invoice.Receiver.Company.ActualAddress = null;

                        addressDa.CreateOrUpdate(db, invoice.Receiver.Company.LegalAddress);
                        invoice.Receiver.Company.LegalAddress_ID = invoice.Receiver.Company.LegalAddress.ID;
                        invoice.Receiver.Company.LegalAddress = null;

                        companyDa.CreateOrUpdate(db, invoice.Receiver.Company);
                        invoice.Receiver.Company_ID = invoice.Receiver.Company.ID;
                        invoice.Receiver.Company = null;

                        representativeDa.CreateOrUpdate(db, invoice.Receiver);
                        invoice.Receiver_ID = invoice.Receiver.ID;
                        invoice.Receiver = null;
                        #endregion

                        #region Sender
                        addressDa.CreateOrUpdate(db, invoice.Sender.Company.ActualAddress);
                        invoice.Sender.Company.ActualAddress_ID = invoice.Sender.Company.ActualAddress.ID;
                        invoice.Sender.Company.ActualAddress = null;

                        addressDa.CreateOrUpdate(db, invoice.Sender.Company.LegalAddress);
                        invoice.Sender.Company.LegalAddress_ID = invoice.Sender.Company.LegalAddress.ID;
                        invoice.Sender.Company.LegalAddress = null;

                        companyDa.CreateOrUpdate(db, invoice.Sender.Company);
                        invoice.Sender.Company_ID = invoice.Sender.Company.ID;
                        invoice.Sender.Company = null;

                        representativeDa.CreateOrUpdate(db, invoice.Sender);
                        invoice.Sender_ID = invoice.Sender.ID;
                        invoice.Sender = null;
                        #endregion

                        List<Element> elements = invoice.Elements == null ? new List<Element>() : invoice.Elements.ToList();
                        invoice.Elements = null;
                        //generate the regNumber (cant be null)
                        invoice.RegNumber = "r";
                        _invoiceDa.CreateOrUpdate(db, invoice);
                        #endregion

                        if (invoice.Incoming)//incoming
                        {
                            foreach (Element element in elements)
                            {
                                Item item = itemDa.GetItem(db, element.Item.ID);
                                List<int> itemIds = new List<int>();
                                if (item != null)
                                {
                                    itemIds = elementDa.GetSameItemIdsFromInvoice(db, item, invoice.ID, true);
                                }

                                productDa.CreateOrUpdate(db, element.Item.Product);

                                //removes from database if count was reduced and when item wasn't sold yet
                                if (itemIds.Count > element.Item.Quantity)
                                {
                                    int removedCount = itemIds.Count - element.Item.Quantity;
                                    List<int> removedItemIds = new List<int>();
                                    foreach (int i in itemIds)
                                    {
                                        item = itemDa.GetItem(db, i);
                                        if (removedCount > 0)
                                        {
                                            if (item.OutgoingTaxGroup_ID == null)
                                            {
                                                itemDa.Delete(db, item);
                                                removedItemIds.Add(i);
                                                removedCount = removedCount - 1;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    itemIds = itemIds.Except(removedItemIds).ToList();
                                }

                                for (int i = 0; i < element.Item.Quantity; i++)
                                {
                                    item = itemDa.GetItem(db, itemIds.ElementAtOrDefault(i));
                                    if (element.Item.Delete)
                                    {
                                        //remove if item hasn't been sold yet
                                        if (item.OutgoingTaxGroup_ID == null)
                                        {
                                            itemDa.Delete(db, item);
                                        }
                                    }
                                    else
                                    {
                                        if (item == null)
                                        {
                                            item = new Item();
                                        }
                                        item.Product_ID = element.Item.Product.ID;
                                        item.SerNumber = element.Item.SerNumber;
                                        item.Price = element.Item.Price;
                                        item.IncomingTaxGroup_ID = element.Item.IncomingTaxGroup.ID;
                                        
                                        itemDa.CreateOrUpdate(db, item);

                                        Element tempElement = new Element
                                        {
                                            Item_ID = item.ID,
                                            Invoice_ID = invoice.ID
                                        };
                                        elementDa.CreateOrUpdate(db, tempElement);
                                    }
                                }
                            }
                        }
                        else//outgoing
                        {
                            foreach (Element element in elements)
                            {
                                Item item = itemDa.GetItem(db, element.Item.ID);
                                List<int> sameItemIds = elementDa.GetSameItemIdsFromInvoice(db, item, invoice.ID, false);//gets similar items in invoice

                                productDa.CreateOrUpdate(db, element.Item.Product);

                                //removes from invoice if count was reduced
                                if (sameItemIds.Count > element.Item.Quantity)
                                {
                                    int removedCount = sameItemIds.Count - element.Item.Quantity;
                                    List<int> removedItemIds = new List<int>();
                                    foreach (int i in sameItemIds)
                                    {
                                        item = itemDa.GetItem(db, i);
                                        if (removedCount > 0)
                                        {
                                            //on delete of outgoing make item not sold and removes from invoice
                                            item.OutgoingTaxGroup_ID = null;
                                            itemDa.CreateOrUpdate(db, item);
                                            Element tempElement = new Element
                                            {
                                                Item_ID = item.ID,
                                                Invoice_ID = invoice.ID
                                            };
                                            db.Elements.Attach(tempElement);
                                            elementDa.Delete(db, tempElement);
                                            removedItemIds.Add(i);
                                            removedCount = removedCount - 1;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    sameItemIds = sameItemIds.Except(removedItemIds).ToList();
                                }

                                for (int i = 0; i < element.Item.Quantity; i++)
                                {
                                    //tries to get the item
                                    item = itemDa.GetItem(db, sameItemIds.ElementAtOrDefault(i));

                                    if (element.Item.Delete)//on delete of outgoing make item not sold and removes from invoice
                                    {
                                        item.OutgoingTaxGroup_ID = null;
                                        itemDa.CreateOrUpdate(db, item);
                                        Element tempElement = new Element
                                        {
                                            Item_ID = item.ID,
                                            Invoice_ID = invoice.ID
                                        };
                                        db.Elements.Attach(tempElement);
                                        elementDa.Delete(db, tempElement);
                                    }
                                    else
                                    {
                                        if (item == null)//if item not found take same, but nut sold
                                        {
                                            item = itemDa.GetItem(db, element.Item.ID);
                                            item = new ItemDM().GetSameIncomingPriceItems(db, item).Item1.FirstOrDefault();
                                        }

                                        if (item != null)
                                        {
                                            item.Product_ID = element.Item.Product.ID;
                                            item.OutgoingTaxGroup_ID = element.Item.OutgoingTaxGroup.ID;
                                            
                                            itemDa.CreateOrUpdate(db, item);

                                            Element tempElement = new Element
                                            {
                                                Item_ID = item.ID,
                                                Invoice_ID = invoice.ID
                                            };
                                            elementDa.CreateOrUpdate(db, tempElement);
                                        }
                                    }
                                }
                            }
                        }

                        dbTransaction.Commit();
                        return true;
                    }
                    catch
                    {
                        dbTransaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public Invoice GetInvoice(int id)
        {
            using (var db = new DatabaseContext())
            {
                return GetInvoice(db, id);
            }
        }

        public List<Invoice> GetInvoices(int numOfRecords, string regNumber, string docNumber, DateTime from, DateTime to, string company, decimal sum)
        {
            using (var db = new DatabaseContext())
            {
                List<Invoice> invoices = new List<Invoice>();
                foreach (int id in _invoiceDa.GetInvoices(db, numOfRecords, regNumber, docNumber, from, to, company, sum))
                {
                    invoices.Add(GetInvoice(db, id));
                }
                return invoices;
            }
        }

        public bool Delete(int id)
        {
            using (var db = new DatabaseContext())
            {
                Invoice invoice = _invoiceDa.GetInvoice(db, id);
                if (invoice == null)
                {
                    return false;
                }
                return _invoiceDa.Delete(db, invoice);
            }
        }

        private Invoice GetInvoice(DatabaseContext db, int id)
        {
            ElementDa elementDa = new ElementDa();
            Invoice invoice = _invoiceDa.GetInvoice(db, id);
            if (invoice != null)
            {
                invoice.Elements = elementDa.GetInvoiceElements(db, invoice.ID);
                //sets the price with and without taxes
                if (invoice.Incoming)
                {
                    invoice.SumNoTax = invoice.Elements.Sum(x => x.Item.Price);

                    invoice.Sum = invoice.Elements.Sum(x =>
                        x.Item.Price + x.Item.Price * ((decimal) x.Item.IncomingTaxGroup.Tax / 100));
                    invoice.Sum += invoice.Transport;
                }
                else
                {
                    //should be improved for performance
                    ItemDM itemDm = new ItemDM();
                    invoice.SumNoTax = invoice.Elements.Sum(x => itemDm.CalculateIncomingPrice(db, x.Item));
                    invoice.Sum = invoice.Elements.Sum(x => itemDm.CalculateOutgoingPrice(db, x.Item)) +
                                  invoice.Transport;
                }
            }

            return invoice;
        }
    }
}