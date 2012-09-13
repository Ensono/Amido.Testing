using System.Net.Http;
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
                .Assertions(x =>
                                {
                                    Assert.AreEqual(typeof(RouteTestController), x.Controller);
                                    Assert.AreEqual("Get", x.ActionName);
                                });

            RouteTest.Request("api/routeTest", HttpMethod.Put)
                .Assertions(x =>
                {
                    Assert.AreEqual(typeof(RouteTestController), x.Controller);
                    Assert.AreEqual("Put", x.ActionName);
                });

            RouteTest.Request("api/doapost/123", HttpMethod.Post)
                .Assertions(x =>
                                {
                                    Assert.AreEqual(typeof(RouteTestController), x.Controller);
                                    Assert.AreEqual("DoAPost", x.ActionName);
                                    Assert.AreEqual("123", x.Parameters["id"]);
                                });

            RouteTest.Request("api/doacomplexpost/mc", HttpMethod.Post)
                .Assertions(x =>
                                {
                                    Assert.AreEqual(typeof (RouteTestController), x.Controller);
                                    Assert.AreEqual("DoAComplexPost", x.ActionName);
                                    Assert.AreEqual("mc", x.Parameters["surname"]);
                                });
        }
    }
}
