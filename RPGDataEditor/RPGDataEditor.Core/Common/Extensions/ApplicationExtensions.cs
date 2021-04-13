using Prism;
using Prism.Ioc;
using System;
using System.Windows;

namespace RPGDataEditor.Core
{
    public static class ApplicationExtensions
    {
        public static object TryResolve(this Application app, Type type)
        {
            try
            {
                return app is PrismApplicationBase prismApp ? prismApp.Container.Resolve(type) : default;
            }
            catch (ContainerResolutionException ex)
            {
                return null;
            }
        }

        public static T TryResolve<T>(this Application app) => TryResolve(app, typeof(T)) is T item ? item : default;

        public static Window FindWindow(this Application app, Predicate<Window> predicate)
        {
            foreach (object window in app.Windows)
            {
                if (window is Window win && predicate(win))
                {
                    return win;
                }
            }
            return null;
        }
    }
}
