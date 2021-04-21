using System.Collections.Generic;

namespace RPGDataEditor.Mvvm.Navigation
{
    public class DialogParameters : IDialogParameters
    {
        public IEnumerable<string> Parameters => throw new System.NotImplementedException();

        public void Add(string parameter, object value) => throw new System.NotImplementedException();
        public object GetValue(string parameter) => throw new System.NotImplementedException();
    }
}
