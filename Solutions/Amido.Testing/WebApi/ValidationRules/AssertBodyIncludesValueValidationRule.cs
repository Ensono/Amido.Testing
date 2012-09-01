using System.ComponentModel;
using Amido.Testing.Dbc;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Amido.Testing.WebApi.ValidationRules
{
    [DisplayName("Assert Body Includes Value")]
    public class AssertBodyIncludesValueValidationRule : ValidationRule
    {
        private readonly string expectedIncludedValue;

        public AssertBodyIncludesValueValidationRule(string expectedIncludedValue)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedIncludedValue), "The expected included value cannot be null or empty.");

            this.expectedIncludedValue = expectedIncludedValue;
        }

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
