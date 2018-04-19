using System.Net;
using System.Net.Http.Headers;

namespace XUnitTests.Http.Models
{
    public class HttpEndPointResult<TResponseModel>
    {
        public TResponseModel ResponseModel { get; set; }

        public HttpResponseHeaders ResponseHeaders { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
