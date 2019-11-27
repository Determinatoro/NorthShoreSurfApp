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
using NorthShoreSurfApp.ViewModels;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCarpoolingPage : ContentPage
    {
        public NewCarpoolingPageViewModel NewCarpoolingPageViewModel { get => (NewCarpoolingPageViewModel)this.BindingContext; }
        public NewCarpoolingPage()
        {

            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            Grid grid = (Grid)Content;
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            grid.Margin = safeAreaInset; 

            carpoolNavigationBar.BackButtonClicked += NavigationBar_BackButtonClicked;
        }


        private void NavigationBar_BackButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
                Navigation.RemovePage(this);
        }

        private void CreateEvent_ButtonClicked(object sender, EventArgs e)
        {
            if(sender == CreateEventButton)
            {
                /* App.DataService.GetData(NorthShoreSurfApp.Resources.AppResources.could_not_create_carpool_event, false, () => App.DataService.CreateCarpoolRide) */
            }
        }



    }
}