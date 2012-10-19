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
        private readonly Func<WebTestResponse, AssertActionResult> action;

        /// <summary>
        /// Constructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="action">The validation action to run.</param>
        /// <param name="message">The successful message.</param>
        /// <param name="errorMessage">The error message.</param>
        public AssertActionValidationRule(Func<WebTestResponse, AssertActionResult> action)
        {
            Contract.Requires(action != null, "Action cannot be null.");

            this.action = action;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
        public override void Validate(object sender, ValidationEventArgs e)
        {
            var assertActionResult = action(e.Response);

            if(assertActionResult.Success)
            {
                e.Message = assertActionResult.Message;
            }
            else
            {
                e.IsValid = false;
                e.Message = assertActionResult.ErrorMessage;
            }
        }
    }
}
