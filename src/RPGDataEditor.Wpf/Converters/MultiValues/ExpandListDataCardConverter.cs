using System;
using System.Globalization;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Converters
{
    public class ExpandListDataCardConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool isExpanded = (bool)values[0];
                bool expandable = (bool)values[1];
                bool noExpandableVisibility = (bool)values[2];
                if (expandable)
                {
                    return isExpanded;
                }
                return noExpandableVisibility;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new InvalidOperationException("This converter accepts at least 3 bindings", ex);
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException("Bindings value must be boolean", ex);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => new object[] { value };
    }
}
