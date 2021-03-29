using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace RPGDataEditor.Wpf
{
    public static class BindingExtensions
    {
        private class Dummy : DependencyObject
        {
            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(object), typeof(Dummy), new PropertyMetadata(null));
        }

        public static T GetResolvedValue<T>(this BindingExpression binding)
        {
            object val = binding.GetResolvedValue();
            return val != null ? (T)val : default;
        }

        public static object GetResolvedValue(this BindingExpression binding) =>
            binding.ResolvedSource.GetType().GetProperty(binding.ResolvedSourcePropertyName, BindingFlags.Public | BindingFlags.Instance).GetValue(binding.ResolvedSource);

        public static T GetResolvedValue<T>(this Binding binding)
        {
            object val = binding.GetResolvedValue();
            return val != null ? (T)val : default;
        }

        public static object GetResolvedValue(this Binding binding)
        {
            Dummy dummy = new Dummy();
            BindingOperations.SetBinding(dummy, Dummy.ValueProperty, binding);
            return dummy.GetValue(Dummy.ValueProperty);
        }

        public static BindingBase CloneBinding(this BindingBase bindingBase)
        {
            if (bindingBase is Binding binding)
            {
                Binding result = new Binding {
                    AsyncState = binding.AsyncState,
                    BindingGroupName = binding.BindingGroupName,
                    BindsDirectlyToSource = binding.BindsDirectlyToSource,
                    Converter = binding.Converter,
                    ConverterCulture = binding.ConverterCulture,
                    ConverterParameter = binding.ConverterParameter,
                    FallbackValue = binding.FallbackValue,
                    IsAsync = binding.IsAsync,
                    Mode = binding.Mode,
                    NotifyOnSourceUpdated = binding.NotifyOnSourceUpdated,
                    NotifyOnTargetUpdated = binding.NotifyOnTargetUpdated,
                    NotifyOnValidationError = binding.NotifyOnValidationError,
                    Path = binding.Path,
                    StringFormat = binding.StringFormat,
                    TargetNullValue = binding.TargetNullValue,
                    UpdateSourceExceptionFilter = binding.UpdateSourceExceptionFilter,
                    UpdateSourceTrigger = binding.UpdateSourceTrigger,
                    ValidatesOnDataErrors = binding.ValidatesOnDataErrors,
                    ValidatesOnExceptions = binding.ValidatesOnExceptions,
                    XPath = binding.XPath,
                    ValidatesOnNotifyDataErrors = binding.ValidatesOnNotifyDataErrors,
                    Delay = binding.Delay
                };
                if (binding.Source == null)
                {
                    if (string.IsNullOrEmpty(binding.ElementName))
                    {
                        result.RelativeSource = binding.RelativeSource;
                    }
                    else
                    {
                        result.ElementName = binding.ElementName;
                    }
                }
                else
                {
                    result.Source = binding.Source;
                }
                foreach (System.Windows.Controls.ValidationRule validationRule in binding.ValidationRules)
                {
                    result.ValidationRules.Add(validationRule);
                }
                return result;
            }
            else if (bindingBase is MultiBinding multiBinding)
            {
                MultiBinding result = new MultiBinding {
                    BindingGroupName = multiBinding.BindingGroupName,
                    Converter = multiBinding.Converter,
                    ConverterCulture = multiBinding.ConverterCulture,
                    ConverterParameter = multiBinding.ConverterParameter,
                    FallbackValue = multiBinding.FallbackValue,
                    Mode = multiBinding.Mode,
                    NotifyOnSourceUpdated = multiBinding.NotifyOnSourceUpdated,
                    NotifyOnTargetUpdated = multiBinding.NotifyOnTargetUpdated,
                    NotifyOnValidationError = multiBinding.NotifyOnValidationError,
                    StringFormat = multiBinding.StringFormat,
                    TargetNullValue = multiBinding.TargetNullValue,
                    UpdateSourceExceptionFilter = multiBinding.UpdateSourceExceptionFilter,
                    UpdateSourceTrigger = multiBinding.UpdateSourceTrigger,
                    ValidatesOnDataErrors = multiBinding.ValidatesOnDataErrors,
                    ValidatesOnExceptions = multiBinding.ValidatesOnDataErrors,
                    ValidatesOnNotifyDataErrors = multiBinding.ValidatesOnNotifyDataErrors,
                    Delay = multiBinding.Delay,
                };

                foreach (System.Windows.Controls.ValidationRule validationRule in multiBinding.ValidationRules)
                {
                    result.ValidationRules.Add(validationRule);
                }

                foreach (BindingBase childBinding in multiBinding.Bindings)
                {
                    result.Bindings.Add(CloneBinding(childBinding));
                }
                return result;
            }
            else if (bindingBase is PriorityBinding priorityBinding)
            {
                PriorityBinding result = new PriorityBinding {
                    BindingGroupName = priorityBinding.BindingGroupName,
                    FallbackValue = priorityBinding.FallbackValue,
                    StringFormat = priorityBinding.StringFormat,
                    TargetNullValue = priorityBinding.TargetNullValue,
                    Delay = priorityBinding.Delay
                };

                foreach (BindingBase childBinding in priorityBinding.Bindings)
                {
                    result.Bindings.Add(CloneBinding(childBinding));
                }
                return result;
            }

            throw new NotSupportedException("Failed to clone binding");
        }
    }
}
