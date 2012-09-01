using System;
using System.Collections.Generic;
using Amido.Testing.WebApi.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.WebApi.ValidationRules;
using System.Threading;

namespace Amido.Testing.WebApi
{
    public abstract class WebApiTestBase : WebTest
    {
        private TestRequests testRequests;
        private WebTestRequest currentRequest;
        private TestRequest currentTestRequest;
        private Outcome currentWebTestOutcome;

        public TestTasks TestTasks
        {
            get
            {
                return new TestTasks();
            }
        }

        public TestRequests Requests
        {
            get
            { 
                testRequests =  new TestRequests();
                return testRequests;
            }
        }

        protected WebApiTestBase()
        {
            PreWebTest += OnPreWebTest;
            PostWebTest += OnPostWebTest;
        }

        private void OnPreWebTest(object sender, PreWebTestEventArgs preWebTestEventArgs)
        {
            try
            {
                StartUp();
            }
            catch (Exception)
            {
                preWebTestEventArgs.WebTest.Outcome = Outcome.Fail;
                preWebTestEventArgs.WebTest.AddCommentToResult("Startup failed.");
            }
            
        }

        private void OnPostWebTest(object sender, PostWebTestEventArgs postWebTestEventArgs)
        {
            try
            {
                CleanUp();
            }
            catch (Exception)
            {
                postWebTestEventArgs.WebTest.Outcome = Outcome.Fail;
                postWebTestEventArgs.WebTest.AddCommentToResult("Cleanup failed.");
            }
        }

        public abstract void WebTestRequests();

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            WebTestRequests();

            foreach (var testRequest in testRequests.Requests)
            {
                currentTestRequest = testRequest;
                currentRequest = testRequest.Request();
                currentWebTestOutcome = Outcome;
                currentRequest.ValidationRuleReferences.Clear();

                foreach (var assert in testRequest.Asserts)
                {
                    currentRequest.ValidateResponse += assert().Validate;
                }

                if (testRequest.MaxRetries == 0)
                {
                    if(testRequest.WaitPeriod > 0)
                    {
                        Thread.Sleep(testRequest.WaitPeriod);
                    }
                    yield return currentRequest;
                }
                else
                {
                    currentRequest.PostRequest += CurrentRequestOnPostRequest;

                    for(var i = 0; i <= testRequest.MaxRetries; i++)
                    {
                        //TestValidateRetry(testRequest, i);

                        currentRequest.ValidateResponse += new AssertRetryValidationRule(currentTestRequest.RetryTestType, currentTestRequest.RetryValue).Validate;

                        yield return currentRequest;


                        if (currentTestRequest.RetryTestType == RetryTestType.StatusCodeEquals && (int)LastResponse.StatusCode == int.Parse(testRequest.RetryValue))
                        {
                            break;
                        }

                        if (currentTestRequest.RetryTestType == RetryTestType.BodyEquals && LastResponse.BodyString == testRequest.RetryValue)
                        {
                            break;
                        }

                        if (currentTestRequest.RetryTestType == RetryTestType.BodyIncludes && LastResponse.BodyString.IndexOf(testRequest.RetryValue, StringComparison.Ordinal) > -1)
                        {
                            break;
                        }
                        
                        Thread.Sleep(testRequest.Interval);
                    }
                }
            }
        }

        private void CurrentRequestOnPostRequest(object sender, PostRequestEventArgs postRequestEventArgs)
        {
            if (currentWebTestOutcome == Outcome.Pass)
            {
                postRequestEventArgs.WebTest.Outcome = Outcome.Pass;
            }
        }

        public virtual void StartUp()
        {
        }

        public virtual void CleanUp()
        {
        }

        #region Test Helpers

        private void TestValidateRetry(TestRequest testRequest, int i)
        {
            if (i == 0)
            {
                currentRequest.Url = currentRequest.Url + "/askj";
            }

            if (i == testRequest.MaxRetries - 1)
            {
                currentRequest.Url = currentRequest.Url.Replace("/askj", "");
            }
        }

        #endregion
    }
}
