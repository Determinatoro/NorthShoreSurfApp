﻿using System;
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




            App.DataService.GetData(NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait, false, () => App.DataService.GetCarpoolRides(), (response) =>
            {
                if (response.Success)
                {
                    CarpoolingPageViewModel.Rides = new ObservableCollection<CarpoolRide>(response.Result);
                     

                } else
                {
                    CarpoolingPageViewModel.Rides.Add(new CarpoolRide()
                    {
                        ZipCode = "8000",
                        Address = "Parkvej",
                        City = "Aalborg",
                        DestinationZipCode = "9480",
                        DestinationAddress = "North Shore Surf",
                        DestinationCity = "Løkken",
                        NumberOfSeats = 2,
                        DepartureTime = new DateTime(2019, 1, 1, 13, 0, 0),
                        PricePerPassenger = 50

                    });
                    CarpoolingPageViewModel.Rides.Add(new CarpoolRide()
                    {
                        ZipCode = "9000",
                        Address = "Æblevej",
                        City = "Aalborg",
                        DestinationZipCode = "9480",
                        DestinationAddress = "North Shore Surf",
                        DestinationCity = "Løkken",
                        NumberOfSeats = 5,
                        DepartureTime = new DateTime(2019, 1, 1, 14, 30, 0),
                        PricePerPassenger = 50
                    });
                }

            });




    }

        private async void Plus_Clicked(object sender, EventArgs e)
        {
           
                
                await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new NewCarpoolingPage()));
            
        }



        private void Ride_Clicked(object sender, EventArgs e)
        {
            if (sender == rideList)
            {
                App.DataService.GetData(NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait, false, () => App.DataService.GetCarpoolRequests(), (response) =>
                {
                    if (response.Success)
                    {
                        CarpoolingPageViewModel.Requests = new ObservableCollection<CarpoolRequest>(response.Result);
                        rideList.IsVisible = false;
                        requestList.IsVisible = true;
                        

                    }

                });
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