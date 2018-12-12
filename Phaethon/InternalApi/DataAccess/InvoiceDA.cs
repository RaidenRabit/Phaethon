using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Model;

namespace InternalApi.DataAccess
{
    public class InvoiceDa
    {
        internal void CreateOrUpdate(DatabaseContext db, Invoice invoice)
        {
            db.Invoices.AddOrUpdate(invoice);
            db.SaveChanges();
        }

        internal Invoice GetInvoice(DatabaseContext db, int id)
        {
            return db.Invoices
                .Include(x => x.Receiver.Company.Address)
                .Include(x => x.Receiver.Company.Location)
                .Include(x => x.Sender.Company.Address)
                .Include(x => x.Sender.Company.Location)
                .SingleOrDefault(x => x.ID == id);
        }

        internal List<Invoice> GetInvoices(DatabaseContext db, int numOfRecords, string regNumber, string invoiceNumber, DateTime from, DateTime to, string company, decimal sum)
        {
            //not used regNumber
            return db.Invoices
                    .Include(x => x.Sender.Company)
                    .Include(x => x.Receiver.Company)
                    .Where(x => !x.Incoming && from <= x.PrescriptionDate && x.PrescriptionDate <= to ||
                                x.Incoming && from <= x.ReceptionDate && x.ReceptionDate <= to)
                    .Where(x => x.DocNumber.Contains(invoiceNumber))
                    .Where(x => x.Receiver.Company.Name.Contains(company) ||
                                x.Sender.Company.Name.Contains(company))
                    .OrderByDescending(x => x.ID)
                    .Take(numOfRecords)
                    .AsEnumerable()
                    .Select(invoice => new { invoice, Sum = db.Elements.Where(x => x.Invoice_ID == invoice.ID).Select(x => x.Item.IncomingPrice).DefaultIfEmpty(0).Sum(x => x)})
                    .Select(x => { x.invoice.Sum = x.Sum; return x.invoice; })
                    .Where(x => x.Sum == sum)
                    .ToList();
        }

        internal bool Delete(DatabaseContext db, Invoice invoice)
        {
            db.Invoices.Remove(invoice);
            return db.SaveChanges() > 0;
        }
    }
}
