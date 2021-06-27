using RPGDataEditor.Wpf.Controls;
using System;

namespace RPGDataEditor.Wpf.Providers
{
    public interface IAutoTemplateProvider
    {
        AutoTemplate Resolve(Type type);
    }
}