using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;
using FFImageLoading.Forms;
using NorthShoreSurfApp.ViewModels;
using Plugin.DeviceOrientation;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarpoolingPage : ContentPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public CarpoolingPageViewModel CarpoolingPageViewModel { get => (CarpoolingPageViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CarpoolingPage()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            Grid grid = (Grid)Content;
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            grid.Margin = safeAreaInset;

            // ItemSelected

            rideList.ItemSelected += ListItem_Selected;
            requestList.ItemSelected += ListItem_Selected;

            // Toggled

            RidesTab.Toggled += RidesTab_Clicked;

            // Clicked

            // Plus icon clicked in navigation bar
            navigationBar.ButtonOne.Clicked += async (sender, args) =>
            {
                await Navigation.PushAsync(new NewCarpoolingPage());
            };
            // Message icon clicked in navigation bar
            navigationBar.ButtonTwo.Clicked += async (sender, args) =>
            {
                //await Navigation.PushAsync(new NewCarpoolingPage());
            };
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

            this.DelayedTask(500, () =>
            {
                GetCarpoolRides();
            });
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Get carpool rides from the data service
        /// </summary>
        private void GetCarpoolRides(bool showDialog = false)
        {
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                showDialog,
                () => App.DataService.GetCarpoolRides(),
                async (response) =>
                {
                    if (response.Success)
                    {
                        CarpoolingPageViewModel.Rides = new ObservableCollection<CarpoolRide>(response.Result);
                    }
                    else
                    {
                        // Show error
                        CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                        await PopupNavigation.Instance.PushAsync(customDialog);
                    }
                });
        }

        /// <summary>
        /// Get carpool requests from the data service
        /// </summary>
        private void GetCarpoolRequests(bool showDialog = false)
        {
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                false,
                () => App.DataService.GetCarpoolRequests(),
                async (response) =>
                {
                    if (response.Success)
                    {
                        CarpoolingPageViewModel.Requests = new ObservableCollection<CarpoolRequest>(response.Result);
                        rideList.IsVisible = false;
                        requestList.IsVisible = true;
                    }
                    else
                    {
                        // Show error
                        CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                        await PopupNavigation.Instance.PushAsync(customDialog);
                    }
                });
        }

        #endregion

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        private void ListItem_Selected(object sender, EventArgs e)
        {
            // CarpoolRides list
            if (sender == rideList)
            {
                CarpoolRide SelectedRide = (CarpoolRide)rideList.SelectedItem;
                Console.WriteLine(SelectedRide.DepartureTimeHourString);
            }
            // CarpoolRequests list
            else if (sender == requestList)
            {

            }
        }

        private void RidesTab_Clicked(object sender, EventArgs e)
        {
            if (RidesTab.SelectedToggleType == ToggleType.Right)
            {
                rideList.IsVisible = false;
                requestList.IsVisible = true;
                GetCarpoolRequests(true);
            }
            else if (RidesTab.SelectedToggleType == ToggleType.Left)
            {
                rideList.IsVisible = true;
                requestList.IsVisible = false;
                GetCarpoolRides(true);
            }
        }

        #endregion
    }
}
