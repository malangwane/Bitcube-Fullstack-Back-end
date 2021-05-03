using System;
using System.Collections.Generic;
using System.Text;

namespace FullStack.ViewModels.Users
{
    public class UserAuthenticateResponseModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool AdminRole { get; set; }
        public bool Locked { get; set; }
        public string Token { get; set; }
    }
}
