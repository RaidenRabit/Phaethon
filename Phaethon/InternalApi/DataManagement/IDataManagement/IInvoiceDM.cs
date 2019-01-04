using Core.Model;
using System;
using System.Collections.Generic;

namespace InternalApi.DataManagement.IDataManagement
{
    internal interface IInvoiceDM
    {
        bool CreateOrUpdate(Invoice invoice);

        Invoice GetInvoice(int id);

        List<Invoice> GetInvoices(int numOfRecords, string regNumber, string docNumber, DateTime from, DateTime to, string company, decimal sum);

        bool Delete(int id);
    }
}
