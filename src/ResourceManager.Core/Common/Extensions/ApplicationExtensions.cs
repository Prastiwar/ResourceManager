using System;
using System.Windows;

namespace ResourceManager.Core
{
    public static class ApplicationExtensions
    {
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
