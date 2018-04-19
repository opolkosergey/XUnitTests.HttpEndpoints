using System.Net.Http;
using XUnitTests.Http.Base;

namespace HttpEndpointsTests.EndPoints.GithubSearchEndpoint
{
    public class GithubSearchHelpEndpoint : HttpEndPoint<GithubSearchHelpResponseModel>
    {
        protected override string Uri => "https://help.github.com";

        protected override string RequestUri => "/search/search.json";

        protected override HttpMethod HttpMethod => HttpMethod.Get;
    }
}
