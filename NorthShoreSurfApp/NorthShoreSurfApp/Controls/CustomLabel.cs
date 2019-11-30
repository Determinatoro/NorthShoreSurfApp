using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp
{
    public class CustomLabel : Label
    {
        public static readonly BindableProperty LinesProperty = BindableProperty.Create(nameof(Lines), typeof(int), typeof(CustomLabel), -1);
        public static readonly BindableProperty TextSizeProperty = BindableProperty.Create(nameof(Lines), typeof(int), typeof(CustomLabel), 16.0);

        public int Lines
        {
            get { return (int)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }
        public double TextSize
        {
            get { return (double)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }
    }
}
