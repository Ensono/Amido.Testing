using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Assert Content Type Equals Value.
    /// </summary>
    [DisplayName("Assert Content Type Equals Value")]
    public class AssertContentTypeEqualsValidationRule : ValidationRule
    {
        private readonly string expectedContentType;

        /// <summary>
        /// Constructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="expectedContentType">The expected content type.</param>
        public AssertContentTypeEqualsValidationRule(string expectedContentType)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(expectedContentType), "The expected content type value cannot be null or empty.");
            this.expectedContentType = expectedContentType;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
        public override void Validate(object sender, ValidationEventArgs e)
        {
            if (e.Response.ContentType == expectedContentType)
            {
                e.Message = "The content type was correct.";
            }
            else
            {
                e.IsValid = false;
                e.Message = string.Format("The content type was not correct. Expected: {0} Actual: {1}.", expectedContentType, e.Response.ContentType);
            }
        }
    }
}
