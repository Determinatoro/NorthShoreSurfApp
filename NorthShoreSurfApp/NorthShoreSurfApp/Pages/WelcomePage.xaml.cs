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

            // Open pages as modal if it is used in the tabbed page else as navigation page
            Action<Xamarin.Forms.Page> openPage = async (page) =>
            {
                switch (welcomePageContentSite)
                {
                    case WelcomePageContentSite.Welcome:
                        {
                            await Navigation.PushAsync(page);
                            break;
                        }
                    case WelcomePageContentSite.UserNotLoggedIn:
                        {
                            await Navigation.PushModalAsync(page);
                            break;
                        }
                }
            };
            // Sign up
            WelcomeViewModel.SignUpCommand = new Command(() =>
            {
                openPage(new SignUpUserPage(SignUpUserPageType.SignUp));
            });
            // Log in
            WelcomeViewModel.LogInCommand = new Command(() =>
            {
                openPage(new SignUpUserPage(SignUpUserPageType.Login));
            });
            // Continue as guest
            WelcomeViewModel.ContinueAsGuestCommand = new Command(() =>
            {
                AppValuesService.SaveValue(LocalDataKeys.UserId, null);
                AppValuesService.SaveValue(LocalDataKeys.IsGuest, bool.TrueString);
                App.Current.MainPage = new RootTabbedPage();
            });
        }

        #endregion
    }
}