using System.ComponentModel.DataAnnotations;

namespace FullStack.ViewModels.Users
{
    public class UserCreateUpdateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool AdminRole { get; set; }
        public bool Locked { get; set; }
    }
}