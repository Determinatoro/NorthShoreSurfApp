using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.Services;
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
    public partial class UserPage : ContentPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public UserViewModel UserViewModel { get => (UserViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor
        public UserPage()
        {
            // Hide default navigation bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            // Use iOS safe area insets
            ((Grid)Content).SetIOSSafeAreaInsets(this);

            // Edit command
            UserViewModel.EditCommand = new Command(async () =>
            {
                // Show user edit information page
                await Navigation.PushModalAsync(new SignUpUserPage(SignUpUserPageType.EditInformation, UserViewModel.ExistingUser));
            });
            // Delete account command
            UserViewModel.DeleteAccountCommand = new Command(() =>
            {
                this.ShowYesNo(NorthShoreSurfApp.Resources.AppResources.are_you_sure_you_want_to_delete_your_account, () =>
                {
                    // Get user data from database
                    App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.deleting_account_please_wait,
                            false, () => App.DataService.DeleteUser(AppValuesService.GetUserId().Value),
                            (response) =>
                            {
                            // Check response
                            if (response.Success)
                                {
                                // Log out
                                LogOut();
                                }
                                else
                                {
                                //Show error
                                this.ShowMessage(response.ErrorMessage);
                                }
                            });
                });
            });
            // Log out command
            UserViewModel.LogOutCommand = new Command(() =>
            {
                this.ShowYesNo(NorthShoreSurfApp.Resources.AppResources.are_you_sure_you_want_to_log_out, () =>
                {
                    // Log out
                    LogOut();
                });
            });
        }
        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Log out in the app
        /// </summary>
        private void LogOut()
        {
            // Log out of app
            AppValuesService.LogOut();
            // Sign out of Firebase
            FirebaseResponse firebaseResponse = App.FirebaseService.SignOut();
            // Check response
            if (!firebaseResponse.Success)
            {
                this.ShowMessage(firebaseResponse.ErrorMessage);
                return;
            }
            // Go to welcomepage
            App.Current.MainPage = new WelcomePage();
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

            this.DelayedTask(500, () =>
            {
                // Get user data from database
                App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                        false,
                        () => App.DataService.GetUser(AppValuesService.GetUserId().Value),
                        (response) =>
                        {
                            if (response.Success)
                            {
                            // Set user object
                            UserViewModel.ExistingUser = response.Result;
                            }
                            else
                            {
                            //Show error
                            this.ShowMessage(response.ErrorMessage);
                            }
                        });
            });
        }

        #endregion
    }
}