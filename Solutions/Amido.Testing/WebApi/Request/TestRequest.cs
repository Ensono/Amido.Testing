using System;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Amido.Testing.WebApi.Request
{
    public class TestRequest
    {
        public Func<WebTestRequest> Request { get; set; }
        public Func<ValidationRule>[] Asserts { get; set; }
        public RetryTestType RetryTestType { get; set; }
        public string RetryValue { get; set; }
        public int MaxRetries { get; set; }
        public int Interval { get; set; }
        public bool AssertHasFailed { get; set; }
        public int WaitPeriod { get; set; }
    }
}
