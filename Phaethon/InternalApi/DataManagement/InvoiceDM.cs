using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        addressDa.CreateOrUpdate(db, invoice.Receiver.Company.Address);
                        invoice.Receiver.Company.Address_ID = invoice.Receiver.Company.Address.ID;
                        invoice.Receiver.Company.Address = null;

                        addressDa.CreateOrUpdate(db, invoice.Receiver.Company.Location);
                        invoice.Receiver.Company.Location_ID = invoice.Receiver.Company.Location.ID;
                        invoice.Receiver.Company.Location = null;

                        companyDa.CreateOrUpdate(db, invoice.Receiver.Company);
                        invoice.Receiver.Company_ID = invoice.Receiver.Company.ID;
                        invoice.Receiver.Company = null;

                        representativeDa.CreateOrUpdate(db, invoice.Receiver);
                        invoice.Receiver_ID = invoice.Receiver.ID;
                        invoice.Receiver = null;
                        #endregion

                        #region Sender
                        addressDa.CreateOrUpdate(db, invoice.Sender.Company.Address);
                        invoice.Sender.Company.Address_ID = invoice.Sender.Company.Address.ID;
                        invoice.Sender.Company.Address = null;

                        addressDa.CreateOrUpdate(db, invoice.Sender.Company.Location);
                        invoice.Sender.Company.Location_ID = invoice.Sender.Company.Location.ID;
                        invoice.Sender.Company.Location = null;

                        companyDa.CreateOrUpdate(db, invoice.Sender.Company);
                        invoice.Sender.Company_ID = invoice.Sender.Company.ID;
                        invoice.Sender.Company = null;

                        representativeDa.CreateOrUpdate(db, invoice.Sender);
                        invoice.Sender_ID = invoice.Sender.ID;
                        invoice.Sender = null;
                        #endregion

                        List<Element> elements = invoice.Elements == null ? new List<Element>() : invoice.Elements.ToList();
                        invoice.Elements = null;

                        #region Transport
                        decimal total;
                        if (invoice.Incoming) //incoming
                        {
                            total = elements.Sum(item => item.Item.IncomingPrice);
                        }
                        else//outgoing
                        {
                            total = elements.Sum(item => item.Item.OutgoingPrice);
                        }
                        decimal transport = invoice.Transport;
                        Invoice dbInvoice = _invoiceDa.GetInvoice(db, invoice.ID);
                        if (dbInvoice != null)
                        {
                            transport = transport - dbInvoice.Transport;
                        }
                        if (total != 0)
                        {
                            transport = transport / total;
                        }
                        #endregion
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
                                    itemIds = elementDa.GetSameItemIdsInIncomingInvoice(db, item, invoice.ID);
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
                                            if (item.OutgoingPrice == 0 && item.OutgoingTaxGroup_ID == null)
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
                                        if (item != null && item.OutgoingPrice == 0 && item.OutgoingTaxGroup_ID == null)
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
                                        item.IncomingPrice = element.Item.IncomingPrice * transport + element.Item.IncomingPrice;
                                        item.IncomingTaxGroup_ID = element.Item.IncomingTaxGroup_ID;
                                        
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
                                List<int> itemIds = elementDa.GetSameItemIdsInOutgoingInvoice(db, item, invoice.ID);

                                productDa.CreateOrUpdate(db, element.Item.Product);

                                //removes from invoice if count was reduced
                                if (itemIds.Count > element.Item.Quantity)
                                {
                                    int removedCount = itemIds.Count - element.Item.Quantity;
                                    List<int> removedItemIds = new List<int>();
                                    foreach (int i in itemIds)
                                    {
                                        item = itemDa.GetItem(db, i);
                                        if (removedCount > 0)
                                        {
                                            //on delete of outgoing make item not sold and removes from invoice
                                            item.OutgoingPrice = 0;
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
                                    itemIds = itemIds.Except(removedItemIds).ToList();
                                }

                                for (int i = 0; i < element.Item.Quantity; i++)
                                {
                                    item = itemDa.GetItem(db, itemIds.ElementAtOrDefault(i));
                                    if (element.Item.Delete)//on delete of outgoing make item not sold and removes from invoice
                                    {
                                        item.OutgoingPrice = 0;
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
                                        if (item == null)
                                        {
                                            item = itemDa.GetItem(db, element.Item.ID);
                                            item = itemDa.GetItemNotSoldItem(db, item).ElementAtOrDefault(0);
                                        }

                                        if (item != null)
                                        {
                                            item.Product_ID = element.Item.Product.ID;
                                            item.OutgoingPrice =
                                                element.Item.OutgoingPrice * transport + element.Item.OutgoingPrice;
                                            item.OutgoingTaxGroup_ID = element.Item.OutgoingTaxGroup_ID;

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
                return _invoiceDa.GetInvoice(db, id);
            }
        }

        public List<Invoice> GetInvoices(int numOfRecords, int selectedCompany, string name, int selectedDate, DateTime from, DateTime to, string docNumber)
        {
            using (var db = new DatabaseContext())
            {
                return _invoiceDa.GetInvoices(db, numOfRecords, selectedCompany, name, selectedDate, from, to, docNumber);
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
    }
}