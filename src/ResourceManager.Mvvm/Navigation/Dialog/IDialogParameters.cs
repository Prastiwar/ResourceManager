using System.Collections.Generic;

namespace ResourceManager.Mvvm.Navigation
{
    public interface IDialogParameters
    {
        void Add(string parameter, object value);

        object GetValue(string parameter);

        IEnumerable<string> Parameters { get; }
    }
}
