using System.Net;
using System.Threading.Tasks;
using Xunit;
using XUnitTests.EndPoints.GithubSearchEndpoint;
using XUnitTests.EndPoints.TutByHomePageEndPoint;

namespace XUnitTests
{
    public class UnitTests
    {
        [Fact]
        public async Task GithubSearchHelpEndpointTest()
        {
            var endpoint = new GithubSearchHelpEndpoint();

            var result = await endpoint.GetResult();

            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.NotEqual(null, result.ResponseModel);
        }

        [Fact]
        public async Task TutByHomePageEndpointTest()
        {
            var endpoint = new TutByHomePageEndPoint();

            var result = await endpoint.GetResult();

            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.NotEqual(null, result.ResponseModel);
        }
    }
}