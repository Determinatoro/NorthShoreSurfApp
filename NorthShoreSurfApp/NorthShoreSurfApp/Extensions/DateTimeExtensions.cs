using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace NorthShoreSurfApp
{
    public static class DateTimeExtensions
    {
        public static string toCarpoolingFormat(this DateTime dateTime)
        {
            if( (dateTime - DateTime.Today).TotalDays < 8)
            {
                return dateTime.ToString("dddd");
            } else
            {
                return dateTime.ToString("MM/dd/yy");
            }
            
        }
    }
}




