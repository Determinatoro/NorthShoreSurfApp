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
using GooglePlaces;

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

            navigationBar.BackButtonClicked += NavigationBar_BackButtonClicked;
            createEventButton.Clicked += CreateEvent_ButtonClicked;
            RidesTab.Toggled += RidesTab_Clicked;


            search_bar.ApiKey = "AIzaSyC1Vdt6UwkRZtBlfQppMa6HHGWbwpqCgRY";
            search_bar.Type = PlaceType.Address;
            search_bar.Components = new Components("country:dk");
            search_bar.PlacesRetrieved += Search_Bar_PlacesRetrieved;
            search_bar.TextChanged += Search_Bar_TextChanged;
            search_bar.MinimumSearchText = 2;
            search_bar.Language = GoogleAPILanguage.Danish;
            results_list.ItemSelected += Results_List_ItemSelected;


        }

        private void NavigationBar_BackButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
                Navigation.RemovePage(this);
        }

        private void CreateEvent_ButtonClicked(object sender, EventArgs e)
        {
            if(sender == createEventButton)
            {
                // App.DataService.GetData(NorthShoreSurfApp.Resources.AppResources.could_not_create_carpool_event, false, () => App.DataService.CreateCarpoolRide(1,DatePicker.Date, addressTextBox.Text);
                
            }
        }

        private void RidesTab_Clicked(object sender, EventArgs e)
        {
            if (RidesTab.SelectedToggleType == ToggleType.Right)
            {
                newRide.IsVisible = false;
                newRequest.IsVisible = true;
                
            }
            else if (RidesTab.SelectedToggleType == ToggleType.Left)
            {
                newRide.IsVisible = true;
                newRequest.IsVisible = false;
                
            }
        }

        void Search_Bar_PlacesRetrieved(object sender, AutoCompleteResult result)
        {
            results_list.ItemsSource = result.AutoCompletePlaces;
           

            if (result.AutoCompletePlaces != null && result.AutoCompletePlaces.Count > 0)
                results_list.IsVisible = true;
        }

        void Search_Bar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                results_list.IsVisible = false;
                
            }
            else
            {
                results_list.IsVisible = true;
                
            }
        }

        async void Results_List_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var prediction = (AutoCompletePrediction)e.SelectedItem;
            results_list.SelectedItem = null;

            var place = await Places.GetPlace(prediction.Place_ID, search_bar.ApiKey);

            if (place != null)
            {
                search_bar.Text = string.Format("{0} {1}, {2} {3}", place.PostalCode,place.Locality,place.StreetName, place.StreetNumber, "OK");
                results_list.IsVisible = false;
                /* NewCarpoolingPageViewModel.NewRide.DestinationZipCode = place.PostalCode.ToString();
                NewCarpoolingPageViewModel.NewRide.City = place.Locality.ToString();
                NewCarpoolingPageViewModel.NewRide.DestinationAddress = string.Format("{0} {1}", place.StreetName.ToString(), place.StreetNumber.ToString()); */
                
            }
                
        }





    }
}