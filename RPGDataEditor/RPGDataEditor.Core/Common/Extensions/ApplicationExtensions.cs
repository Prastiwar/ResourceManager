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

    }
}
