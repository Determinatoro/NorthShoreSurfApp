using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    /*****************************************************************/
    // TRANSLATE EXTENSIONS
    /*****************************************************************/
    #region Translate Extensions

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

    #endregion

    /*****************************************************************/
    // GENERAL EXTENSIONS
    /*****************************************************************/
    #region General Extensions

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

    #endregion

    /*****************************************************************/
    // PAGE EXTENSIONS
    /*****************************************************************/
    #region Page Extensions

    public static class PageExtensions
    {
        public static async void ShowMessage(this Xamarin.Forms.Page page, string errorMessage, string cancelTitle = null)
        {
            CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, errorMessage, cancelTitle == null ? Resources.AppResources.ok : cancelTitle);
            await PopupNavigation.Instance.PushAsync(customDialog);
        }
        public static async void ShowYesNo(this Xamarin.Forms.Page page, string message, Action accepted, Action denied = null)
        {
            CustomDialog customDialog = new CustomDialog(CustomDialogType.YesNo, message);
            customDialog.Accepted += (sender, args) =>
            {
                accepted();
            };
            customDialog.Denied += (sender, args) =>
            {
                denied?.Invoke();
            };
            await PopupNavigation.Instance.PushAsync(customDialog);
        }
        public static void DelayedTask(this Xamarin.Forms.Page page, int waitTimeInMilliseconds, Action action) {
            Timer timer = new Timer((state) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    action();
                });
            },
            null,
            // Wait time before timer runs
            waitTimeInMilliseconds,
            // No interval
            Timeout.Infinite);
        }
    }

    #endregion

    /*****************************************************************/
    // DATE TIME EXTENSIONS
    /*****************************************************************/
    #region DateTime Extensions

    public static class DateTimeExtensions
    {
        public static string ToCarpoolingFormat(this DateTime dateTime)
        {
            if ((dateTime - DateTime.Today).TotalDays < 8)
            {
                return dateTime.ToString("dddd").FirstCharToUpper();
            }
            else
            {
                return dateTime.ToString("dd-MM-yy");
            }
        }
    }

    #endregion

    /*****************************************************************/
    // VIEW EXTENSIONS
    /*****************************************************************/
    #region View Extensions

    public static class ViewExtensions
    {
        public static void SetIOSSafeAreaInsets(this View view, Xamarin.Forms.Page page)
        {
            // Use safe area on iOS
            page.On<iOS>().SetUseSafeArea(true);
            // Get safe area margins
            var safeAreaInset = page.On<iOS>().SafeAreaInsets();
            // Set safe area margins
            view.Margin = safeAreaInset;
        }
    }

    #endregion

    /*****************************************************************/
    // LIST EXTENSIONS
    /*****************************************************************/
    #region List Extensions

    public static class ListExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableList)
        {
            if (enumerableList != null)
            {
                // Create an emtpy observable collection object
                var observableCollection = new ObservableCollection<T>();

                // Loop through all the records and add to observable collection object
                foreach (var item in enumerableList)
                {
                    observableCollection.Add(item);
                }

                // Return the populated observable collection
                return observableCollection;
            }
            return null;
        }
    }

    #endregion
}
