using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RPGDataEditor.Mvvm.Navigation
{
    public class DialogParametersBuilder
    {
        private readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        public Exception Exception {
            get => Get<Exception>();
            set => Set(value);
        }

        public bool IsSuccess {
            get => Get<bool>();
            set => Set(value);
        }

        public DialogParametersBuilder WithResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
            return this;
        }

        public DialogParametersBuilder WithException(Exception exception)
        {
            Exception = exception;
            return this;
        }

        protected virtual IDialogParameters CreateDialogParameters() => new DialogParameters();

        public IDialogParameters Build()
        {
            IDialogParameters parameters = CreateDialogParameters();
            foreach (KeyValuePair<string, object> pair in properties)
            {
                parameters.Add(pair.Key, pair.Value);
            }
            return parameters;
        }

        protected T Get<T>([CallerMemberName] string propertyName = null)
        {
            if (properties.TryGetValue(propertyName, out object value))
            {
                if (value != null)
                {
                    return (T)value;
                }
            }
            return default;
        }

        protected void Set<T>(T value, [CallerMemberName] string propertyName = null) => properties[propertyName] = value;

        protected void Set<T>(ref T variable, T value, [CallerMemberName] string propertyName = null)
        {
            variable = value;
            properties[propertyName] = value;
        }
    }
}
