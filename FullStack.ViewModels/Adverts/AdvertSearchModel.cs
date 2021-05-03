using System;
using System.Collections.Generic;
using System.Text;

namespace FullStack.ViewModels.Adverts
{
    public class AdvertSearchModel
    {
        public string[] Keywords { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int OrderBy { get; set; }
    }
}
