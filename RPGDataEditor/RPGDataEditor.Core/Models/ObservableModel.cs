using FluentValidation.Results;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RPGDataEditor.Core.Models
{
    public abstract class ObservableModel : BindableClass, ICopiable, IValidable
    {
        public virtual bool CopyValues(object fromCopy) => CopyProperties(fromCopy, (prop) => {
            PropertyInfo fromCopyProp = fromCopy.GetType().GetProperty(prop.Name);
            bool canSetValue = prop.SetMethod != null && fromCopyProp != null && fromCopyProp.PropertyType == prop.PropertyType;
            bool canGetValue = fromCopyProp.GetMethod != null;
            if (canSetValue && canGetValue)
            {
                object value = fromCopyProp.GetValue(fromCopy);
                prop.SetValue(this, value);
            }
        });

        private bool CopyProperties(object fromCopy, Action<PropertyInfo> copyProperty)
        {
            try
            {
                bool hasCopyRelatioship = GetType().IsAssignableFrom(fromCopy.GetType()) || fromCopy.GetType().IsAssignableFrom(GetType());
                if (!hasCopyRelatioship)
                {
                    return false;
                }
                PropertyInfo[] props = GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    copyProperty(prop);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public virtual object DeepClone() => DeepCopy.DeepCopier.Copy(this, GetType());

        public virtual object Clone() => MemberwiseClone();

        public event EventHandler<ValidationResult> Validated;

        private void CallOnEach<T>(object root, Action<T> action)
        {
            foreach (PropertyInfo property in root.GetType().GetProperties())
            {
                if (property.GetMethod == null || property.GetCustomAttribute<NotValidableAttribute>() != null)
                {
                    continue;
                }
                object value = property.GetValue(this);
                if (value is T tValue)
                {
                    action(tValue);
                }
                if (value is IEnumerable enumerable && !(enumerable is IEnumerable<char>))
                {
                    foreach (object item in enumerable)
                    {
                        if (item is T tItem)
                        {
                            action(tItem);
                        }
                    }
                }
            }
        }

        public virtual void NotifyValidate(ValidationResult result)
        {
            OnValidated(this, result);
            CallOnEach<IValidable>(this, validable => validable.NotifyValidate(result));
        }

        protected void OnValidated(object sender, ValidationResult result) => Validated?.Invoke(sender, result);
    }
}
