using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NorthShoreSurfApp.ViewModels;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        private WelcomeViewModel WelcomeViewModel { get => (WelcomeViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public WelcomePage(WelcomePageContentSite welcomePageContentSite = WelcomePageContentSite.Welcome)
        {
            // Hide default navigation bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            // Initialize page
            InitializeComponent();
            // Use safe area on iOS
            On<iOS>().SetUseSafeArea(true);
            // Get root grid
            Grid grid = (Grid)Content;
            // Get safe area margins
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            // Set safe area margins
            grid.Margin = safeAreaInset;

            // Set page in model
            WelcomeViewModel.Page = this;
            // Set content site in model
            WelcomeViewModel.WelcomePageContentSite = welcomePageContentSite;
        }

        #endregion
    }
}