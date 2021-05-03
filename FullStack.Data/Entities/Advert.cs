using System;
using System.Collections.Generic;
using System.Text;

namespace FullStack.Data.Entities
{
    public class Advert
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public Province Province { get; set; }
        public int ProvinceId { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string State { get; set; }
        public bool Featured { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<FavouriteJoin> FavouriteJoins { get; set; }
            = new List<FavouriteJoin>();
    }
}
