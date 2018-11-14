using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class InvoiceDa
    {
        internal bool Create(Invoice invoice)
        {
            using (var db = new DatabaseContext())
            {
                db.Companies.AddOrUpdate(invoice.Receiver.Company);
                db.Representatives.AddOrUpdate(invoice.Receiver);
                db.Companies.AddOrUpdate(invoice.Sender.Company);
                db.Representatives.AddOrUpdate(invoice.Sender);
                invoice.Receiver_ID = invoice.Receiver.ID;
                invoice.Sender_ID = invoice.Sender.ID;
                db.Invoices.AddOrUpdate(invoice);

                foreach (Element element in invoice.Elements)
                {
                    db.ProductGroups.AddOrUpdate(element.Item.Product.ProductGroup);
                    element.Item.Product.ProductGroup_ID = element.Item.Product.ProductGroup.ID;
                    db.Products.AddOrUpdate(element.Item.Product);
                    element.Item.Product_ID = element.Item.Product.ID;
                    db.Items.AddOrUpdate(element.Item);
                    element.Item_ID = element.Item.ID;
                    db.Elements.AddOrUpdate(element);
                }

                return db.SaveChanges() > 0;
            }
        }

        internal Invoice Read(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Invoices
                    .Include(x => x.Receiver.Company)
                    .Include(x => x.Sender.Company)
                    .Include(x => x.Elements.Select(i => i.Item.Product.ProductGroup))
                    .SingleOrDefault(x => x.ID == id);
            }
        }

        internal List<Invoice> GetInvoices(int numOfRecords, int selectedCompany, string name, int selectedDate,  DateTime from, DateTime to, string docNumber)
        {
            using (var db = new DatabaseContext())
            {
                return db.Invoices
                    .Include(x => x.Sender.Company)
                    .Include(x => x.Receiver.Company)
                    .Where(x => (selectedCompany == 0 && x.Receiver.Company.Name.Contains(name) ||
                                selectedCompany == 1 && x.Sender.Company.Name.Contains(name)) &&
                                (selectedDate == 0 && from <= x.PrescriptionDate && x.PrescriptionDate <= to ||
                                selectedDate == 1 && from <= x.ReceptionDate && x.ReceptionDate <= to ||
                                selectedDate == 2 && from <= x.PaymentDate && x.PaymentDate <= to) &&
                                x.DocNumber.Contains(docNumber))
                    .Take(numOfRecords)
                    .ToList();
            }
        }

        internal bool Delete(int id)
        {
            using (var db = new DatabaseContext())
            {
                db.Invoices.Remove(db.Invoices.SingleOrDefault(x => x.ID == id));
                return db.SaveChanges() > 0;
            }
        }
    }
}
