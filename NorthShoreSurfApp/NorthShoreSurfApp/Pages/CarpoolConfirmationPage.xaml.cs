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
    public partial class CarpoolConfirmationPage : ContentPage
    {
        public CarpoolConfirmationPageViewModel CarpoolConfirmationPageViewModel { get => (CarpoolConfirmationPageViewModel)this.BindingContext; }

        List<Event> events = new List<Event>();
        public CarpoolConfirmationPage()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            Grid grid = (Grid)Content;
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            grid.Margin = safeAreaInset;

            navigationBar.BackButtonClicked += NavigationBar_BackButtonClicked;

        }

        private void NavigationBar_BackButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CarpoolingPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var userId = int.Parse(App.LocalDataService.GetValue(nameof(LocalDataKeys.UserId)));

            App.DataService.GetData(
                          NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                          true,
                          () => App.DataService.GetCarpoolConfirmations(),
                          async (response) =>
                          {
                              if (response.Success)
                              {
                                  List<CarpoolConfirmation> ownRideConfirmations = new List<CarpoolConfirmation>();
                                  foreach(CarpoolConfirmation confirmation in response.Result)
                                  {
                                      if(confirmation.CarpoolRide.DriverId == userId && confirmation.PassengerId != userId)
                                      {
                                          ownRideConfirmations.Add(confirmation);
                                      }
                                  }
                                  CarpoolConfirmationPageViewModel.Requests = new ObservableCollection<CarpoolConfirmation>(ownRideConfirmations);
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

        }

    }
}
