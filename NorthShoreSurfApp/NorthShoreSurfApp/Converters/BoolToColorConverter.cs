using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp.Converters
{
    class BoolToColorConverter : IValueConverter
    {
        public Color TrueColor { get; set; }
        public Color FalseColor { get; set; }

        private object Value { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                Value = value;
                if (value is bool)
                {
                    return ((bool)value) ? TrueColor : FalseColor;
                }
            }
            catch { }

            return Color.Transparent;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Value;
        }
    }
}
