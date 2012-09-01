using System.ComponentModel;
using Amido.Testing.Dbc;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Assert Body Equals Value.
    /// </summary>
    [DisplayName("Assert Body Equals Value")]
    public class AssertBodyEqualsValidationRule : ValidationRule
    {
        private readonly string expectedBody;

        /// <summary>
        /// Constructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="expectedBody">The expected response body.</param>
        public AssertBodyEqualsValidationRule(string expectedBody)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedBody), "The expected body cannot be null or empty.");

            this.expectedBody = expectedBody;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
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
