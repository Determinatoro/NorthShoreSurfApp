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
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public CarpoolDetailsPageViewModel CarpoolDetailsPageViewModel { get => (CarpoolDetailsPageViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CarpoolDetailsPage(CarpoolRide ride) : this()
        {
            CarpoolDetailsPageViewModel.CarpoolRide = ride;
            CarpoolDetailsPageViewModel.ButtonCommand = new Command(() =>
            {
                JoinRide();
            });
            GetInformation();
        }

        public CarpoolDetailsPage(CarpoolRequest request) : this()
        {
            CarpoolDetailsPageViewModel.CarpoolRequest = request;
            CarpoolDetailsPageViewModel.ButtonCommand = new Command(() =>
            {
                InvitePassenger();
            });
            GetInformation();
        }

        private CarpoolDetailsPage()
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
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Pop this page
        /// </summary>
        private async void PopPage()
        {
            await Navigation.PopModalAsync();
        }
        /// <summary>
        /// Get information for the page
        /// </summary>
        private void GetInformation()
        {
            int userId = AppValuesService.UserId.Value;

            // Get information
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                            false,
                            async () =>
                            {
                                // User information
                                var getUserResponse = await App.DataService.GetUser(userId);
                                // The user's confirmations
                                var getConfirmationsResponse = await App.DataService.GetCarpoolConfirmations(userId);
                                // The user's carpool rides
                                var getOwnCarpoolRidesResponse = await App.DataService.GetOwnCarpoolRides(userId);
                                return new Tuple<DataResponse<User>, DataResponse<List<CarpoolConfirmation>>, DataResponse<List<CarpoolRide>>>(
                                    getUserResponse,
                                    getConfirmationsResponse,
                                    getOwnCarpoolRidesResponse
                                );
                            },
                            (response) =>
                            {
                                if (response.Item1.Success && response.Item2.Success && response.Item3.Success)
                                {
                                    CarpoolDetailsPageViewModel.User = response.Item1.Result;
                                    CarpoolDetailsPageViewModel.CarpoolConfirmations = response.Item2.Result;
                                    CarpoolDetailsPageViewModel.CarpoolRides = response.Item3.Result;
                                }
                                else if (!response.Item1.Success)
                                {
                                    this.ShowMessage(response.Item1.ErrorMessage);
                                }
                                else if (!response.Item2.Success)
                                {
                                    this.ShowMessage(response.Item2.ErrorMessage);
                                }
                                else if (!response.Item3.Success)
                                {
                                    this.ShowMessage(response.Item3.ErrorMessage);
                                }
                            });
        }
        /// <summary>
        /// Join ride
        /// </summary>
        private void JoinRide()
        {
            var userId = int.Parse(App.LocalDataService.GetValue(nameof(LocalDataKeys.UserId)));

            // Join carpool ride
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.SignUpToCarpoolRide(CarpoolDetailsPageViewModel.CarpoolRide.Id, userId),
                            (response) =>
                            {
                                if (response.Success)
                                {
                                    CarpoolDetailsPageViewModel.AddCarpoolConfirmation(response.Result);
                                    this.ShowMessage(
                                            NorthShoreSurfApp.Resources.AppResources.you_have_requested_to_join_the_carpool,
                                            NorthShoreSurfApp.Resources.AppResources.ok
                                        );
                                }
                                else
                                {
                                    this.ShowMessage(response.ErrorMessage);
                                }
                            });
        }
        /// <summary>
        /// Invite passenger
        /// </summary>
        private async void InvitePassenger()
        {
            int? userId = AppValuesService.UserId;

            // Select carpool ride
            // Show list selection
            CustomListDialog customListDialog = new CustomListDialog(
                CarpoolDetailsPageViewModel.CarpoolRideListItemTemplate,
                CarpoolDetailsPageViewModel.CarpoolRidesList,
                string.Format(NorthShoreSurfApp.Resources.AppResources.select_parameter, NorthShoreSurfApp.Resources.AppResources.ride.ToLower())
                );
            customListDialog.ListSeparatorVisibility = SeparatorVisibility.None;
            // List item tapped
            customListDialog.ItemTapped += (sender, args) =>
            {
                // Selected carpool ride
                CarpoolRide carpoolRide = (CarpoolRide)args.Item;

                // Invite passenger
                App.DataService.GetData(
                                NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                                true,
                                () => App.DataService.InvitePassenger(CarpoolDetailsPageViewModel.CarpoolRequest.Id, carpoolRide.Id),
                                (response) =>
                                {
                                    if (response.Success)
                                    {
                                        GetInformation();
                                        this.ShowMessage(
                                                NorthShoreSurfApp.Resources.AppResources.you_have_invited_a_passenger_to_your_carpool_ride,
                                                NorthShoreSurfApp.Resources.AppResources.ok
                                            );
                                    }
                                    else
                                    {
                                        this.ShowMessage(response.ErrorMessage);
                                    }
                                });
            };
            await PopupNavigation.Instance.PushAsync(customListDialog);
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
        }
        // OnBackButtonPressed
        protected override bool OnBackButtonPressed()
        {
            PopPage();
            return true;
        }

        #endregion


    }
}