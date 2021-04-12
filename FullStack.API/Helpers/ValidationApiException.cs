using FullStack.API.Helpers;
using FullStack.API.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Exceptions
{
    // Custom exception class for throwing validation exceptions that is caught and handled inside the api
    public class ValidationApiException: ApiException
    {
        public ValidationApiException(ValidationResult[] results): base(results[0].Message)
        {
            Errors = new ReadOnlyCollection<ValidationResult>(results);
        }

        public ReadOnlyCollection<ValidationResult> Errors { get; }
    }
}
