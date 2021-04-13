using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Providers
{
    public interface IModelProvider<TModel> where TModel : ObservableModel
    {
        TModel CreateModel(string name);
    }
}
