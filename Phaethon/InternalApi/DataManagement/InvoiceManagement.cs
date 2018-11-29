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

        public bool CreateOrUpdate(Invoice invoice)//made only for incoming
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
                        decimal total = elements.Sum(item => item.Item.IncomingPrice);
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

                        foreach (Element element in elements)
                        {
                            List<int> itemIds = elementDa.GetSameItemIds(db, element.Item.ID);
                            //don't remove if item was sold
                            #region Prepare item
                            Item item = new Item
                            {
                                Delete = element.Item.Delete,
                                Quantity = element.Item.Quantity
                            };

                            if (!item.Delete)
                            {
                                productDa.CreateOrUpdate(db, element.Item.Product);

                                item.SerNumber = element.Item.SerNumber;
                                item.Product_ID = element.Item.Product.ID;
                                if (invoice.Incoming)
                                {
                                    item.IncomingPrice = element.Item.IncomingPrice * transport + element.Item.IncomingPrice;
                                    item.IncomingTaxGroup_ID = element.Item.IncomingTaxGroup_ID;
                                }
                                else
                                {
                                    //doesn't save incoming price and tax group
                                    item.IncomingPrice = element.Item.IncomingPrice;
                                    item.IncomingTaxGroup_ID = element.Item.IncomingTaxGroup_ID;
                                    item.OutgoingPrice = element.Item.OutgoingPrice;
                                    item.OutgoingTaxGroup_ID = element.Item.OutgoingTaxGroup_ID;
                                }
                            }
                            #endregion

                            element.Item = null;
                            element.Invoice = null;
                            
                            for (int i = 0; i < item.Quantity; i++)
                            {
                                item.ID = itemIds.ElementAtOrDefault(i);
                                if (item.Delete)
                                {
                                    Item deleteItem = itemDa.GetItem(db, item.ID);
                                    itemDa.Delete(db, deleteItem);
                                }
                                else
                                {
                                    itemDa.CreateOrUpdate(db, item);
                                    element.Item_ID = item.ID;
                                    element.Invoice_ID = invoice.ID;
                                    elementDa.CreateOrUpdate(db, element);
                                }
                            }

                            //removes the removed ones
                            foreach (int i in itemIds.Skip(item.Quantity))
                            {
                                item.ID = i;
                                Item deleteItem = itemDa.GetItem(db, item.ID);
                                itemDa.Delete(db, deleteItem);
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