using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Helpers.Exceptions
{
    // Custom exception class for throwing unauthorized exception that is caught and handled inside the api
    public class UnauthorizedApiException: ApiException
    {
        public UnauthorizedApiException() : base() { }
        public UnauthorizedApiException(string message) : base(message) { }
    }
}
