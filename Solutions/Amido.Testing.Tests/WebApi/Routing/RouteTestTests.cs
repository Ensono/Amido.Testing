using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using Amido.Testing.WebApi;
using Amido.Testing.WebApi.Controllers;
using Amido.Testing.WebApi.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Amido.Testing.Tests.WebApi.Routing
{
    [TestClass]
    public class RouteTestTests
    {
        [TestMethod]
        public void GetTest()
        {
            RouteTest.Initialise(RouteConfig.RegisterRoutes);

            RouteTest.Request("api/RouteTest", HttpMethod.Get)
                .AssertControllerType<RouteTestController>()
                .AssertActionName("Get");

            RouteTest.Request("api/routeTest", HttpMethod.Put)
                .AssertControllerType<RouteTestController>()
                .AssertActionName("Put");

            RouteTest.Request("api/doapost/123", HttpMethod.Post)
                .AssertControllerType<RouteTestController>()
                .AssertActionName("DoAPost")
                .AssertRouteData("id", "123");

            RouteTest.Request("api/doacomplexpost/mc", HttpMethod.Post)
                .AssertControllerType<RouteTestController>()
                .AssertActionName("doacomplexpost")
                .AssertRouteData("surname", "mc");
        }
    }
}
