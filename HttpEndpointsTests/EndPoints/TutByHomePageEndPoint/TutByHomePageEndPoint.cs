using System.Net.Http;
using XUnitTests.Core.Base;

namespace HttpEndpointsTests.EndPoints.TutByHomePageEndPoint
{
    public class TutByHomePageEndPoint : HttpEndPoint<string>
    {
        protected override string Uri => "http://www.tut.by";

        protected override string RequestUri => null;

        protected override HttpMethod HttpMethod => HttpMethod.Get;
    }
}