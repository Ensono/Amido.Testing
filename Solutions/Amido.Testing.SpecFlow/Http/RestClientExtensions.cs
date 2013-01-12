using System.Net.Http;

namespace Amido.Testing.SpecFlow.Http
{
    public static class RestClientExtensions
    {
        public static HttpResponseMessage StoreResponseOnScenarioContext(this HttpResponseMessage httpResponseMessage)
        {
            return StoreResponseOnScenarioContext(httpResponseMessage, null);
        }

        public static HttpResponseMessage StoreResponseOnScenarioContext(this HttpResponseMessage httpResponseMessage, string key)
        {
            var context = ScenarioContextService.GetContext<ScenarioResponseDictionary>();
            context.LastResponse = httpResponseMessage;

            if(!string.IsNullOrWhiteSpace(key))
            {
                if(!context.ContainsKey(key))
                {
                    context.Add(key, httpResponseMessage);   
                }
                else
                {
                    var newKeySuffix = 0;
                    string newKey;
                    do
                    {
                        newKey = string.Format("{0}{1}", key, newKeySuffix);
                        newKeySuffix++;
                    } while (context.ContainsKey(newKey));

                    context.Add(newKey, httpResponseMessage);   
                }
            }

            return httpResponseMessage;
        }
    }
}
