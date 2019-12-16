using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.UIComponents;
using NorthShoreSurfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CarpoolConfirmationPage : ContentPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public CarpoolConfirmationViewModel CarpoolConfirmationViewModel { get => (CarpoolConfirmationViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CarpoolConfirmationPage()
        {
            // Hide default navigation bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            // Initialize the page
            InitializeComponent();
            // iOS safe area insets
            ((Grid)Content).SetIOSSafeAreaInsets(this);
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

            GetCarpoolConfirmationItems(false);

            navigationBar.BackButtonClicked += (sender, args) =>
            {
                PopPage();
            };
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

        private void GetCarpoolConfirmationItems(bool showDialog = true)
        {
            int userId = AppValuesService.UserId.Value;

            // Get user data from database
            App.DataService.GetData(
                    NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                    showDialog, 
                    () => App.DataService.GetConfirmations(userId),
                    (response) =>
                    {
                        // Check response
                        if (response.Success)
                        {
                            // Get result object
                            var confirmationsResult = response.Result;
                            // Create new list
                            var list = new List<CarpoolConfirmationItem>();
                            if (confirmationsResult.OwnRides.Count > 0)
                            {
                                // First header (Own rides)
                                list.Add(new CarpoolConfirmationItem() { Title = NorthShoreSurfApp.Resources.AppResources.own_rides });
                                // Own rides
                                foreach (var item in confirmationsResult.OwnRides)
                                {
                                    // Set current user id
                                    foreach (var carpoolConfirmation in item.CarpoolConfirmations)
                                        carpoolConfirmation.CurrentUserId = userId;
                                    
                                    // Add confirmation items
                                    list.Add(new CarpoolConfirmationItem()
                                    {
                                        UserId = userId,
                                        OwnRide = true,
                                        CarpoolRide = item,
                                        AcceptConfirmationCommand = new Command(CarpoolConfirmationAccepted),
                                        DenyConfirmationCommand = new Command(CarpoolConfirmationDenied)
                                    });
                                }
                            }
                            if (confirmationsResult.OtherRides.Count > 0)
                            {
                                // Second header (Other rides)
                                list.Add(new CarpoolConfirmationItem() { Title = NorthShoreSurfApp.Resources.AppResources.other_rides });
                                // Other rides
                                foreach (var item in confirmationsResult.OtherRides)
                                {
                                    // Set current user id
                                    foreach (var carpoolConfirmation in item.CarpoolConfirmations)
                                        carpoolConfirmation.CurrentUserId = userId;
                                    
                                    // Add confirmation items
                                    list.Add(new CarpoolConfirmationItem()
                                    {
                                        UserId = userId,
                                        OtherRide = true,
                                        CarpoolRide = item,
                                        AcceptConfirmationCommand = new Command(CarpoolConfirmationAccepted),
                                        DenyConfirmationCommand = new Command(CarpoolConfirmationDenied)
                                    });
                                }
                            }

                            // Set list
                            CarpoolConfirmationViewModel.ItemsSource = list;
                        }
                        else
                        {
                            //Show error
                            this.ShowMessage(response.ErrorMessage);
                        }
                    });
        }

        private void AnswerCarpoolConfirmation(CarpoolConfirmation carpoolConfirmation, bool accept)
        {
            // Get user data from database
            App.DataService.GetData(
                    NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                    true,
                    () => App.DataService.AnswerCarpoolConfirmation(AppValuesService.UserId.Value, carpoolConfirmation.Id, accept),
                    (response) =>
                    {
                        // Check response
                        if (response.Success)
                        {
                            GetCarpoolConfirmationItems(false);
                        }
                        else
                        {
                            //Show error
                            this.ShowMessage(response.ErrorMessage);
                        }
                    });
        }

        #endregion

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        /// <summary>
        /// Carpool confirmation accepted
        /// </summary>
        /// <param name="func">Function to get the selected carpool confirmation</param>
        private void CarpoolConfirmationAccepted(object func)
        {
            var confirmation = ((Func<CarpoolConfirmation>)func).Invoke();
            AnswerCarpoolConfirmation(confirmation, true);
        }
        /// <summary>
        /// Carpool confirmation denied
        /// </summary>
        /// <param name="func">Function to get the selected carpool confirmation</param>
        private void CarpoolConfirmationDenied(object func)
        {
            var confirmation = ((Func<CarpoolConfirmation>)func).Invoke();
            AnswerCarpoolConfirmation(confirmation, false);
        }

        #endregion
    }
}