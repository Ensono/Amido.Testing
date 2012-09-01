using System.ComponentModel;
using Amido.Testing.Dbc;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Assert Body Does Not Include Value.
    /// </summary>
    [DisplayName("Assert Body Does Not Include Value")]
    public class AssertBodyDoesNotIncludeValueValidationRule : ValidationRule
    {
        private readonly string value;

        /// <summary>
        /// Constructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="value">The value that should not be in the the response body.</param>
        public AssertBodyDoesNotIncludeValueValidationRule(string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");

            this.value = value;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
        public override void Validate(object sender, ValidationEventArgs e)
        {
            if (e.Response.BodyString.IndexOf(value, System.StringComparison.Ordinal) == -1)
            {
                e.Message = "The body correctly does not include the value.";
            }
            else
            {
                e.IsValid = false;
                e.Message = string.Format("The body includes the value: {0} in the body: {1}", value, e.Response.BodyString);
            }
        }
    }
}
