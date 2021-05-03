using System;
using System.Collections.Generic;
using System.Text;

namespace FullStack.ViewModels.Adverts
{
    public class AdvertViewModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public string Province { get; set; }
        public int ProvinceId { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public decimal Price { get; set; }
        public string Date { get; set; }
        public string State { get; set; }
        public bool Featured { get; set; }
        public int UserId { get; set; }
    }
}