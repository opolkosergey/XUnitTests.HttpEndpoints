using System.Net;
using System.Threading.Tasks;
using HttpEndpointsTests.EndPoints.GithubSearchEndpoint;
using HttpEndpointsTests.EndPoints.NonExistentEndpoint;
using HttpEndpointsTests.EndPoints.TutByHomePageEndPoint;
using Xunit;

namespace HttpEndpointsTests
{
    public class UnitTests
    {
        [Fact]
        public async Task GithubSearchHelpEndpointTest()
        {
            var endpoint = new GithubSearchHelpEndpoint();

            var result = await endpoint.GetResult();

            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.NotNull(result.ResponseModel);
        }

        [Fact]
        public async Task TutByHomePageEndpointTest()
        {
            var endpoint = new TutByHomePageEndPoint();

            var result = await endpoint.GetResult();

            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.NotNull(result.ResponseModel);
        }

        [Fact]
        public async Task NotExistentEndpointTest()
        {
            var endpoint = new NotExistentEndpoint()                    
                .WithUrlRequestModel(new NotExistentEndpointRequestModel                
                {                
                    Id = 1,                    
                    Symbol = "x",
                    Pages = new []{ 1, 2 ,3 }
                })                    
                .WithRequestBodyModel(new NotExistentEndpointRequestBodyModel                                   
                {                                       
                    Data = 4
                })
                .WithHeader("Cookie", "httponly");

            var result = await endpoint.GetResult();

            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);            
        }
    }
}
