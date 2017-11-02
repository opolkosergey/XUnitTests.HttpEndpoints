﻿using System.Net.Http;
using XUnitTests.Core.Base;

namespace HttpEndpointsTests.EndPoints.TutByHomePageEndPoint
{
    public class TutByHomePageEndPoint : HttpEndPoint<string>
    {        
        public override string Uri => "http://www.tut.by";

        public override string RequestUri => null;

        public override HttpMethod HttpMethod => HttpMethod.Get;
    }
}