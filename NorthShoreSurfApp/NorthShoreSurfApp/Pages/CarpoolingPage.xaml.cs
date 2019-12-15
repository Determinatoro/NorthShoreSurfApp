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
using NorthShoreSurfApp.Services;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarpoolingPage : ContentPage, ITabbedPageService
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
            // Hide default navigation bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            // Initialize the page
            InitializeComponent();
            // iOS safe area insets
            ((Grid)Content).SetIOSSafeAreaInsets(this);

            // ItemSelected
            rideList.ItemSelected += ListItem_Selected;
            requestList.ItemSelected += ListItem_Selected;

            // Toggled
            ctbCarpool.Toggled += (sender, args) =>
            {
                switch (CarpoolingPageViewModel.ToggleType)
                {
                    case ToggleType.Left:
                        GetCarpoolRides(true);
                        break;
                    case ToggleType.Right:
                        GetCarpoolRequests(true);
                        break;
                }
            };

            // Plus icon clicked in navigation bar
            navigationBar.ButtonOne.Clicked += async (sender, args) =>
            {
                NewCarpoolingPage newCarpoolingPage = new NewCarpoolingPage(CarpoolingPageViewModel.ToggleType == ToggleType.Left ? NewCarpoolingPageType.NewCarpoolRide : NewCarpoolingPageType.NewCarpoolRequest);
                newCarpoolingPage.Disappearing += (sender, args) => UpdateList();
                await Navigation.PushModalAsync(newCarpoolingPage);
            };
            // Message icon clicked in navigation bar
            navigationBar.ButtonTwo.Clicked += async (sender, args) =>
            {
                CarpoolConfirmationPage carpoolConfirmationPage = new CarpoolConfirmationPage();
                carpoolConfirmationPage.Disappearing += (sender, args) => UpdateList();
                await Navigation.PushModalAsync(carpoolConfirmationPage);
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
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Update the selected list
        /// </summary>
        private void UpdateList()
        {
            switch (CarpoolingPageViewModel.ToggleType)
            {
                case ToggleType.Left:
                    GetCarpoolRides();
                    break;
                case ToggleType.Right:
                    GetCarpoolRequests();
                    break;
            }
        }
        /// <summary>
        /// Get carpool rides from the data service
        /// </summary>
        private void GetCarpoolRides(bool showDialog = false)
        {
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                showDialog,
                async () =>
                {
                    var response = await App.DataService.GetUsersCarpoolRides(AppValuesService.UserId.Value);
                    var response2 = await App.DataService.GetPendingCarpoolConfirmations(AppValuesService.UserId.Value);
                    return new Tuple<DataResponse<CarpoolResult>, DataResponse<List<CarpoolConfirmation>>>(response, response2);
                },
                (response) =>
                {
                    if (response.Item1.Success && response.Item2.Success)
                    {
                        // Sort rides in own and others and separate them with a divider
                        var tempList = response.Item1.Result.OwnRides;
                        if (tempList.Count > 0 && response.Item1.Result.OtherRides.Count > 0)
                            tempList.Add(new CarpoolRide() { IsDivider = true });
                        tempList.AddRange(response.Item1.Result.OtherRides);
                        // Set carpool rides
                        CarpoolingPageViewModel.Rides = tempList;
                        // Set carpool confirmations
                        CarpoolingPageViewModel.CarpoolConfirmations = response.Item2.Result;
                    }
                    else if (!response.Item1.Success)
                    {
                        // Show error
                        this.ShowMessage(response.Item1.ErrorMessage);
                    }
                    else if (!response.Item2.Success)
                    {
                        // Show error
                        this.ShowMessage(response.Item2.ErrorMessage);
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
                showDialog,
                async () =>
                {
                    var response = await App.DataService.GetUsersCarpoolRequests(AppValuesService.UserId.Value);
                    var response2 = await App.DataService.GetPendingCarpoolConfirmations(AppValuesService.UserId.Value);
                    return new Tuple<DataResponse<RequestResult>, DataResponse<List<CarpoolConfirmation>>>(response, response2);
                },
                (response) =>
                {
                    if (response.Item1.Success && response.Item2.Success)
                    {
                        // Sort requests in own and others and separate them with a divider
                        var tempList = response.Item1.Result.OwnRequests;
                        if (tempList.Count > 0 && response.Item1.Result.OtherRequests.Count > 0)
                            tempList.Add(new CarpoolRequest() { IsDivider = true });
                        tempList.AddRange(response.Item1.Result.OtherRequests);
                        // Set carpool requests
                        CarpoolingPageViewModel.Requests = tempList;
                        // Set carpool confirmations
                        CarpoolingPageViewModel.CarpoolConfirmations = response.Item2.Result;
                    }
                    else if (!response.Item1.Success)
                    {
                        // Show error
                        this.ShowMessage(response.Item1.ErrorMessage);
                    }
                    else if (!response.Item2.Success)
                    {
                        // Show error
                        this.ShowMessage(response.Item2.ErrorMessage);
                    }
                });
        }

        #endregion

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        private async void ListItem_Selected(object sender, EventArgs e)
        {
            var listView = sender as Xamarin.Forms.ListView;
            if (listView.SelectedItem == null)
                return;

            // CarpoolRides list
            if (sender == rideList)
            {
                CarpoolRide selectedRide = (CarpoolRide)rideList.SelectedItem;
                CarpoolDetailsPage carpoolDetailsPage = new CarpoolDetailsPage(selectedRide);
                carpoolDetailsPage.Disappearing += (sender, args) =>
                {
                    GetCarpoolRides();
                };
                await Navigation.PushModalAsync(carpoolDetailsPage);
                rideList.SelectedItem = null;
            }
            // CarpoolRequests list
            else if (sender == requestList)
            {
                CarpoolRequest selectedRequest = (CarpoolRequest)requestList.SelectedItem;
                CarpoolDetailsPage carpoolDetailsPage = new CarpoolDetailsPage(selectedRequest);
                carpoolDetailsPage.Disappearing += (sender, args) =>
                {
                    GetCarpoolRequests();
                };
                await Navigation.PushModalAsync(carpoolDetailsPage);
                requestList.SelectedItem = null;
            }
        }
        private void RidesTab_Clicked(object sender, EventArgs e)
        {
            UpdateList();
        }

        #endregion

        /*****************************************************************/
        // INTERFACE METHODS
        /*****************************************************************/
        #region Interface methods

        // OnPageSelected
        public void OnPageSelected()
        {
            this.DelayedTask(200, () =>
            {
                GetCarpoolRides(true);
            });
        }

        #endregion
    }
}
