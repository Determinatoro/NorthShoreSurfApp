using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewModels;
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
    public partial class HomeDetailsPage : ContentPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        private HomeDetailsViewModel HomeDetailsViewModel { get => (HomeDetailsViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public HomeDetailsPage(HomeDetailsPageType homeDetailsPageType) : this()
        {
            HomeDetailsViewModel.PageType = homeDetailsPageType;
        }

        private HomeDetailsPage()
        {
            // Hide default navigation bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            // Initialize the page
            InitializeComponent();
            // iOS safe area insets
            ((Grid)Content).SetIOSSafeAreaInsets(this);

            // Back button clicked
            navigationBar.BackButtonClicked += (sender, args) =>
            {
                PopPage();
            };
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
            // Get information for the page
            GetInformation();
        }

        // OnBackButtonPressed
        protected override bool OnBackButtonPressed()
        {
            PopPage();
            return true;
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Pop the current modal page
        /// </summary>
        private async void PopPage()
        {
            while (Navigation.ModalStack.Count > 0 && Navigation.ModalStack.Contains(this))
                await Navigation.PopModalAsync();
        }

        /// <summary>
        /// Get information for the page
        /// </summary>
        public void GetInformation()
        {
            switch (HomeDetailsViewModel.PageType)
            {
                case HomeDetailsPageType.OpeningHours:
                    {
                        // Get opening hours information
                        App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                            true,
                            () => App.DataService.GetOpeningHoursInformation(),
                            async (response) =>
                            {
                                if (response.Success)
                                {
                                    // Set opening hours information
                                    HomeDetailsViewModel.OpeningHoursDetails = response.Result;
                                }
                                else
                                {
                                    // Show error
                                    CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                    await PopupNavigation.Instance.PushAsync(customDialog);
                                }
                            });

                        break;
                    }
                case HomeDetailsPageType.ContactInfo:
                    {
                        // Get contact info from the database
                        App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                            true,
                            () => App.DataService.GetContactInfo(),
                            async (response) =>
                            {
                                if (response.Success)
                                {
                                    // Set contact info object
                                    HomeDetailsViewModel.ContactInfo = response.Result;
                                }
                                else
                                {
                                    // Show error
                                    CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                    await PopupNavigation.Instance.PushAsync(customDialog);
                                }
                            });

                        break;
                    }
            }
        }

        #endregion
    }
}