namespace XUnitTests.Http.Interfaces
{
    public interface ISerializableBodyModel
    {
        string Serialize();

        string GetContentType();
    }
}
