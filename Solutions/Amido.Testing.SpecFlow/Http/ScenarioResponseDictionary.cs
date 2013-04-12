using System.Collections.Generic;
using System.Net.Http;

namespace Amido.Testing.SpecFlow.Http
{
    public class ScenarioResponseDictionary : Dictionary<string, HttpResponseMessage>
    {
        public HttpResponseMessage LastResponse { get; set; }
    }
}
