using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace Amido.Testing.WebApi.ValidationRules
{
    [DisplayName("Assert Status Code")]
    public class AssertStatusCodeValidationRule : ValidationRule
    {
        private readonly int expectedStatusCode;

        public AssertStatusCodeValidationRule(int expectedStatusCode)
        {
            this.expectedStatusCode = expectedStatusCode;
        }

        public override void Validate(object sender, ValidationEventArgs e)
        {
            if ((int)e.Response.StatusCode == expectedStatusCode)
            {
                e.Message = "The expected status code was returned.";
            }
            else
            {
                e.IsValid = false;
                e.Message = string.Format("The expected status code was not returned. Expected: {0} Actual: {1}.", expectedStatusCode, (int)e.Response.StatusCode);
            }
        }
    }
}
