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
            InitializeComponent();
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

            GetCarpoolConfirmationItems();

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
            int userId = AppValuesService.GetUserId().Value;

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
                            var confirmationsResult = response.Result;

                            var list = new List<CarpoolConfirmationItem>();
                            list.Add(new CarpoolConfirmationItem() { Title = NorthShoreSurfApp.Resources.AppResources.own_rides });
                            foreach (var item in confirmationsResult.OwnRides)
                            {
                                // Set current user id
                                foreach(var carpoolConfirmation in item.CarpoolConfirmations)
                                    carpoolConfirmation.CurrentUserId = userId;
                                // Order the confirmations
                                // 1. Confirmed
                                // 2. Invites
                                // 3. Join requests
                                item.CarpoolConfirmations = item.CarpoolConfirmations
                                                                .OrderByDescending(x => x.IsConfirmed)
                                                                .ThenByDescending(x => x.IsInvite)
                                                                .ToList();
                                // Add confirmation items
                                list.Add(new CarpoolConfirmationItem() 
                                {
                                    CarpoolRide = item, 
                                    IsOwnRide = true, 
                                    AcceptConfirmationCommand = new Command(CarpoolConfirmationAccepted),
                                    DenyConfirmationCommand = new Command(CarpoolConfirmationDenied)
                                });
                            }
                            list.Add(new CarpoolConfirmationItem() { Title = NorthShoreSurfApp.Resources.AppResources.other_rides });

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

        private void CarpoolConfirmationAccepted(object func)
        {
            var confirmation = ((Func<CarpoolConfirmation>)func).Invoke();
            AnswerCarpoolConfirmation(confirmation, true);
        }

        private void CarpoolConfirmationDenied(object func)
        {
            var confirmation = ((Func<CarpoolConfirmation>)func).Invoke();
            AnswerCarpoolConfirmation(confirmation, false);
        }

        #endregion
    }
}