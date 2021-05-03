using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FullStack.API.Helpers;
using FullStack.Data.Repositories;
using FullStack.ViewModels.Users;

namespace FullStack.API.Services
{
    public interface IUserValidator
    {
        public IEnumerable<ValidationResult> Validate(string password);
        public IEnumerable<ValidationResult> Validate(UserAuthenticateRequestModel model);
        public IEnumerable<ValidationResult> Validate(UserCreateUpdateModel model);
    }

    public class UserValidator : IUserValidator
    {
        public IEnumerable<ValidationResult> Validate(string password)
        {
            var passwordResult = ValidatePassword(password);
            if (passwordResult != null) yield return passwordResult;
        }
        public IEnumerable<ValidationResult> Validate(UserAuthenticateRequestModel model)
        {
            var emailResult = ValidateEmail(model.Email);
            if (emailResult != null) yield return emailResult;

            var passwordResult = ValidatePassword(model.Password);
            if (passwordResult != null) yield return passwordResult;
        }

        public IEnumerable<ValidationResult> Validate(UserCreateUpdateModel model)
        {
            var firstNameResult = ValidateFirstName(model.FirstName);
            if (firstNameResult != null) yield return firstNameResult;

            var lastNameResult = ValidateLastName(model.LastName);
            if (lastNameResult != null) yield return lastNameResult;

            var emailResult = ValidateEmail(model.Email);
            if (emailResult != null) yield return emailResult;

            var phoneNumberResult = ValidatePhoneNumber(model.PhoneNumber);
            if (phoneNumberResult != null) yield return phoneNumberResult;

            var passwordResult = ValidatePassword(model.Password);
            if (passwordResult != null) yield return passwordResult;
        }

        private ValidationResult ValidateFirstName(string firstName)
        {
            if (firstName == null)
                return new ValidationResult(nameof(firstName), "First name is required");

            bool isValid = true;
            StringBuilder sb = new StringBuilder();
            var firstNameCleansed = Regex.Replace(firstName, @"\s+", "");
            if (firstNameCleansed.Length != firstName.Length)
            {
                sb.Append("First name cannot contain whitespace. ");
                isValid = false;
            }
            if (firstName.Length < 3)
            {
                sb.Append("First name must contain at least 1 character. ");
                isValid = false;
            }
            if (firstName.Length > 100)
            {
                sb.Append("First name must contain less than 100 characters. ");
                isValid = false;
            }
            if (!isValid)
                return new ValidationResult(nameof(firstName), sb.ToString());
            else
                return null;
        }

        private ValidationResult ValidateLastName(string lastName)
        {
            if (lastName == null)
                return new ValidationResult(nameof(lastName), "Last name is required");

            bool isValid = true;
            StringBuilder sb = new StringBuilder();
            var lastNameCleansed = Regex.Replace(lastName, @"\s+", "");
            if (lastNameCleansed.Length != lastName.Length)
            {
                sb.Append("Last name cannot contain whitespace. ");
                isValid = false;
            }
            if (lastName.Length < 3)
            {
                sb.Append("Last name must contain at least 3 characters. ");
                isValid = false;
            }
            if (lastName.Length > 100)
            {
                sb.Append("Last name must contain less than 100 characters. ");
                isValid = false;
            }
            if (!isValid)
                return new ValidationResult(nameof(lastName), sb.ToString());
            else
                return null;
        }

        private ValidationResult ValidateEmail(string email)
        {
            if (email == null)
                return new ValidationResult(nameof(email), "Email address is required");

            bool isValid = true;
            StringBuilder sb = new StringBuilder();
            if (email.IndexOf("@") == -1)
            {
                sb.Append("Invalid email address. ");
                isValid = false;
            }
            var emailCleansed = Regex.Replace(email, @"\s+", "");
            if (emailCleansed.Length != email.Length)
            {
                sb.Append("Email address cannot contain whitespace. ");
                isValid = false;
            }
            if (email.Length < 6)
            {
                sb.Append("Email address must contain at least 6 characters. ");
                isValid = false;
            }
            if (email.Length > 100)
            {
                sb.Append("Email address must contain less than 100 characters. ");
                isValid = false;
            }
            if (!isValid)
                return new ValidationResult(nameof(email), sb.ToString());
            else
                return null;
        }

        private ValidationResult ValidatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber != null && phoneNumber.Length < 6)
            {
                return new ValidationResult(nameof(phoneNumber),"Phone number must contain at least 6 characters. ");
            }
            if (phoneNumber != null && phoneNumber.Length > 30)
            {
                return new ValidationResult(nameof(phoneNumber), "Phone number must contain less than 30 characters. ");
            };

            return null;
        }

        private ValidationResult ValidatePassword(string password)
        {
            if (password == null)
                return new ValidationResult(nameof(password), "Password is required");

            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasNumber = new Regex(@"[0-9]+");
            var hasMiniMaxChars = new Regex(@".{8,100}");

            bool isValid = true;
            StringBuilder sb = new StringBuilder("Invalid Password. ");

            var passwordCleansed = Regex.Replace(password, @"\s+", "");
            if (passwordCleansed.Length != password.Length)
            {
                sb.Append("Password cannot contain whitespace. ");
                isValid = false;
            }
            if (!hasUpperChar.IsMatch(password))
            {
                sb.Append("Password should contain at least one upper case letter. ");
                isValid = false;
            }
            if (!hasLowerChar.IsMatch(password))
            {
                sb.Append("Password should contain at least one lower case letter. ");
                isValid = false;
            }
            if (!hasNumber.IsMatch(password))
            {
                sb.Append("Password should contain at least one numeric value. ");
                isValid = false;
            }
            if (!hasMiniMaxChars.IsMatch(password))
            {
                sb.Append("Password should not be lesser than 8 or greater than 100 characters. ");
                isValid = false;
            }
            if (!isValid)
                return new ValidationResult(nameof(password), sb.ToString());
            else
                return null;
        }
    }
}
