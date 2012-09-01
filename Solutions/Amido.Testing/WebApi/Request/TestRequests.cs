using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.Dbc;
using System.Threading;

namespace Amido.Testing.WebApi.Request
{
    public class TestRequests
    {
        internal List<TestRequest> Requests;
 
        public TestRequests()
        {
            Requests = new List<TestRequest>();
        }

        public TestRequests Add(Func<WebTestRequest> webApiRequest, params Func<ValidationRule>[] asserts)
        {
            Contract.Requires(webApiRequest != null, "The web api request cannot be null.");

            return Retry(0, "0", 0, 0, webApiRequest, asserts);
        }

        public TestRequests Retry(RetryTestType retryTestType, int value, int maxRetries, int interval, Func<WebTestRequest> webApiRequest, params Func<ValidationRule>[] asserts)
        {
            return Retry(retryTestType, value.ToString(CultureInfo.InvariantCulture), maxRetries, interval, webApiRequest, asserts);
        }

        public TestRequests Retry(RetryTestType retryTestType, string value, int maxRetries, int interval, Func<WebTestRequest> webApiRequest, params Func<ValidationRule>[] asserts)
        {
            Contract.Requires(maxRetries >= 0, "The max retries cannot be less than 0.");
            Contract.Requires(interval >= 0, "The retry interval cannot be less than 0.");
            Contract.Requires(webApiRequest != null, "The web api request cannot be null.");

            var testRequest = new TestRequest { Request = webApiRequest, Asserts = asserts, RetryTestType = retryTestType, RetryValue = value, MaxRetries = maxRetries, Interval = interval };
            Requests.Add(testRequest);
            return this;
        }

        public TestRequests Wait(int milliseconds, Func<WebTestRequest> webApiRequest, params Func<ValidationRule>[] asserts)
        {
            Contract.Requires(milliseconds != 0);
            Contract.Requires(webApiRequest != null, "The web api request cannot be null.");

            var testRequest = new TestRequest {Request = webApiRequest, Asserts = asserts, WaitPeriod = milliseconds};
            Requests.Add(testRequest);
            return this;
        }
    }
}
