using Core.Model;
using InternalApi.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalApi.Controller
{
    public class InvoiceController
    {
        public Invoice CreateInvoice(Invoice invoice)
        {
            try
            {
                InvoiceDA invoiceDA = new InvoiceDA();
                invoiceDA.Create(invoice);
                return invoiceDA.Read(invoice.ID);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public Invoice ReadInvoice(int id)
        {
            try
            {
                InvoiceDA invoiceData = new InvoiceDA();
                return invoiceData.Read(id);
            }
            catch
            {
                return null;
            }
        }

        public List<Invoice> GetInvoices()
        {
            try
            {
                InvoiceDA invoiceData = new InvoiceDA();
                return invoiceData.GetInvoices();
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateInvoice(Invoice invoice)
        {
            try
            {
                InvoiceDA invoiceData = new InvoiceDA();
                invoiceData.Update(invoice);
                Invoice dbInvoice = invoiceData.Read(invoice.ID);
                if (dbInvoice.Equals(invoice))//if was updated right
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteInvoice(int id)
        {
            try
            {
                InvoiceDA invoiceData = new InvoiceDA();
                Invoice invoice = invoiceData.Read(id);
                invoiceData.Delete(invoice);
                if (invoiceData.Read(invoice.ID) == null)//if was deleted
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
