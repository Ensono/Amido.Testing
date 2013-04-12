using System;
using System.Collections;
using System.Net;
using Amido.Testing.WebApi;
using Amido.Testing.WebApi.Request;
using Amido.Testing.WebApi.ValidationRules;

namespace Amido.Testing.Tests
{
    public class TestHarness : WebApiTestBase
    {
        public override void StartUp()
        {
            TestTasks.Add(() => { })
                .Wait();
            //throw new Exception("oh dear, the start up failed");
        }

        public override void CleanUp()
        {
            TestTasks.Add(() => { })
               .Wait();
            //throw new Exception("oh dear, the cleanup up failed");
        }

        public override void WebTestRequests()
        {
            var comment = "not set";

            Requests
                .Wait(2000, () =>
                                {
                                    return WebApiRequest
                                        .Url("http://www.google.co.uk")
                                        .WithVerb(Verb.GET)
                                        .Create();
                                })
                .Add(() =>
                         {
                             return WebApiRequest
                                 .Url("http://www.google.co.uk")
                                 .WithVerb(Verb.GET)
                                 .Create();
                         },
                     () => { return new AssertStatusCodeValidationRule(200); },
                     () =>
                         {
                             return new AssertActionValidationRule(r =>
                                                                       {
                                                                           comment = string.Format("Demonstrating delegated response: {0}", r.ContentType);
                                                                           return new AssertActionResult(r.StatusCode == HttpStatusCode.OK,
                                                                               "The action was successful",
                                                                               "The action was not successful");
                                                                       }
                             );
                         }
                )
                .Retry(RetryTestType.StatusCodeEquals, 200, 2, 1000, () =>
                {
                    return WebApiRequest
                        .Url("http://www.google.co.uk")
                        .WithVerb(Verb.GET)
                        .Create();
                },
                     () => { return new AssertStatusCodeValidationRule(200); },
                     () => { return new AssertBodyIncludesValueValidationRule("google"); }
                );

            var unwantedText = Guid.NewGuid().ToString();
            Requests
                .Retry(RetryTestType.BodyDoesNotInclude, unwantedText, 2, 1000, () =>
                         {
                             return WebApiRequest
                                 .Url("http://www.google.co.uk")
                                 .WithVerb(Verb.GET)
                                 .Create();
                         },
                     () => { return new AssertBodyDoesNotIncludeValueValidationRule(unwantedText); }
                );

            FinalOutput(() => { return comment; });
        }
    }
}
