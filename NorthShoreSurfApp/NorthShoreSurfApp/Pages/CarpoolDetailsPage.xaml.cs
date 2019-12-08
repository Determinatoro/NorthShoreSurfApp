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

        public CarpoolDetailsPage(CarpoolRide ride)
        {
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
            btnJoin.Clicked += BtnJoin_Clicked;
            _ride = ride;

        }

        private void BtnJoin_Clicked(object sender, EventArgs e)
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

        }

        private void BtnInvite_Clicked(object sender, EventArgs e)
        {
            var userId = int.Parse(App.LocalDataService.GetValue(nameof(LocalDataKeys.UserId)));

            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                            true,
                            () => App.DataService.InvitePassenger(_request.Id, 1),
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
            if (_ride != null)
            {
                if(_ride.Comment != null)
                CarpoolDetailsPageViewModel.Message = _ride.Comment;
                CarpoolDetailsPageViewModel.User.Add(_ride.Driver);
                CarpoolDetailsPageViewModel.Ride.Add(_ride);

            }
            else if (_request != null)
            {
                if(_request.Comment != null)
                CarpoolDetailsPageViewModel.Message = _request.Comment;
                CarpoolDetailsPageViewModel.Ride.Add(_request);

            }

        }

    }
}