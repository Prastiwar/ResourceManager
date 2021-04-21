namespace RPGDataEditor.Providers
{
    public interface IModelProvider<TModel>
    {
        TModel CreateModel(string name);
    }
}
