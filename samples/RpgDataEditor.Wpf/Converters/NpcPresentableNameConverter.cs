using RpgDataEditor.Models;
using ResourceManager.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RpgDataEditor.Wpf.Converters
{
    public class NpcPresentableNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is Npc npc ? $"(ID: {npc.Id}) {npc.Name}" : "None";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw ConverterExceptionMessages.GetNotSupportedConversion(typeof(string), typeof(Npc));
    }
}
