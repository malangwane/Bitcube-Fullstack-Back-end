using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Helpers
{
    public class ValidationResult
    {
        public ValidationResult(string key, string message)
        {
            this.Key = key;
            this.Message = message;
        }
        public string Key { get; }
        public string Message { get; }
    }
}
