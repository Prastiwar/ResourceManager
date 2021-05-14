﻿using Microsoft.Extensions.Configuration;
using ResourceManager;
using System.Windows;

namespace RPGDataEditor.Wpf.Controls
{
    public class SessionControl : ChangeableUserControl
    {
        private static readonly TypeSource[] sources = new TypeSource[] {
            new TypeSource("Ftp", typeof(IConfiguration)),
            new TypeSource("Local", typeof(IConfiguration)),
            new TypeSource("Sql", typeof(IConfiguration))
        };

        protected override void OnTemplateApplied()
        {
            if (GetBindingExpression(TypesSourceProperty) == null)
            {
                TypesSource = GetSources();
            }
            base.OnTemplateApplied();
        }

        protected virtual TypeSource[] GetSources() => sources;

        protected override object GetActualContentResource(TypeSource type) => Application.Current.TryFindResource(type.Name + "ConnectionContent");

        protected override TypeSource GetDataContextTypeSource()
        {
            if (DataContext is IConfiguration configuration)
            {
                return new TypeSource(configuration.GetDataSourceSection()["Type"], DataContext.GetType());
            }
            return null;
        }
    }
}
