using Core.Model;
using System;
using System.Collections.Generic;

namespace InternalApi.DataManagement.IDataManagement
{
    internal interface IInvoiceManagement
    {
        bool CreateOrUpdate(Invoice invoice);

        Invoice GetInvoice(int id);

        List<Invoice> GetInvoices(int numOfRecords, int selectedCompany, string name, int selectedDate, DateTime from, DateTime to, string docNumber);

        bool Delete(int id);
    }
}
