using RPGDataEditor.Mvvm.Navigation;

namespace RPGDataEditor.Mvvm.Commands
{
    public static class ShowDialogQueryHelper
    {
        public static ShowDialogQuery CreateModelQuery<TModel>(TModel model) => new ShowDialogQuery(typeof(TModel).Name, new ModelDialogParameters<TModel>(model).Build());
    }
}
