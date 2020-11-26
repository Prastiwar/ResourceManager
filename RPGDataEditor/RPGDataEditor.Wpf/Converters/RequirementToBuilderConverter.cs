using RPGDataEditor.Core.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class RequirementToBuilderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => new PlayerRequirementBuilder() { Model = value as PlayerRequirementModel };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is PlayerRequirementBuilder builder ? builder.Model : null;
    }
}
