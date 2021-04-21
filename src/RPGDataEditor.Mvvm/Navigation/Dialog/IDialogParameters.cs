namespace RPGDataEditor.Mvvm.Navigation
{
    public interface IDialogParameters
    {
        void Add(string parameter, object value);

        object GetValue(string parameter);
    }
}
