using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Domain
{
    /// <summary>
    /// Manages invoice items
    /// </summary>
     public abstract class InvoiceItem
    {
        public string ServiceName { get; set; }
        public double hoursWorked;
        public double HoursWorked 
        {
            get { return hoursWorked; }
            set
            {
                if (value < Math.Floor(value) + 0.3)
                    hoursWorked = Math.Floor(value);
                else
                    hoursWorked = Math.Floor(value) + 0.3;
            }
        }
        public double Rate { get; set; }
        public decimal Amount =>(decimal) (Rate * HoursWorked);

        public override string ToString()
        {
            return  $"Service: {this.ServiceName}, " +
                    $"Hours: {this.HoursWorked}, " +
                    $"Rate: R{this.Rate:0.00} per hour, " +
                    $"Cost: R{this.Amount:0.00}";

        }

        /// <summary>
        /// Prints the item information
        /// </summary>
        public void PrintItem()
        {

            Console.WriteLine($"{this.ServiceName,-10}\t|\t" +
                                $"{this.HoursWorked}\t|\t" +
                                $"R{this.Rate:0.00}\t\t|\t" +
                                $"R{this.Amount:0.00}");
        }
        /// <summary>
        /// Testing item info
        /// </summary>
        public class Testing : InvoiceItem
        {
            private double TESTING_RATE = 123.45;
            public Testing() : base()
            {
                this.Rate = TESTING_RATE;
                this.ServiceName = "testing";
            }
            public Testing( double workedHours) : this()
            {
                HoursWorked = workedHours;
            }

        }

        /// <summary>
        /// Development item info
        /// </summary>
        public class Development : InvoiceItem
        {
            private const double rate = 320.00;
            public Development() : base()
            {
                Rate = rate;
                ServiceName = "development";
            }
            public Development(double serviceHours) : this()
            {
                this.HoursWorked = serviceHours;
            }
        }
        /// <summary>
        /// Maintenace item info
        /// </summary>
        public class Maintenance : InvoiceItem
        {
            private const double rate = 260.45;
            public Maintenance() : base()
            {
                this.Rate = rate;
                this.ServiceName = "maintenance";
            }
            public Maintenance( double serviceHours) : this()
            {
                HoursWorked = serviceHours;
            }
        }

    }
}
