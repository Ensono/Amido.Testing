using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Allows a custom action to be used as a <see cref="ValidationRule"/> assertion.
    /// </summary>
    [DisplayName("Assert Action")]
    public class AssertActionValidationRule : ValidationRule
    {
        private readonly Func<WebTestResponse, bool> action;
        private readonly string message;
        private readonly string errorMessage;

        /// <summary>
        /// Constructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="action">The validation action to run.</param>
        /// <param name="message">The successful message.</param>
        /// <param name="errorMessage">The error message.</param>
        public AssertActionValidationRule(Func<WebTestResponse, bool> action, string message, string errorMessage)
        {
            Contract.Requires(action != null, "Action cannot be null.");
            Contract.Requires(!string.IsNullOrWhiteSpace(message), "Message cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(errorMessage), "Error message cannot be null or empty.");

            this.action = action;
            this.message = message;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
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
