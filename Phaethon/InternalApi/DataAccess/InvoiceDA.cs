using Core.Model;
using InternalApi.DataAccess;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace InternalApi.DataManagement
{
    public class InvoiceDA
    {
        public void Create(Invoice invoice)
        {
            using (var db = new DatabaseContext())
            {
                db.Invoices.Attach(invoice);
                db.SaveChanges();
            }
        }

        public Invoice Read(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.Invoices.Find(id);
            }
        }

        public List<Invoice> GetInvoices()
        {
            using (var db = new DatabaseContext())
            {
                return db.Invoices.ToList();
            }
        }

        public void Update(Invoice invoice)
        {
            using (var db = new DatabaseContext())
            {
                db.Invoices.AddOrUpdate(invoice);
                db.SaveChanges();
            }
        }

        public void Delete(Invoice invoice)
        {
            using (var db = new DatabaseContext())
            {
                db.Invoices.Attach(invoice);
                db.Invoices.Remove(invoice);
                db.SaveChanges();
            }
        }
    }
}
