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
            // iOS safe area insets
            ((Grid)Content).SetIOSSafeAreaInsets(this);

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
                    if (AppValuesService.IsGuest())
                    {
                        var tabbedPage = ((RootTabbedPage)App.Current.MainPage);
                        tabbedPage.SelectedItem = tabbedPage.Children.Where(x => ((Xamarin.Forms.NavigationPage)x).RootPage is WelcomePage).FirstOrDefault();
                    }
                    else
                    {
                        // Show details about next ride
                    }
                }
            };
            frameOpeningHours.GestureRecognizers.Add(frameTap);
            frameContactUs.GestureRecognizers.Add(frameTap);
            frameWebcam.GestureRecognizers.Add(frameTap);
            frameNextRide.GestureRecognizers.Add(frameTap);

            // Get information for the page
            GetInformation();
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Get information for the home page from the data service
        /// </summary>
        public void GetInformation()
        {
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                false,
                async () =>
                {
                    // Todays opening hours
                    var response = await App.DataService.GetTodaysOpeningHours();
                    // Informatio about the next carpool ride
                    var response2 = await App.DataService.GetNextCarpoolRide();
                    return new Tuple<DataResponse<string>, DataResponse<CarpoolRide>>(response, response2);
                },
                async (response) =>
                {
                    if (response.Item1.Success)
                    {
                        // Set opening hours content
                        HomeViewModel.OpeningHoursContent = response.Item1.Result;
                    }
                    else
                    {
                        // Show error
                        await PopupNavigation.Instance.PushAsync(
                            new CustomDialog(CustomDialogType.Message, response.Item1.ErrorMessage)
                            );
                    }

                    if (response.Item1.Success)
                    {
                        // Set next carpool ride
                        HomeViewModel.NextCarpoolRide = response.Item2.Result;
                    }
                    else
                    {
                        // Show error
                        await PopupNavigation.Instance.PushAsync(
                            new CustomDialog(CustomDialogType.Message, response.Item1.ErrorMessage)
                            );
                    }
                });
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
