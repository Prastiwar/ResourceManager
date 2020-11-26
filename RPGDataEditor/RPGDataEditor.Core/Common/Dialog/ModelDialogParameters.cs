namespace RPGDataEditor.Core
{
    public class ModelDialogParameters<TModel> : DialogParametersBuilder
    {
        public ModelDialogParameters(TModel model) => Model = model;
        public ModelDialogParameters(TModel model, object result)
        {
            Model = model;
            Result = result;
        }

        public TModel Model {
            get => Get<TModel>();
            set => Set(value);
        }

        public object Result {
            get => Get<object>();
            set => Set(value);
        }
    }
}
