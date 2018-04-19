using System.Net.Http;
using XUnitTests.Http.Base;

namespace HttpEndpointsTests.EndPoints.NonExistentEndpoint
{
    public class NotExistentEndpoint : HttpEndPointWithUrlRequestModelAndBody<NotExistentEndpointRequestModel, NotExistentEndpointRequestBodyModel, string>
    {
        protected override string Uri => "http://www.notfound.com";

        protected override string RequestUri => "/{id}";

        protected override HttpMethod HttpMethod => HttpMethod.Post;
    }
}
