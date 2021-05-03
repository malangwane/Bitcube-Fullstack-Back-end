using System;
using System.Collections.Generic;
using System.Text;

namespace FullStack.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool AdminRole { get; set; }
        public bool Locked { get; set; }
        public ICollection<FavouriteJoin> FavouriteJoins { get; set; }
            = new List<FavouriteJoin>();
        public ICollection<Advert> Adverts { get; set; }
            = new List<Advert>();
    }
}
