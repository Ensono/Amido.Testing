using System;
using System.Collections.Generic;

namespace Amido.Testing.WebApi.Routing
{
    public class RouteData
    {
        public Type Controller { get; set; }
        public string ActionName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}