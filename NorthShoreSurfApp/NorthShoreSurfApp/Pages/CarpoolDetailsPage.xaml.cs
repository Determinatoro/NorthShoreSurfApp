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

        public CarpoolDetailsViewModel CarpoolDetailsPageViewModel { get => (CarpoolDetailsViewModel)this.BindingContext; }

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
                if (CarpoolDetailsPageViewModel.PageType == CarpoolDetailsPageType.CarpoolRide_Leave)
                    LeaveRide();
                else
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

            // Back button command
            CarpoolDetailsPageViewModel.BackCommand = new Command(() =>
            {
                PopPage();
            });
            // Edit command
            CarpoolDetailsPageViewModel.EditCommand = new Command(async () =>
            {
                NewCarpoolingPage newCarpoolingPage = null;
                switch (CarpoolDetailsPageViewModel.PageObject)
                {
                    case CarpoolDetailsPageObject.CarpoolRide:
                        newCarpoolingPage = new NewCarpoolingPage(CarpoolDetailsPageViewModel.CarpoolRide);
                        newCarpoolingPage.CarpoolRideUpdated = (carpoolRide) =>
                        {
                            CarpoolDetailsPageViewModel.CarpoolRide = carpoolRide;
                        };
                        break;
                    case CarpoolDetailsPageObject.CarpoolRequest:
                        newCarpoolingPage = new NewCarpoolingPage(CarpoolDetailsPageViewModel.CarpoolRequest);
                        newCarpoolingPage.CarpoolRequestUpdated = (carpoolRequest) =>
                        {
                            CarpoolDetailsPageViewModel.CarpoolRequest = carpoolRequest;
                        };
                        break;
                }
                await Navigation.PushModalAsync(newCarpoolingPage);
            });
            // Delete command
            CarpoolDetailsPageViewModel.DeleteCommand = new Command(() =>
            {
                this.ShowYesNo(string.Format(NorthShoreSurfApp.Resources.AppResources.are_you_sure_you_want_to_delete_the, CarpoolDetailsPageViewModel.NavigationBarTitle.ToLower()), 
                    () =>
                    {
                        switch (CarpoolDetailsPageViewModel.PageObject)
                        {
                            case CarpoolDetailsPageObject.CarpoolRide:
                                DeleteCarpoolRide();
                                break;
                            case CarpoolDetailsPageObject.CarpoolRequest:
                                DeleteCarpoolRequest();
                                break;
                        }
                    });
            });
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
        /// Delete carpool ride
        /// </summary>
        private void DeleteCarpoolRide()
        {
            var userId = AppValuesService.UserId.Value;

            // Delete carpool ride
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.DeleteCarpoolRide(CarpoolDetailsPageViewModel.CarpoolRide.Id),
                            (response) =>
                            {
                                if (response.Success)
                                {
                                    // Pop the page because the object has been deleted
                                    PopPage();
                                }
                                else
                                {
                                    this.ShowMessage(response.ErrorMessage);
                                }
                            });
        }
        /// <summary>
        /// Delete carpool request
        /// </summary>
        private void DeleteCarpoolRequest()
        {
            var userId = AppValuesService.UserId.Value;

            // Delete carpool request
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.DeleteCarpoolRequest(CarpoolDetailsPageViewModel.CarpoolRequest.Id),
                            (response) =>
                            {
                                if (response.Success)
                                {
                                    // Pop the page because the object has been deleted
                                    PopPage();
                                }
                                else
                                {
                                    this.ShowMessage(response.ErrorMessage);
                                }
                            });
        }
        /// <summary>
        /// Leave ride
        /// </summary>
        private void LeaveRide()
        {
            var userId = AppValuesService.UserId.Value;

            // Join carpool ride
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.UnsignFromCarpoolRide(CarpoolDetailsPageViewModel.CarpoolRide.Id, userId),
                            (response) =>
                            {
                                if (response.Success)
                                {
                                    CarpoolDetailsPageViewModel.RemoveCarpoolConfirmations(response.Result);
                                    this.ShowMessage(
                                            NorthShoreSurfApp.Resources.AppResources.you_have_left_the_carpool_ride,
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
        /// Join ride
        /// </summary>
        private void JoinRide()
        {
            var userId = AppValuesService.UserId.Value;

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