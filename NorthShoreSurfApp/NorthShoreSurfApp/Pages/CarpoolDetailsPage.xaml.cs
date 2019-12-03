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
using NorthShoreSurfApp.ViewModels;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;
using FFImageLoading.Forms;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarpoolDetailsPage : ContentPage
    {
        public CarpoolDetailsPageViewModel CarpoolDetailsPageViewModel { get => (CarpoolDetailsPageViewModel)this.BindingContext; }

        private CarpoolRide _ride;

        private CarpoolRequest _request;

        private bool _isRide;

        private bool _isRequest;

        public CarpoolDetailsPage(CarpoolRide ride)
        {
            _ride = ride;
            _isRide = true;
            _isRequest = false;
                Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
                Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
                InitializeComponent();
                // Use safe area on iOS
                On<iOS>().SetUseSafeArea(true);
                // Get root grid
                Grid grid = (Grid)Content;
                // Get safe area margins
                var safeAreaInset = On<iOS>().SafeAreaInsets();
                // Set safe area margins
                grid.Margin = safeAreaInset;
            navigationBar.BackButtonClicked += NavigationBar_BackButtonClicked;
            btnJoin.Clicked += BtnJoin_Clicked1;
            requestPage.IsVisible = false;
            ridePage.IsVisible = true;
            btnJoin.IsVisible = true;
            btnInvite.IsVisible = false;
        }

        private void BtnJoin_Clicked1(object sender, EventArgs e)
        {
            var userId = int.Parse(App.LocalDataService.GetValue(nameof(LocalDataKeys.UserId)));

            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                            true,
                            () => App.DataService.SignUpToCarpoolRide(_ride.Id, userId),
                            async (response) =>
                            {
                                if (response.Success)
                                {

                                    CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, "Created" + response.Success);
                                    await PopupNavigation.Instance.PushAsync(customDialog);
                                }
                                else
                                {
                                    CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                    await PopupNavigation.Instance.PushAsync(customDialog);
                                }
                            });
        }


        public CarpoolDetailsPage(CarpoolRequest request)
        {
            _isRide = false;
            _isRequest = true;
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            // Use safe area on iOS
            On<iOS>().SetUseSafeArea(true);
            // Get root grid
            Grid grid = (Grid)Content;
            // Get safe area margins
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            // Set safe area margins
            grid.Margin = safeAreaInset;
            navigationBar.BackButtonClicked += NavigationBar_BackButtonClicked;
            btnInvite.Clicked += BtnInvite_Clicked;
            _request = request;
            requestPage.IsVisible = true;
            ridePage.IsVisible = false;
            btnInvite.IsVisible = true;
            btnJoin.IsVisible = false;
        }

        private void BtnInvite_Clicked(object sender, EventArgs e)
        {
            var userId = int.Parse(App.LocalDataService.GetValue(nameof(LocalDataKeys.UserId)));

            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                            true,
                            () => App.DataService.InvitePassenger(_request.PassengerId, 1),
                            async (response) =>
                            {
                                if (response.Success)
                                {

                                    CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, "Created" + response.Success);
                                    await PopupNavigation.Instance.PushAsync(customDialog);
                                }
                                else
                                {
                                    CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                    await PopupNavigation.Instance.PushAsync(customDialog);
                                }
                            });
        }

        private void NavigationBar_BackButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CarpoolingPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(_isRide)
            {
                CarpoolDetailsPageViewModel.Name = _ride.Driver.FullName();
                CarpoolDetailsPageViewModel.PhoneNo = _ride.Driver.PhoneNo;
                CarpoolDetailsPageViewModel.Gender = "FIXGENDER";
                CarpoolDetailsPageViewModel.Age = _ride.Driver.Age;

                CarpoolDetailsPageViewModel.Price = _ride.PricePerPassengerString;
                CarpoolDetailsPageViewModel.Address = _ride.Address;
                CarpoolDetailsPageViewModel.DestinationAddress = _ride.DestinationAddress;
                CarpoolDetailsPageViewModel.DepartureTime = _ride.DepartureTimeHourString;
                CarpoolDetailsPageViewModel.DepartureTimeDay = _ride.DepartureTimeDayString;
                CarpoolDetailsPageViewModel.ZipCode = _ride.ZipcodeCityString + " " + _ride.City;
                CarpoolDetailsPageViewModel.DestinationZipCode = _ride.DestinationZipcodeCityString + " " + _ride.City;
                CarpoolDetailsPageViewModel.NumberOfSeats = _ride.NumberOfSeats;
                CarpoolDetailsPageViewModel.AvailableSeats = _ride.AvailableSeats;

            }
            else if(_isRequest)
            {
                CarpoolDetailsPageViewModel.Name = _request.Passenger.FullName();
                CarpoolDetailsPageViewModel.PhoneNo = _request.Passenger.PhoneNo;
                CarpoolDetailsPageViewModel.Gender = "FixGender";
                CarpoolDetailsPageViewModel.Age = _request.Passenger.Age;

                CarpoolDetailsPageViewModel.DepartureTimeDay = _request.DepartureTimeDayString;
                CarpoolDetailsPageViewModel.DepartureTime = _request.TimeInterval;
                CarpoolDetailsPageViewModel.ZipCode = _request.ZipCode + " " +_request.City;
                CarpoolDetailsPageViewModel.DestinationZipCode = "9480 Løkken";

            }
            
        }

    }
}