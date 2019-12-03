using NorthShoreSurfApp.Database;
using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewModels;
using Plugin.DeviceOrientation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace NorthShoreSurfApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class HomePage : ContentPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        private HomeViewModel HomeViewModel { get => (HomeViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public HomePage()
        {
            // Hide default navigation bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            // Initialize the page
            InitializeComponent();
            // Use safe area on iOS
            On<iOS>().SetUseSafeArea(true);
            // Get root grid
            Grid grid = (Grid)Content;
            // Get safe area margins
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            // Set safe area margins
            grid.Margin = safeAreaInset;

            // Tapped on weather and ocean forecasts
            TapGestureRecognizer frameTap = new TapGestureRecognizer();
            frameTap.Tapped += async (sender, e) =>
            {
                if (sender == frameOpeningHours)
                    await Navigation.PushModalAsync(new HomeDetailsPage(HomeDetailsPageType.OpeningHours));
                else if (sender == frameContactUs)
                    await Navigation.PushModalAsync(new HomeDetailsPage(HomeDetailsPageType.ContactInfo));
                else if (sender == frameWebcam)
                    await Navigation.PushModalAsync(new SurfingConditionsFullscreenPage(SurfingConditionsViewModel.VideoUrl), false);
                else if (sender == frameNextRide)
                {
                    // Show details about next ride
                }
            };
            frameOpeningHours.GestureRecognizers.Add(frameTap);
            frameContactUs.GestureRecognizers.Add(frameTap);
            frameWebcam.GestureRecognizers.Add(frameTap);
            frameNextRide.GestureRecognizers.Add(frameTap);

            HomeViewModel.GetInformation();
        }

        #endregion

        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        // OnAppearing
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Navigation.ModalStack.Count == 0)
            {
                // Orientation
                CrossDeviceOrientation.Current.LockOrientation(Plugin.DeviceOrientation.Abstractions.DeviceOrientations.Portrait);
                // Show status bar on android
                App.ScreenService.ShowStatusBar();
            }
        }

        #endregion
    }
}
