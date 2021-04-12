using CSharp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using static CSharp.Domain.InvoiceItem;

namespace CSharp.App
{
    class Program
    {

       /// <summary>
       /// Genarates random 12 invoices
       /// </summary>
       /// <returns></returns>
        static IEnumerable<Invoice> GenerateInvoice()
        {
            var startingDate = DateTime.Today;
            DateTime currentDate;
            Invoice Invoice;
             int invoiceReference = 1;
            // loop to generate 12 invoices
            for (int i = 0; i < 12; i++)
            {
                // get date creation of invoice
                currentDate = startingDate.AddMonths(i);
                // get invoice 
                Invoice = new Invoice(invoiceReference++, GetClient(), currentDate);

                var invoiceItems = InvoiceItemsGenerator(currentDate);
                foreach (var items in invoiceItems)
                {
                    Invoice.InvoiceItems = items;
                }
                yield return Invoice;
            }
        }

        
        static Client GetClient()
        {
            Random rnd = new Random();
            var clients = new List<Client>();

            clients.Add(new Client("Lungisa Capital", "102 Paul Kruger Ave, Bloemfontein, 1903", "0213365458"));
            clients.Add(new Client("Mark and Jay Attoney", "Nelson Mandela Drive, Nelpruit, 1425", "0125587458"));
            clients.Add(new Client("Pied Pieper LOC", "10 Lyle Street, Sandton, 1331", "0512512222"));

            return clients[rnd.Next(0, clients.Count())];
        }

       
        static IEnumerable<List<InvoiceItem>> InvoiceItemsGenerator(DateTime invoiceDate)
        {
            InvoiceItem tempItem;
            var listOfItems = new List<InvoiceItem>();
            Random rnd = new Random();

            var invoiceDateOfPreviousMonth = invoiceDate.AddMonths(-1);
            var daysInPreviousMonth = DateTime.DaysInMonth(invoiceDateOfPreviousMonth.Year, invoiceDateOfPreviousMonth.Month);
            invoiceDateOfPreviousMonth = new DateTime(invoiceDateOfPreviousMonth.Year, invoiceDateOfPreviousMonth.Month, daysInPreviousMonth - 5);
            TimeSpan timeSpanBetweenInvoiceDates = invoiceDate - invoiceDateOfPreviousMonth;

            // generate 12 lists of invoice items for each invoice
            for (int i = 0; i < 12; i++)
            {
                listOfItems.Clear();
                for (int j = 0; j < rnd.Next(1, 10); j++)
                {
                    // Get invoice randomly
                    switch (rnd.Next(1, 5))
                    {
                        case 1:
                            tempItem = new Maintenance( rnd.NextDouble() * (7- 0.3) + 0.3);
                            listOfItems.Add(tempItem);
                            break;
                        case 2:
                            tempItem = new Testing( rnd.NextDouble() * (7 - 0.3) + 0.3);
                            listOfItems.Add(tempItem);
                            break;
                        case 3:
                            tempItem = new Development( rnd.NextDouble() * (7 - 0.3) + 0.3);
                            listOfItems.Add(tempItem);
                            break;
                    }
                }
            }
            yield return listOfItems.OrderBy(item => item.ServiceName).ToList();
        }
        /// <summary>
        /// Main Method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var invoiceGenerator = GenerateInvoice();
            foreach (var invoice in invoiceGenerator)
            {
                invoice.DisplayInvoice();
            }
        }
    }
}
