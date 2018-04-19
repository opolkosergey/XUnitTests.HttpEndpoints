using Newtonsoft.Json;
using XUnitTests.Http.Interfaces;

namespace XUnitTests.Http.ResponseDeserializers
{
    internal class JsonDeserializer : IResponseDeserializer
    {
        public T Deserialize<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
