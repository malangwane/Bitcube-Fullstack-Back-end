using FullStack.API.Helpers;
using FullStack.API.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Exceptions
{
    // Custom exception class for throwing update password exception that is caught and handled inside the api
    public class CheckPasswordApiException : ApiException
    {
        public CheckPasswordApiException() : base() { }
        public CheckPasswordApiException(string message) : base(message) { }
    }
}
