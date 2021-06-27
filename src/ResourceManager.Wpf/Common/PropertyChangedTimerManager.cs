using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ResourceManager.Wpf.Common
{
    public static class PropertyChangedTimerManager
    {
        public class PropertyChangeRegistration
        {
            public PropertyChangeRegistration(object context, string propertyName, Action onChangedCall)
            {
                Context = context;
                PropertyName = propertyName;
                OnChangedCall = onChangedCall;
                Property = context.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (Property == null || Property.GetGetMethod() == null)
                {
                    throw new ArgumentException("Invalid property, must be public, non-static, with get method");
                }
                Value = Property.GetValue(Context);
            }

            public object Value { get; set; }
            public object Context { get; }
            public string PropertyName { get; }
            public PropertyInfo Property { get; }
            public Action OnChangedCall { get; }
        }

        private static System.Timers.Timer timer;

        private static readonly IList<PropertyChangeRegistration> registrations = new List<PropertyChangeRegistration>();

        private static readonly Func<PropertyChangeRegistration, object> keySelector = (x) => new { x.Context, x.PropertyName };

        public static void NotifyPropertyChanged(object context, string propertyName, Action onPropertyChanged)
        {
            if (onPropertyChanged == null)
            {
                throw new ArgumentNullException(nameof(onPropertyChanged));
            }
            registrations.Add(new PropertyChangeRegistration(context, propertyName, onPropertyChanged));
        }

        public static void RemovePropertyChanged(object context, string propertyName, Action onPropertyChanged)
        {
            int index = -1;
            for (int i = 0; i < registrations.Count; i++)
            {
                if (EqualityComparer<object>.Default.Equals(registrations[i].Context == context)
                    && EqualityComparer<string>.Default.Equals(registrations[i].PropertyName == propertyName)
                    && EqualityComparer<Action>.Default.Equals(registrations[i].OnChangedCall == onPropertyChanged))
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0)
            {
                registrations.RemoveAt(index);
            }
        }

        public static void Run()
        {
            if (timer != null)
            {
                timer.Start();
                return;
            }
            timer = new System.Timers.Timer() {
                AutoReset = true,
                Interval = 50
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private static async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IList<Task> tasks = new List<Task>();
            foreach (PropertyChangeRegistration registration in registrations.GroupBy(keySelector, x => x).SelectMany(x => x))
            {
                object value = registration.Property.GetValue(registration.Context);
                if (!EqualityComparer<object>.Default.Equals(registration.Value, value))
                {
                    tasks.Add(Task.Run(registration.OnChangedCall));
                    registration.Value = value;
                }
            }
            await Task.WhenAll(tasks);
        }

        public static void Stop() => timer?.Stop();
    }
}
