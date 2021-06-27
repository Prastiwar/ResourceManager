namespace ResourceManager.Mvvm.Navigation
{
    public class ModelDialogParameters<TModel> : DialogParametersBuilder
    {
        public ModelDialogParameters(TModel model) => Model = model;

        public TModel Model {
            get => Get<TModel>();
            set => Set(value);
        }
    }
}
