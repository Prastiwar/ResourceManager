using ResourceManager.Wpf.Controls;
using System;

namespace ResourceManager.Wpf.Providers
{
    public interface IAutoTemplateProvider
    {
        AutoTemplate Resolve(Type type);
    }
}