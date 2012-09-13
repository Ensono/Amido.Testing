using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.Routing
{
    public class RouteTest
    {
        internal static HttpConfiguration HttpConfiguration;
        internal HttpRequestMessage HttpRequestMessage;
        internal IHttpRouteData HttpRouteData;
        internal IHttpControllerSelector ControllerSelector;
        internal HttpControllerContext ControllerContext;

        public static void Initialise(Action<HttpRouteCollection> routeSetupAction)
        {
            HttpConfiguration = new HttpConfiguration {IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always};
            routeSetupAction(HttpConfiguration.Routes);
        }

        public static RouteTestAssertions Request(string url, HttpMethod httpMethod)
        {
            var routeTest =  new RouteTest("http://test.com/" + url, httpMethod);
            return new RouteTestAssertions(routeTest);
        }

        protected RouteTest(string url, HttpMethod httpMethod)
        {
            Contract.Requires(HttpConfiguration != null, "You must call Initialise to set up you mappings and http configuration");

            HttpRequestMessage = new HttpRequestMessage(httpMethod, url);
            HttpRequestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, HttpConfiguration);
            HttpRouteData = HttpConfiguration.Routes.GetRouteData(HttpRequestMessage);
            HttpRequestMessage.Properties[HttpPropertyKeys.HttpRouteDataKey] = HttpRouteData;
            ControllerSelector = new DefaultHttpControllerSelector(HttpConfiguration);
            ControllerContext = new HttpControllerContext(HttpConfiguration, HttpRouteData, HttpRequestMessage);
        }
    }
}
