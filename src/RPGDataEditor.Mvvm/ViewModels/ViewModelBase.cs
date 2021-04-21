namespace RPGDataEditor.Mvvm
{
    public abstract class ViewModelBase : BindableClass
    {
        public ViewModelBase(ViewModelContext context) => Context = context;

        public ViewModelContext Context { get; }
    }
}
