using RPGDataEditor.Wpf.Controls;
using System;

namespace RPGDataEditor.Wpf.Providers
{
    public interface IControlGenerateTemplateProvider
    {
        ControlGenerateTemplate Resolve(Type type);
    }
}