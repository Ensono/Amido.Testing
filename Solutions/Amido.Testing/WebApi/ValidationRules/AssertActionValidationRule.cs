using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.ValidationRules
{
    [DisplayName("Assert Action")]
    public class AssertActionValidationRule : ValidationRule
    {
        private readonly Func<WebTestResponse, bool> action;
        private readonly string message;
        private readonly string errorMessage;

        public AssertActionValidationRule(Func<WebTestResponse, bool> action, string message, string errorMessage)
        {
            Contract.Requires(action != null, "Action cannot be null.");
            Contract.Requires(!string.IsNullOrWhiteSpace(message), "Message cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(errorMessage), "Error message cannot be null or empty.");

            this.action = action;
            this.message = message;
            this.errorMessage = errorMessage;
        }

        public override void Validate(object sender, ValidationEventArgs e)
        {
            if(action(e.Response))
            {
                e.Message = message;
            }
            else
            {
                e.IsValid = false;
                e.Message = errorMessage;
            }
        }
    }
}
