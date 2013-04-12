using Amido.Testing.SpecFlow.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Amido.Testing.SpecFlow.Tests.Http
{
    [Binding]
    public class HttpAssertions
    {
        [Then("the response http status code should be (.*)")]
        public void ThenTheResultShouldBe(int expectedStatusCode)
        {
            var context = ScenarioContextService.GetContext<ScenarioResponseDictionary>();
            Assert.AreEqual(expectedStatusCode, (int)context.LastResponse.StatusCode);
        }
    }
}
