using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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

            rideList.ItemSelected += Ride_Clicked;
        }


        private void Ride_Clicked(object sender, EventArgs e)
        {
            if (sender == rideList)
            {
                var model = CarpoolingPageViewModel;
                
          
            }
        }


    }
}