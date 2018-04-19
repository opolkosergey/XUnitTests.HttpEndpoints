namespace XUnitTests.Http.Interfaces
{
    internal interface IResponseDeserializer
    {
        T Deserialize<T>(string content);
    }
}
