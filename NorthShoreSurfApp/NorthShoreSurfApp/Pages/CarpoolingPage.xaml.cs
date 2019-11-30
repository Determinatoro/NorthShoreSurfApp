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
using NorthShoreSurfApp.ViewModels.CarpoolingPage;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;
using FFImageLoading.Forms;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarpoolingPage : ContentPage
    {
        public CarpoolingPageViewModel CarpoolingPageViewModel { get => (CarpoolingPageViewModel)this.BindingContext; }
        public CarpoolingPage()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            Grid grid = (Grid)Content;
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            grid.Margin = safeAreaInset;

            //rideList.ItemSelected
            RidesTab.Toggled += RidesTab_Clicked;
            navigationBar.ButtonOne.Clicked += Plus_Clicked;
            navigationBar.ButtonTwo.Clicked += Confirmations_Clicked;

            

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // var userId = int.Parse(App.LocalDataService.GetValue(nameof(LocalDataKeys.UserId)));

            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                false, () => App.DataService.GetCarpoolRides(),
                async (response) =>
                {
                    if (response.Success)
                    {
                        
                        CarpoolingPageViewModel.Rides = new ObservableCollection<CarpoolRide>(response.Result);
                    }
                    else
                    {
                        CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                        await PopupNavigation.Instance.PushAsync(customDialog);
                    }
                });

            
        }

        private async void Plus_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewCarpoolingPage());

        }
        private async void Confirmations_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new ());
        }

        private void rideSelected(object sender, EventArgs e)
        {
            if (sender == rideList)
            {


                CarpoolRide SelectedRide = (CarpoolRide)rideList.SelectedItem;
                Console.WriteLine(SelectedRide.DepartureTimeHourString);
                
            }
        }

        private void RidesTab_Clicked(object sender, EventArgs e)
        {
            if (sender == RidesTab)
            {
                App.DataService.GetData(NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait, false, () => App.DataService.GetCarpoolRequests(), (response) =>
                {
                    if (response.Success)
                    {
                        CarpoolingPageViewModel.Requests = new ObservableCollection<CarpoolRequest>(response.Result);
                        
                        
                    }

                });
            }
        }
    }
}
