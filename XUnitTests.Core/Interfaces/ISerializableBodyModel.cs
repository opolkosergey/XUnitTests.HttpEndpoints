namespace XUnitTests.Core.Interfaces
{
    public interface ISerializableBodyModel
    {
        string Serialize();
        string GetContentType();
    }
}
