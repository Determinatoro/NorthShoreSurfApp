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
          
            InitializeComponent();
            
           
            

            rideList.ItemTapped += Ride_Clicked;
            RidesTab.Clicked += RidesTab_Clicked;
            RequestsTab.Clicked += RequestsTab_Clicked;
        }


        private async void Ride_Clicked(object sender, EventArgs e)
        {
            if (sender == rideList)
            {
                await Navigation.PushAsync(new NewCarpoolingPage());
            }

        }

        private void RidesTab_Clicked(object sender, EventArgs e)
        {
            if(sender == RidesTab)
            {
                RidesTab.Background = Color.White;
                
            }
        }

        private void RequestsTab_Clicked(object sender, EventArgs e)
        {

        }

       

    }
}