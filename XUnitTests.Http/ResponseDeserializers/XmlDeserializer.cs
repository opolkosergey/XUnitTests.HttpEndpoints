using System.IO;
using System.Xml.Serialization;
using XUnitTests.Http.Interfaces;

namespace XUnitTests.Http.ResponseDeserializers
{
    internal class XmlDeserializer : IResponseDeserializer
    {
        public T Deserialize<T>(string content)
        {
            T resultModel;
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(content))
            {
                resultModel = (T)serializer.Deserialize(reader);
            }

            return resultModel;
        }
    }
}
