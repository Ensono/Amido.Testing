using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;
namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Assert Body Is Empty.
    /// </summary>
    [DisplayName("Assert Body Is Empty")]
    public class AssertBodyIsEmptyValidationRule : ValidationRule
    {
        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
        public override void Validate(object sender, ValidationEventArgs e)
        {
            if (e.Response.IsBodyEmpty)
            {
                e.Message = "The response body is empty as expected.";
            }
            else
            {
                e.IsValid = false;
                e.Message = string.Format("The response body is not empty. Actual body: {0}.", e.Response.BodyString);
            }
        }
    }
}
