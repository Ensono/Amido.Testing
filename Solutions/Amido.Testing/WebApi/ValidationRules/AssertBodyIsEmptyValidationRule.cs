using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.WebTesting;
namespace Amido.Testing.WebApi.ValidationRules
{
    [DisplayName("Assert Body Is Empty")]
    public class AssertBodyIsEmptyValidationRule : ValidationRule
    {
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
