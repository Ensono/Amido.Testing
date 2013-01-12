using Amido.Testing.Dbc;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Assert Status Description.
    /// </summary>
    [DisplayName("Assert Status Description")]
    public class AssertStatusDescriptionValidationRule : ValidationRule
    {
        private readonly string expectedStatusDescription;

        /// <summary>
        /// Contructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="expectedStatusDescription">The expected status description.</param>
        public AssertStatusDescriptionValidationRule(string expectedStatusDescription)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedStatusDescription), "The expected status description cannot be null or empty.");
            this.expectedStatusDescription = expectedStatusDescription;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
        public override void Validate(object sender, ValidationEventArgs e)
        {
            if (e.Response.StatusDescription == expectedStatusDescription)
            {
                e.Message = "The expected status description was returned.";
            }
            else
            {
                e.IsValid = false;
                e.Message = string.Format("The expected status description was not returned. Expected: {0} Actual: {1}.", expectedStatusDescription, e.Response.StatusDescription);
            }
        }
    }
}
