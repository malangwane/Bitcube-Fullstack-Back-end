using FullStack.API.Helpers;
using FullStack.ViewModels.Adverts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FullStack.API.Services
{
    public interface IAdvertValidator
    {
        public IEnumerable<ValidationResult> Validate(AdvertCreateUpdateModel model);
    }

    public class AdvertValidator : IAdvertValidator
    {
        public IEnumerable<ValidationResult> Validate(AdvertCreateUpdateModel model)
        {
            var headerResult = ValidateHeader(model.Header);
            if (headerResult != null) yield return headerResult;

            var descriptionResult = ValidateDescription(model.Description);
            if (descriptionResult != null) yield return descriptionResult;

            var priceResult = ValidatePrice(model.Price);
            if (priceResult != null) yield return priceResult;
        }

        private ValidationResult ValidateHeader(string header)
        {
            if (header == null)
                return new ValidationResult(nameof(header), "Heading is required");

            var headerCleansed = Regex.Replace(header, @"\s+", "");
            if (headerCleansed.Length < 10)
                return new ValidationResult(nameof(header), "Heading must contain at least 10 non-whitespace characters");

            if (header.Length > 100)
                return new ValidationResult(nameof(header), "Heading must contain less than 100 characters");

            return null;
        }

        private ValidationResult ValidateDescription(string description)
        {
            if (description == null)
                return new ValidationResult(nameof(description), "Description is required");

            var descriptionCleansed = Regex.Replace(description, @"\s+", "");
            if (descriptionCleansed.Length < 10)
                return new ValidationResult(nameof(description), "Description must contain at least 10 non-whitespace characters");

            if (description.Length > 1000)
                return new ValidationResult(nameof(description), "Description must contain less than 1000 characters");

            return null;
        }

        private ValidationResult ValidatePrice(decimal price)
        {
            if (price < 10000)
            {
                return new ValidationResult(nameof(price), "Price must be at least R10000");
            }

            if (price > 100000000)
            {
                return new ValidationResult(nameof(price), "Price can't exceed R100000000");
            }

            return null;
        }
    }
}
