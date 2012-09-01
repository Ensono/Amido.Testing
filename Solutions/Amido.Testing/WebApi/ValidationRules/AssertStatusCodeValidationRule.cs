using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Assert Status Code.
    /// </summary>
    [DisplayName("Assert Status Code")]
    public class AssertStatusCodeValidationRule : ValidationRule
    {
        private readonly int expectedStatusCode;

        /// <summary>
        /// Contructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="expectedStatusCode">The expected status code.</param>
        public AssertStatusCodeValidationRule(int expectedStatusCode)
        {
            this.expectedStatusCode = expectedStatusCode;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
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
