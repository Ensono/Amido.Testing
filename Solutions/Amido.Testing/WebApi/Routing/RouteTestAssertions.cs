using System.Web.Http.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Amido.Testing.WebApi.Routing
{
    public class RouteTestAssertions
    {
        private RouteTest routeTest;

        public RouteTestAssertions(RouteTest routeTest)
        {
            this.routeTest = routeTest;
        }

        public RouteTestAssertions AssertControllerType<TController>()
        {
            var descriptor = routeTest.ControllerSelector.SelectController(routeTest.HttpRequestMessage);
            routeTest.ControllerContext.ControllerDescriptor = descriptor;

            if(typeof(TController) != descriptor.ControllerType)
            {
                Assert.Fail("The controller type is incorrect. Expected: {0} Actual {1}", typeof(TController).Name, descriptor.ControllerName);
            }
            
            return this;
        }

        public RouteTestAssertions AssertActionName(string actionName)
        {
            var actionSelector = new ApiControllerActionSelector();
            var descriptor = actionSelector.SelectAction(routeTest.ControllerContext);

            if(actionName.ToLower() != descriptor.ActionName.ToLower())
            {
                Assert.Fail("The action name is incorrect. Expected: {0} Actual {1}", actionName, descriptor.ActionName);
            }

            return this;
        }

        public RouteTestAssertions AssertRouteData(string key, string value)
        {
            object actualValue;
            var hasRouteData = routeTest.HttpRouteData.Values.TryGetValue(key, out actualValue);

            if (!hasRouteData)
            {
                Assert.Fail("The route data key {0} is not present", key);
            }

            if (actualValue.ToString() != value)
            {
                Assert.Fail("The {0} value is incorrect. Expected: {1} Actual {2}", key,  value, actualValue);
            }
            return this;
        }
    }
}
