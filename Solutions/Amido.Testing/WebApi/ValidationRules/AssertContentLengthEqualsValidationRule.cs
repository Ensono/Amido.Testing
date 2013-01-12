using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Assert Content Length Equals Value.
    /// </summary>
    [DisplayName("Assert Content Length Equals Value")]
    public class AssertContentLengthEqualsValidationRule : ValidationRule
    {
        private readonly long expectedContentLength;

        /// <summary>
        /// Constructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="expectedContentLength">The expected content length.</param>
        public AssertContentLengthEqualsValidationRule(long expectedContentLength)
        {
            Contract.Requires(expectedContentLength >= 0, "The expected content length must be a positive integer.");
            this.expectedContentLength = expectedContentLength;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
        public override void Validate(object sender, ValidationEventArgs e)
        {
            if (e.Response.ContentLength == expectedContentLength)
            {
                e.Message = "The content length was correct.";
            }
            else
            {
                e.IsValid = false;
                e.Message = string.Format("The content length was not correct. Expected: {0} Actual: {1}.", expectedContentLength, e.Response.ContentLength);
            }
        }
    }
}
