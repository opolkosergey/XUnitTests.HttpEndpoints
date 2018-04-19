using Newtonsoft.Json;
using XUnitTests.Http.Interfaces;

namespace HttpEndpointsTests.EndPoints.NonExistentEndpoint
{
    public class NotExistentEndpointRequestBodyModel : ISerializableBodyModel
    {
        public int Data { get; set; }

        public string GetContentType()
        {
            return "application/json";
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
