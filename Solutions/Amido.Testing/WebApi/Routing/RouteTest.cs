using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Routing;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.Routing
{
    public class RouteTest
    {
        internal HttpRequestMessage HttpRequestMessage;
        internal IHttpRouteData HttpRouteData;
        internal IHttpControllerSelector ControllerSelector;
        internal HttpControllerContext ControllerContext;

        public static void Initialise(Action<RouteCollection> routeSetupAction)
        {
            routeSetupAction(RouteTable.Routes);
        }

        public static RouteTestAssertions Request(string url, HttpMethod httpMethod)
        {
            var routeTest =  new RouteTest("http://test.com/" + url, httpMethod);
            return new RouteTestAssertions(routeTest);
        }

        protected RouteTest(string url, HttpMethod httpMethod)
        {
            HttpRequestMessage = new HttpRequestMessage(httpMethod, url);
            HttpRequestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, GlobalConfiguration.Configuration);


           // if (HttpRequestMessage == null)

            foreach (IHttpRoute httpRoute in GlobalConfiguration.Configuration.Routes)

                //GlobalConfiguration.Configuration.Routes.VirtualPathRoot = "sdf";
            {

         
                IHttpRouteData routeData = httpRoute.GetRouteData("http://test.com/", HttpRequestMessage);
                if (routeData != null)
                {
                    var d = routeData;
                }
            }
            var r = (IHttpRouteData)null;


           
            var route = new HttpRouteData(new HttpRoute());
            HttpRouteData = GlobalConfiguration.Configuration.Routes.GetRouteData(HttpRequestMessage);
            HttpRequestMessage.Properties[HttpPropertyKeys.HttpRouteDataKey] = HttpRouteData;
            ControllerSelector = new DefaultHttpControllerSelector(GlobalConfiguration.Configuration);
            ControllerContext = new HttpControllerContext(GlobalConfiguration.Configuration, HttpRouteData, HttpRequestMessage);
        }


    }
}
