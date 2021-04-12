using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Helpers.Exceptions
{
    // Custom exception class for throwing duplicate user exception that is caught and handled inside the api
    public class DuplicateUserApiException: ApiException
    {
        public DuplicateUserApiException() : base() { }
        public DuplicateUserApiException(string message) : base(message) { }
    }
}