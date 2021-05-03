using System;
using System.Collections.Generic;
using System.Text;

namespace FullStack.Data.Entities
{
    public class FavouriteJoin
    {
        public int UserId { get; set; }
        public int AdvertId { get; set; }
        public User User { get; set; }
        public Advert Advert { get; set; }
    }
}
