using System.ComponentModel;
using Amido.Testing.Dbc;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Assert Body Includes Value.
    /// </summary>
    [DisplayName("Assert Body Includes Value")]
    public class AssertBodyIncludesValueValidationRule : ValidationRule
    {
        private readonly string expectedIncludedValue;

        /// <summary>
        /// Constructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="expectedIncludedValue">The expected value to appear within the response body.</param>
        public AssertBodyIncludesValueValidationRule(string expectedIncludedValue)
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
            if (e.Response.BodyString.IndexOf(expectedIncludedValue, System.StringComparison.Ordinal) > -1)
            {
                e.Message = "The body included the expected value.";
            }
            else
            {
                e.IsValid = false;
                e.Message = string.Format("The body did not include the expected value. Expected value: {0} Actual body: {1}", expectedIncludedValue, e.Response.BodyString);
            }
        }
    }
}
