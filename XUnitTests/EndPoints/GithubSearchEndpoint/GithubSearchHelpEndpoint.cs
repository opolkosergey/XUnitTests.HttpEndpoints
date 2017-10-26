using System.Net.Http;
using XUnitTests.EndPoints.Base;

namespace XUnitTests.EndPoints.GithubSearchEndpoint
{
    public class GithubSearchHelpEndpoint : HttpEndPoint<GithubSearchHelpResponseModel>
    {
        public override string Uri => "https://help.github.com";

        public override string RequestUri => "/search/search.json";

        public override HttpMethod HttpMethod => HttpMethod.Get;
    }
}
