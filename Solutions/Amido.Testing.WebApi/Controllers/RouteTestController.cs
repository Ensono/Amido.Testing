using System.Net;
using System.Web.Http;
using System.Net.Http;

namespace Amido.Testing.WebApi.Controllers
{
    public class RouteTestController : ApiController
    {
        //
        // GET: /RouteTest/

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Put()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DoAPost(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DoAComplexPost(ComplexViewModel complexViewModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }

    public class ComplexViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
    }
}
