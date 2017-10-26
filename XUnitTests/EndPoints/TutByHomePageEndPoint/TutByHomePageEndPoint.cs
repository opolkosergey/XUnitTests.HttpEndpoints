using System.Net.Http;
using XUnitTests.EndPoints.Base;

namespace XUnitTests.EndPoints.TutByHomePageEndPoint
{
    public class TutByHomePageEndPoint : HttpEndPoint<string>
    {        
        public override string Uri => "http://www.tut.by";

        public override string RequestUri => null;

        public override HttpMethod HttpMethod => HttpMethod.Get;
    }
}