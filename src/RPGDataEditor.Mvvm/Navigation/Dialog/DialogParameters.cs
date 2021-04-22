using System.Collections.Generic;

namespace RPGDataEditor.Mvvm.Navigation
{
    public class DialogParameters : IDialogParameters
    {
        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        public IEnumerable<string> Parameters => parameters.Keys;

        public void Add(string parameter, object value) => parameters[parameter] = value;

        public object GetValue(string parameter) => parameters.TryGetValue(parameter, out object value) ? value : null;
    }
}
