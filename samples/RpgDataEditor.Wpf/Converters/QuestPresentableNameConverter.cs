using RpgDataEditor.Models;
using ResourceManager.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RpgDataEditor.Wpf.Converters
{
    public class QuestPresentableNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is Quest quest ? $"(ID: {quest.Id}) {quest.Title}" : "None";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw ConverterExceptionMessages.GetNotSupportedConversion(typeof(string), typeof(Npc));
    }
}
