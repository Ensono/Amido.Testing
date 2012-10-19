using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.ValidationRules
{
    public class AssertActionResult
    {
        public bool Success { get; set; }
        public string Message {get; set;}
        public string ErrorMessage {get; set;}

        public AssertActionResult(bool success, string message, string errorMessage)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(message), "Message cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(errorMessage), "Error message cannot be null or empty.");

            Success = success;
            Message = message;
            ErrorMessage = errorMessage;
                
        }
    }
}
