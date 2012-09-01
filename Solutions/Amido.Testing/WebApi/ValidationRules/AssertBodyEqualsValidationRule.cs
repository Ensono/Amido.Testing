using System.ComponentModel;
using Amido.Testing.Dbc;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Amido.Testing.WebApi.ValidationRules
{
    [DisplayName("Assert Body Equals Value")]
    public class AssertBodyEqualsValidationRule : ValidationRule
    {
        private readonly string expectedBody;

        public AssertBodyEqualsValidationRule(string expectedBody)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedBody), "The expected body cannot be null or empty.");

            this.expectedBody = expectedBody;
        }

        public override void Validate(object sender, ValidationEventArgs e)
        {
            if (expectedBody == e.Response.BodyString)
            {
                e.Message = "The expected value was correct.";
            }
            else
            {
                e.IsValid = false;
                e.Message = string.Format("The expected value was not correct. Expected: {0} Actual {1}.", expectedBody, e.Response.BodyString);
            }
        }
    }
}
