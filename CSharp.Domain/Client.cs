using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Domain
{
    /// <summary>
    /// /
    /// Manages Client INFORMATION
    /// </summary>
    public class Client
    {
        public Client() { }
        public Client(string name, string address, string contact) : this()
        {
            this.Name = name;
            this.Contact = contact;
            this.Address = address;
        }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address{  get; set; }
    }
}
