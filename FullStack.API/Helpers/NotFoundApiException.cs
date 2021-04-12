using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Helpers.Exceptions
{
    // Custom exception class for throwing not found exception that is caught and handled inside the api
    public class NotFoundApiException : ApiException
    {
        public NotFoundApiException() : base() { }
        public NotFoundApiException(string message) : base(message) { }
    }
}
