using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class InvoiceManagement: IInvoiceManagement
    {
        private readonly InvoiceDa _invoiceDa;

        internal InvoiceManagement()
        {
            _invoiceDa = new InvoiceDa();
        }

        public bool CreateOrUpdate(Invoice invoice)
        {
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
                        companyDa.CreateOrUpdate(db, invoice.Receiver.Company);
                        invoice.Receiver.Company_ID = invoice.Receiver.Company.ID;
                        invoice.Receiver.Company = null;
                        companyDa.CreateOrUpdate(db, invoice.Sender.Company);
                        invoice.Sender.Company_ID = invoice.Sender.Company.ID;
                        invoice.Sender.Company = null;
                        representativeDa.CreateOrUpdate(db, invoice.Receiver);
                        invoice.Receiver_ID = invoice.Receiver.ID;
                        invoice.Receiver = null;
                        representativeDa.CreateOrUpdate(db, invoice.Sender);
                        invoice.Sender_ID = invoice.Sender.ID;
                        invoice.Sender = null;
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
                                List<int> itemIds = elementDa.GetSameItemIdsInInvoice(db, element.Item.ID);

                                productDa.CreateOrUpdate(db, element.Item.Product);

                                //removes from database if count was reduced and when item wasn't sold yet
                                if (itemIds.Count > element.Item.Quantity)
                                {
                                    int removedCount = itemIds.Count - element.Item.Quantity;
                                    foreach (int i in itemIds)
                                    {
                                        Item item = itemDa.GetItem(db, i);
                                        if (removedCount < 0 && item.OutgoingPrice == 0 && item.OutgoingTaxGroup_ID == null)
                                        {
                                            itemDa.Delete(db, item);
                                            removedCount = removedCount - 1;
                                        }
                                    }
                                }

                                for (int i = 0; i < element.Item.Quantity; i++)
                                {
                                    Item item = itemDa.GetItem(db, itemIds.ElementAtOrDefault(i));
                                    if (element.Item.Delete)
                                    {
                                        //remove if item hasn't been sold yet
                                        if (item.OutgoingPrice == 0 && item.OutgoingTaxGroup_ID == null)
                                        {
                                            itemDa.Delete(db, item);
                                        }
                                    }
                                    else
                                    {
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
                                productDa.CreateOrUpdate(db, element.Item.Product);

                                Item item = itemDa.GetItem(db, element.Item.ID);
                                if (element.Item.Delete)
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
                                }
                                else
                                {
                                    item.Product_ID = element.Item.Product.ID;
                                    item.OutgoingPrice = element.Item.OutgoingPrice * transport + element.Item.OutgoingPrice;
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