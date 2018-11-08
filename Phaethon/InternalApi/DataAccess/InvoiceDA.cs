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
                if (db.Invoices.Any(i => i.ID == invoice.ID))
                    db.Entry(invoice).State = EntityState.Modified;
                if (db.Representatives.Any(i => i.ID == invoice.Receiver.ID))
                    db.Entry(invoice.Receiver).State = EntityState.Modified;
                if (db.Companies.Any(i => i.ID == invoice.Receiver.Company.ID))
                    db.Entry(invoice.Receiver.Company).State = EntityState.Modified;
                if (db.Representatives.Any(i => i.ID == invoice.Sender.ID))
                    db.Entry(invoice.Sender).State = EntityState.Modified;
                if (db.Companies.Any(i => i.ID == invoice.Sender.Company.ID))
                    db.Entry(invoice.Sender.Company).State = EntityState.Modified;
                db.Invoices.Add(invoice);

                return db.SaveChanges() > 0;
            }
        }

        internal Invoice Read(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Invoices
                    .Include(x => x.Sender)
                    .Include(x => x.Sender.Company)
                    .Include(x => x.Receiver)
                    .Include(x => x.Receiver.Company)
                    .SingleOrDefault(x => x.ID == id);
            }
        }

        internal List<Invoice> GetInvoices(int numOfRecords)
        {
            using (var db = new DatabaseContext())
            {
                return db.Invoices
                    .Include(x => x.Sender)
                    .Include(x => x.Sender.Company)
                    .Include(x => x.Receiver)
                    .Include(x => x.Receiver.Company)
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
