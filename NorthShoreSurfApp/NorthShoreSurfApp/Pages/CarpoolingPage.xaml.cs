using System;
using System.Collections.Generic;
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



            rideList.ItemTapped += Ride_Clicked;
            RidesTab.Toggled += RidesTab_Clicked;
            carpoolPageNavigationBar.ButtonOne.Clicked += Plus_Clicked;

            App.DataService.GetData(NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait, false, () => App.DataService.GetCarpoolRides, (response) =>
            {
                if(response.Success)
                {
                    //CarpoolingPageViewModel.Rides = response.Result;
                    return (null);
                }
                return(null);
            }


            
            
        }

        private async void Plus_Clicked(object sender, EventArgs e)
        {
           
                
                await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new NewCarpoolingPage()));
            
        }

        private async void Ride_Clicked(object sender, EventArgs e)
        {
            if (sender == rideList)
            {
                
            }

        }

        private void RidesTab_Clicked(object sender, EventArgs e)
        {
            if(sender == RidesTab)
            {
               
                
            }
        }

       

       

    }
}