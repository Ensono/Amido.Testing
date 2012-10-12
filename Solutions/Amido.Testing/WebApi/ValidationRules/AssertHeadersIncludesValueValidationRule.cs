using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Assert Headers Includes Value.
    /// </summary>
    [DisplayName("Assert Headers Includes Value")]
    public class AssertHeadersIncludesValueValidationRule : ValidationRule
    {
        private readonly string expectedIncludedValue;

        /// <summary>
        /// Constructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="expectedIncludedValue">The expected value to appear within the response header.</param>
        public AssertHeadersIncludesValueValidationRule(string expectedIncludedValue)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedIncludedValue), "The expected included value cannot be null or empty.");
            this.expectedIncludedValue = expectedIncludedValue;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
        public override void Validate(object sender, ValidationEventArgs e)
        {
            if (e.Response.Headers.ToString().IndexOf(expectedIncludedValue, System.StringComparison.Ordinal) > -1)
            {
                e.Message = "The headers included the expected value.";
            }
            else
            {
                e.IsValid = false;
                e.Message = string.Format("The headers did not include the expected value. Expected value: {0} Actual headers: {1}", expectedIncludedValue, e.Response.Headers);
            }
        }
    }
}
