using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;

namespace Amido.Testing.WebApi.Routing
{
    public class RouteTestAssertions
    {
        private readonly RouteTest routeTest;

        public RouteTestAssertions(RouteTest routeTest)
        {
            this.routeTest = routeTest;
        }

        public void Assertions(Action<RouteData> assertActions)
        {
            var routeData = new RouteData
                                {
                                    Controller = GetControllerType(),
                                    ActionName = GetActionName(),
                                    Parameters = GetParameters()
                                };
            assertActions(routeData);
        }

        private Dictionary<string, object> GetParameters()
        {
            var parameters = new Dictionary<string, object>();

            foreach (var value in routeTest.HttpRouteData.Values)
            {
                parameters.Add(value.Key, value.Value);
            }

            return parameters;
        }

        private string GetActionName()
        {
            var actionSelector = new ApiControllerActionSelector();
            var descriptor = actionSelector.SelectAction(routeTest.ControllerContext);
            return descriptor.ActionName;
        }

        private Type GetControllerType()
        {
            var descriptor = routeTest.ControllerSelector.SelectController(routeTest.HttpRequestMessage);
            routeTest.ControllerContext.ControllerDescriptor = descriptor;
            return descriptor.ControllerType;
        }
    }
}
