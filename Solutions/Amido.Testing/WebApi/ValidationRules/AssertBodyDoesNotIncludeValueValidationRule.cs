using System.ComponentModel;
using Amido.Testing.Dbc;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Amido.Testing.WebApi.ValidationRules
{
    [DisplayName("Assert Body Does Not Include Value")]
    public class AssertBodyDoesNotIncludeValueValidationRule : ValidationRule
    {
        private readonly string value;

        public AssertBodyDoesNotIncludeValueValidationRule(string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");

            this.value = value;
        }

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
