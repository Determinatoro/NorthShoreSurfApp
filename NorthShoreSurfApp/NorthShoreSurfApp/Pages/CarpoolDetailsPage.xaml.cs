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

        public CarpoolDetailsPageViewModel carpoolDetailsPageViewModel { get => (CarpoolDetailsPageViewModel)this.BindingContext; }


        public CarpoolDetailsPage()
        {
            InitializeComponent();
        }

        public CarpoolDetailsPage(CarpoolRide ride)
        {
            CarpoolRide carpoolRide = ride;
            carpoolDetailsPageViewModel.carpoolRide = carpoolRide;
        }

    }
}