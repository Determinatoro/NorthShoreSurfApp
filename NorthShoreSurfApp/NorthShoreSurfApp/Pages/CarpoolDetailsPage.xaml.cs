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

        private DataResponse<List<CarpoolConfirmation>> dataResponse;

        public CarpoolDetailsPage(CarpoolRide ride)
        {
            _ride = ride;
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
            btnJoin.Clicked += Button_Clicked;

        }



        private async void Button_Clicked(object sender, EventArgs e)
        {
            App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                        true,
                        () => App.DataService.SignUpToCarpoolRide(1, 1),
                        async (response) =>
                        {
                            if (response.Success)
                            {
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                await PopupNavigation.Instance.PushAsync(customDialog);
                            }
                        });
        }
    }
}