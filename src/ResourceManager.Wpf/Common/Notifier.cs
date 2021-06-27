using System;
using System.Threading.Tasks;
using System.Windows;

namespace ResourceManager.Wpf
{
    public static class Notifier
    {
        public static void Call<T>(object target, Action<T> callback)
        {
            if (target is T tTarget)
            {
                callback.Invoke(tTarget);
            }
            if (target is FrameworkElement element)
            {
                if(element.DataContext is T tElement)
                {
                    callback.Invoke(tElement);
                }
            }
        }

        public static async Task CallAsync<T>(object target, Func<T, Task> callback)
        {
            if (target is T tTarget)
            {
                await callback.Invoke(tTarget);
            }
            if (target is FrameworkElement element)
            {
                if (element.DataContext is T tElement)
                {
                    await callback.Invoke(tElement);
                }
            }
        }
    }
}
