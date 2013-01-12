using System.Net.Http;

namespace Amido.Testing.Http
{
    public interface IVerb
    {
        IRestClient WithVerb(HttpMethod httpMethod);
    }
}