namespace EntityFrameworkCore.Metadata.Json
{
    public interface IJsonValueConverter<T>
    {
        string Serialize(T value);

        T Deserialize(string value);
    }
}
