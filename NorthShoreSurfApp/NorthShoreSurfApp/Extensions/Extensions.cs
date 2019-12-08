using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "NorthShoreSurfApp.Resources.AppResources";
        public string Text { get; set; }
        public bool MakeUpper { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;
            ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
            var str = resourceManager.GetString(Text, CultureInfo.CurrentCulture);
            return MakeUpper ? str.ToUpper() : str;
        }
    }

    public static class Extensions
    {
        public static T FindParentWithType<T>(this Element view)
        {
            if (view == null)
                return default(T);

            Element parent = view.Parent;
            while (parent != null)
            {
                if (parent.GetType() == typeof(T))
                    return (T)Convert.ChangeType(parent, typeof(T));
                parent = parent.Parent;
            }

            return default(T);
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
