using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace NorthShoreSurfApp
{
    public class CustomNavigationPage : Xamarin.Forms.NavigationPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty IconSelectedProperty = BindableProperty.Create("IconSelectedResource", typeof(string), typeof(CustomNavigationPage), null);
        public static readonly BindableProperty IconUnselectedProperty = BindableProperty.Create("IconUnselectedResource", typeof(string), typeof(CustomNavigationPage), null);

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        private void Init()
        {
            BarBackgroundColor = (Color)App.Current.Resources["BarBackground"];
            BarTextColor = Color.Black;
        }

        public CustomNavigationPage(Xamarin.Forms.Page page) : base(page)
        {
            Init();
        }

        public CustomNavigationPage() : base()
        {
            Init();
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public string IconSelectedSource
        {
            get { return (string)GetValue(IconSelectedProperty); }
            set { SetValue(IconSelectedProperty, value); }
        }

        public string IconUnselectedSource
        {
            get { return (string)GetValue(IconUnselectedProperty); }
            set { SetValue(IconUnselectedProperty, value); }
        }

        #endregion
    }
}
