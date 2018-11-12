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

        List<Invoice> IInvoiceManagement.GetInvoices(int numOfRecords, int selectedCompany, string name, int selectedDate, DateTime from, DateTime to)
        {
            return _invoiceDa.GetInvoices(numOfRecords, selectedCompany, name, selectedDate, from, to);
        }

        bool IInvoiceManagement.Delete(int id)
        {
            return _invoiceDa.Delete(id);
        }
    }
}