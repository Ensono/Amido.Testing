using System.ComponentModel;
using Amido.Testing.WebApi.Request;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Amido.Testing.WebApi.ValidationRules
{
    [DisplayName("Assert Retry")]
    internal class AssertRetryValidationRule : ValidationRule
    {
        private readonly RetryTestType retryTestType;
        private readonly string expectedValue;

        public AssertRetryValidationRule(RetryTestType retryTestType, string expectedValue)
        {
            this.retryTestType = retryTestType;
            this.expectedValue = expectedValue;
        }

        public override void Validate(object sender, ValidationEventArgs e)
        {
            switch (retryTestType)
            {
                    case RetryTestType.StatusCodeEquals:
                    if ((int)e.Response.StatusCode == int.Parse(expectedValue))
                    {
                        e.Message = "Expected status code is correct.";
                    }
                    else
                    {
                        e.IsValid = false;
                        e.Message = string.Format("Expected status code is not correct. Expected: {0} Actual: {1}",
                                                  expectedValue, (int)e.Response.StatusCode);
                    }
                    break;
                    case RetryTestType.BodyEquals:
                    if (e.Response.BodyString == expectedValue)
                    {
                        e.Message = "Expected body is correct.";
                    }
                    else
                    {
                        e.IsValid = false;
                        e.Message = string.Format("Expected body is not correct. Expected: {0} Actual: {1}",
                                                  expectedValue, e.Response.BodyString);
                    }
                    break;
                    case RetryTestType.BodyIncludes:
                    if (e.Response.BodyString.IndexOf(expectedValue, System.StringComparison.Ordinal) > -1)
                    {
                        e.Message = "Body includes expected value.";
                    }
                    else
                    {
                        e.IsValid = false;
                        e.Message = string.Format("Body does not include expected value. Expected: {0} Actual: {1}",
                                                  expectedValue, e.Response.BodyString);
                    }
                    break;
            }
           
        }
    }
}
