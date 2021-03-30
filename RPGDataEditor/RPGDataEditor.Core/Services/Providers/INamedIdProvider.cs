namespace RPGDataEditor.Core.Providers
{
    public interface INamedIdProvider<TModel>
    {
        string GetName(int id);
        int GetId(string name);
    }
}
