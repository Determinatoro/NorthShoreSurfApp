using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp
{
    public enum StringConverterOption
    {
        Normal,
        Upper,
        Lower
    }
}

namespace NorthShoreSurfApp.Converters
{
    public class StringConverter : IValueConverter
    {
        public StringConverterOption ConverterOption { get; set; }
        private string FallBackValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FallBackValue = (string)value;

            switch (ConverterOption)
            {
                case StringConverterOption.Normal:
                    return value;
                case StringConverterOption.Upper:
                    return ((string)value).ToUpper();
                case StringConverterOption.Lower:
                    return ((string)value).ToLower();
            }

            return string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return FallBackValue == null ? string.Empty : FallBackValue;
        }
    }
}
