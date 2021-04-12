using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp.Domain
{
    /// <summary>
    /// 
    /// Manages invoices
    /// </summary>
    public class Invoice
    {
        public Invoice(int refNo, Client client,DateTime dateCreated) : this()
        {
            this.InvoiceRef = refNo;
            this.Client = client;
            this.InvoiceDate = dateCreated;

        }

        public Invoice()
        {
            this.InvoiceItems = new List<InvoiceItem>();
            this.Client = new Client();
            this.InvoiceDate = DateTime.UtcNow;
        }
        public Client Client { get; set; }
        public string From => "SysBit Technologies";

        public DateTime invoiceDate;
        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set
            {
                var daysInMonth = DateTime.DaysInMonth(value.Year, value.Month);
                invoiceDate = new DateTime(value.Year, value.Month, daysInMonth - 5);

                DueDate = invoiceDate.AddMonths(1);
                var lastDayOftheMonth = DateTime.DaysInMonth(DueDate.Year, DueDate.Month);
                DueDate = new DateTime(DueDate.Year, DueDate.Month, lastDayOftheMonth);
            }
        }

        public int invoiceRef { get; set; }
        public int InvoiceRef { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount => (decimal)invoiceItems.Sum(item => item.Amount);
        public string Description { get; set; }
        public List<InvoiceItem> invoiceItems;
        public List<InvoiceItem> InvoiceItems
        {
            private get { return invoiceItems; }
            set 
            {
                invoiceItems = value; 
                Console.WriteLine(invoiceItems.ToString());
            }     
        }


        public override string ToString()
        {
            return "Invoice: " + InvoiceRef +", "+ "Client: "+ Client;

        }

        /// <summary>
        /// prints the invoice on console
        /// </summary>
        public void DisplayInvoice()
        {
            Console.WriteLine($"\nINVOICE NO: INV-{InvoiceRef}" + "\t\t\tDATE: " + InvoiceDate.DayOfWeek.ToString() + InvoiceDate.ToShortDateString());
            Console.WriteLine("FROM: " + From + "\t\tDUE: " + DueDate.DayOfWeek.ToString() +" "+ DueDate.Date.ToShortDateString());
            Console.WriteLine("CLIENT: " + Client.Name);
            Console.WriteLine("CONTACT: " + Client.Contact);
            Console.WriteLine("ADDRESS: " + Client.Address);
            Console.WriteLine("=================================================================================");
            Console.WriteLine( $"{"SERVICES",-10}\t|\t" + $"HOURS\t|\t" + $"RATE PER HOUR\t|\t" +   $"Amount");
            Console.WriteLine("=================================================================================");
            foreach (var item in invoiceItems)
            {
                item.PrintItem();
            }
            Console.WriteLine("---------------------------------------------------------------------------------");
            Console.WriteLine($"\t\t\t\t\t\t   TOTAL:       R{TotalAmount:0.00} \n" +
                $"---------------------------------------------------------------------------------");
        }

    }
}
