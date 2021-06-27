using RPGDataEditor.Sample.Models;
using RPGDataEditor.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RPGDataEditor.Sample.Wpf.Converters
{
    public class DialoguePresentableNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is Dialogue dialogue ? $"(ID: {dialogue.Id}) {dialogue.Title}" : "None";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw ConverterExceptionMessages.GetNotSupportedConversion(typeof(string), typeof(Npc));
    }
}
