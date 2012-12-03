using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.Request
{
    /// <summary>
    /// Class which stores a list of request actions and assertions.
    /// </summary>
    public class TestRequests
    {
        internal List<TestRequest> Requests;
 
        /// <summary>
        /// Constructs a new <see cref="TestRequests"/>.
        /// </summary>
        public TestRequests()
        {
            Requests = new List<TestRequest>();
        }

        /// <summary>
        /// Adds a new <see cref="WebTestRequest"/>.
        /// </summary>
        /// <param name="webApiRequest">The <see cref="WebTestRequest"/> action.</param>
        /// <param name="asserts">The <see cref="ValidationRule"/> actions.</param>
        /// <returns>Returns the current instance of <see cref="TestRequests"/>.</returns>
        public TestRequests Add(Func<WebTestRequest> webApiRequest, params Func<ValidationRule>[] asserts)
        {
            Contract.Requires(webApiRequest != null, "The web api request cannot be null.");

            return Retry(0, "0", 0, 0, webApiRequest, asserts);
        }

        /// <summary>
        /// Adds a retry policy to the <see cref="WebTestRequest"/>.
        /// </summary>
        /// <param name="retryTestType">The <see cref="RetryTestType"/>.</param>
        /// <param name="value">The value to be used in the retry test.</param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="interval">The interval in milliseconds between each retry.</param>
        /// <param name="webApiRequest">The <see cref="WebTestRequest"/> action.</param>
        /// <param name="asserts">The <see cref="ValidationRule"/> actions.</param>
        /// <returns>Returns the current instance of <see cref="TestRequests"/>.</returns>
        public TestRequests Retry(RetryTestType retryTestType, int value, int maxRetries, int interval, Func<WebTestRequest> webApiRequest, params Func<ValidationRule>[] asserts)
        {
            return Retry(retryTestType, value.ToString(CultureInfo.InvariantCulture), maxRetries, interval, webApiRequest, asserts);
        }

        /// <summary>
        /// Adds a retry policy to the <see cref="WebTestRequest"/>.
        /// </summary>
        /// <param name="retryTestType">The <see cref="RetryTestType"/>.</param>
        /// <param name="value">The value to be used in the retry test.</param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="interval">The interval in milliseconds between each retry.</param>
        /// <param name="webApiRequest">The <see cref="WebTestRequest"/> action.</param>
        /// <param name="asserts">The <see cref="ValidationRule"/> actions.</param>
        /// <returns>Returns the current instance of <see cref="TestRequests"/>.</returns>
        public TestRequests Retry(RetryTestType retryTestType, string value, int maxRetries, int interval, Func<WebTestRequest> webApiRequest, params Func<ValidationRule>[] asserts)
        {
            Contract.Requires(maxRetries >= 0, "The max retries cannot be less than 0.");
            Contract.Requires(interval >= 0, "The retry interval cannot be less than 0.");
            Contract.Requires(webApiRequest != null, "The web api request cannot be null.");

            var testRequest = new TestRequest { Request = webApiRequest, Asserts = asserts, RetryTestType = retryTestType, RetryValue = value, MaxRetries = maxRetries, Interval = interval };
            Requests.Add(testRequest);
            return this;
        }

        /// <summary>
        /// Adds a retry policy to the <see cref="WebTestRequest"/>.
        /// </summary>
        /// <param name="retryTestType">The <see cref="RetryTestType"/>.</param>
        /// <param name="value">The delegate value to be used in the retry test.</param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="interval">The interval in milliseconds between each retry.</param>
        /// <param name="webApiRequest">The <see cref="WebTestRequest"/> action.</param>
        /// <param name="asserts">The <see cref="ValidationRule"/> actions.</param>
        /// <returns>Returns the current instance of <see cref="TestRequests"/>.</returns>
        public TestRequests Retry(RetryTestType retryTestType, Func<string> value, int maxRetries, int interval, Func<WebTestRequest> webApiRequest, params Func<ValidationRule>[] asserts)
        {
            Contract.Requires(maxRetries >= 0, "The max retries cannot be less than 0.");
            Contract.Requires(interval >= 0, "The retry interval cannot be less than 0.");
            Contract.Requires(webApiRequest != null, "The web api request cannot be null.");

            var testRequest = new TestRequest { Request = webApiRequest, Asserts = asserts, RetryTestType = retryTestType, RetryValueDelegate = value, MaxRetries = maxRetries, Interval = interval };
            Requests.Add(testRequest);
            return this;
        }

        /// <summary>
        /// Forces the main thread to wait before running the <see cref="WebTestRequest"/>.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to wait before proceeding with the <see cref="WebTestRequest"/>.</param>
        /// <param name="webApiRequest">The <see cref="WebTestRequest"/> action.</param>
        /// <param name="asserts">The <see cref="ValidationRule"/> actions.</param>
        /// <returns>Returns the current instance of <see cref="TestRequests"/>.</returns>
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
