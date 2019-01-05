using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core;
using Core.Model;

namespace InternalApi.DataAccess
{
    internal class InvoiceDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Invoice invoice)
        {
            db.Invoices.AddOrUpdate(invoice);
            db.SaveChanges();
        }

        internal Invoice GetInvoice(DatabaseContext db, int id)
        {
            return db.Invoices
                .Include(x => x.Receiver.Company.ActualAddress)
                .Include(x => x.Receiver.Company.LegalAddress)
                .Include(x => x.Sender.Company.ActualAddress)
                .Include(x => x.Sender.Company.LegalAddress)
                .SingleOrDefault(x => x.ID == id);
        }

        internal List<int> GetInvoices(DatabaseContext db, int numOfRecords, string regNumber, string docNumber, DateTime from, DateTime to, string company, decimal sum)
        {
            return db.Invoices
                    .Include(x => x.Receiver.Company)
                    .Include(x => x.Receiver.Company)
                    .Where(x => (!x.Incoming && from <= x.CheckoutDate && x.CheckoutDate <= to) ||
                                (x.Incoming && from <= x.ReceptionDate && x.ReceptionDate <= to))
                    .Where(x => x.DocNumber.Contains(docNumber))
                    .Where(x => x.RegNumber.Contains(regNumber))
                    .Where(x => x.Elements.Sum(y => y.Item.Price) >= sum)
                    .Where(x => x.Receiver.Company.Name.Contains(company) ||
                                x.Sender.Company.Name.Contains(company))
                    .OrderByDescending(x => x.ID)
                    .Take(numOfRecords)
                    .Select(x => x.ID)
                    .ToList();
        }

        internal bool Delete(DatabaseContext db, Invoice invoice)
        {
            db.Invoices.Remove(invoice);
            return db.SaveChanges() > 0;
        }
    }
}
