using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using InternalApi.Controller;

namespace InternalApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Company receiverCompany = new Company
            {
                ID = 1,
                Location = "Nibevej 2344 location",
                Name = "shufle",
                RegNumber = "54555556",
                Address = "Nibevej 2344 Address",
                BankNumber = "DK34745624547"
            };

            Representative receiver = new Representative();
            receiver.ID = 1;
            receiver.Name = "Kabola";
            receiver.Company = receiverCompany;

            Company senderCompany = new Company();
            senderCompany.Location = "Wallstreet location";
            senderCompany.Name = "Pop-top-spain";
            senderCompany.RegNumber = "1233333";
            senderCompany.Address = "Wallstreet Address";
            senderCompany.BankNumber = "DK3477777777";

            Representative sender = new Representative();
            sender.Name = "Kasper";
            sender.Company = senderCompany;

            Invoice invoice = new Invoice();
            invoice.DocNumber = "136381022";
            invoice.PaymentDate = DateTime.Now;
            invoice.PrescriptionDate = DateTime.Now;
            invoice.ReceptionDate = DateTime.Now;
            invoice.Receiver = receiver;
            invoice.Sender = sender;
            invoice.Transport = 5;

            InvoiceController invoiceController = new InvoiceController();
            invoiceController.CreateInvoice(invoice);
            Console.WriteLine(invoiceController.GetInvoices().Count);
        }
    }
}
