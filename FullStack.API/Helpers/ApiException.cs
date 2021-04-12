using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Helpers.Exceptions
{
    // Custom exception parent class to catch api exceptions
    public class ApiException: Exception
    {
        public ApiException() : base() { }
        public ApiException(string message) : base(message) { }
    }
}
