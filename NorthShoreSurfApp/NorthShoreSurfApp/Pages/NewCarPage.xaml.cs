using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using NorthShoreSurfApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCarPage : ContentPage
    {

        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public NewCarModel NewCarModel { get => (NewCarModel)this.BindingContext; }

        //private ContentSite CurrentContentSite { get; set; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public NewCarPage()
        {
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

            // Click events
            btnNext.Clicked += Button_Clicked;

            // Back button click event
            /*navigationBar.BackButtonClicked += (sender, args) =>
            {
                if (Navigation.NavigationStack.Count > 1)
                    Navigation.RemovePage(this);
                else
                    SetCurrentContentSite(ContentSite.EnterData, true);
            };*/
            navigationBar.ButtonOneIsVisible = true;
            navigationBar.ButtonTwoIsVisible = true;

            // Set current content site
            //SetCurrentContentSite(ContentSite.EnterData, false);
        }

        #endregion

        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        // OnAppearing
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.OrientationService.Portrait();
        }

        #endregion

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        // Buttons clicked event
        private async void Button_Clicked(object sender, EventArgs e)
        {
            int UserId = 1; //int.Parse(App.LocalDataService.GetValue(nameof(LocalDataKeys.UserId)));

            await App.DataService.CreateCar(UserId, NewCarModel.LicensePlate, NewCarModel.Color);
            Console.WriteLine(NewCarModel.Color);
        }

        #endregion
    }
}