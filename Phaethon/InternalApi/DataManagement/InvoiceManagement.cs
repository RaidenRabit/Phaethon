using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        bool IInvoiceManagement.Create(Invoice invoice)
        {
            return _invoiceDa.Create(invoice);
        }

        Invoice IInvoiceManagement.Read(int id)
        {
            return _invoiceDa.Read(id);
        }

        List<Invoice> IInvoiceManagement.GetInvoices()
        {
            return _invoiceDa.GetInvoices();
        }

        bool IInvoiceManagement.Delete(int id)
        {
            return _invoiceDa.Delete(id);
        }
    }
}