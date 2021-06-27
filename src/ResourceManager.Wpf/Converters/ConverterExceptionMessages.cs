using System;

namespace ResourceManager.Wpf.Converters
{
    public static class ConverterExceptionMessages
    {
        public static string NotSupportedConversion(Type from, Type to) => $"Conversion from {from.Name} to {to.Name} is not supported";
        public static NotSupportedException GetNotSupportedConversion(Type from, Type to) => new NotSupportedException(NotSupportedConversion(from, to));
    }
}
