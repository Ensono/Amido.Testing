using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amido.Testing.Http;
using System.Net.Http;

namespace Amido.Testing.Tests.Http
{
    [TestClass]
    public class RestClientTests
    {
        [TestMethod]
        public void Should_Call_Google_And_Return_Valid_Response()
        {
            // Act
            var result = RestClient.RequestUri("http://www.google.co.uk")
                .WithoutRetries()
                .WithVerb(HttpMethod.Get)
                .MakeRequest();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
