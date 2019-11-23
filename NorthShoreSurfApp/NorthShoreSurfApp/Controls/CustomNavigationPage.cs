using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp
{
    public class CustomNavigationPage : NavigationPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty IconSelectedProperty = BindableProperty.Create(nameof(IconSelectedSource), typeof(string), typeof(CustomNavigationPage), null);
        public static readonly BindableProperty IconUnselectedProperty = BindableProperty.Create(nameof(IconUnselectedSource), typeof(string), typeof(CustomNavigationPage), null);

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

        public CustomNavigationPage(Page page) : base(page)
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
