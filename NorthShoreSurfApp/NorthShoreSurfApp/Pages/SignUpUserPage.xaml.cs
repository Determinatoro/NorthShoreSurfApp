using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using NorthShoreSurfApp.ViewModels;
using Plugin.DeviceOrientation;
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
    /*****************************************************************/
    // ENUMS
    /*****************************************************************/
    #region Enums

    public enum SignUpUserPageType
    {
        SignUp,
        Login,
        EditInformation
    }

    public enum SignUpUserPageContentSite
    {
        EnterData,
        EnterSMSCode,
        EditInformation
    }

    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpUserPage : ContentPage, IFirebaseServiceCallBack
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public SignUpUserViewModel SignUpUserViewModel { get => (SignUpUserViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public SignUpUserPage(SignUpUserPageType signUpUserPageType, User existingUser = null)
        {
            // Hide default navigation bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            // Initialize the page
            InitializeComponent();
            // Use safe area on iOS
            ((Grid)Content).SetIOSSafeAreaInsets(this);

            // Back button click event
            navigationBar.BackButtonClicked += (sender, args) =>
            {
                if (SignUpUserViewModel.CurrentContentSite == SignUpUserPageContentSite.EnterSMSCode)
                    SignUpUserViewModel.CurrentContentSite = SignUpUserPageContentSite.EnterData;
                else
                {
                    if (Navigation.NavigationStack.Count > 1)
                        Navigation.RemovePage(this);
                    else if (Navigation.ModalStack.Count > 0)
                        Navigation.PopModalAsync();
                }
            };

            // List item tapped in gender picker
            pickerGender.ListItemTapped += (sender, args) =>
            {
                var gender = (Gender)args.Item;
                pickerGender.Text = gender.Name;
                SignUpUserViewModel.GenderId = gender.Id;
            };

            // Set values in view model
            SignUpUserViewModel.PageType = signUpUserPageType;
            SignUpUserViewModel.ExistingUser = existingUser;
            SignUpUserViewModel.CurrentContentSite = SignUpUserViewModel.PageType == SignUpUserPageType.EditInformation ? SignUpUserPageContentSite.EditInformation : SignUpUserPageContentSite.EnterData;

            if (SignUpUserViewModel.PageType == SignUpUserPageType.EditInformation)
            {
                // Event for when the phone no. changes on the edit information page
                SignUpUserViewModel.PhoneNoChanged += (sender, args) =>
                {
                    SignUpUserViewModel.CurrentContentSite =
                           SignUpUserViewModel.HasPhoneNumberChanged ?
                           SignUpUserPageContentSite.EnterData :
                           SignUpUserPageContentSite.EditInformation;
                };
            }

            // Next command
            SignUpUserViewModel.NextCommand = new Command(() =>
            {
                // Check if all data is given
                if (!SignUpUserViewModel.AllDataGiven)
                {
                    // Tell the user to fill out all fields on the page
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_fill_out_all_the_empty_fields);
                    return;
                }

                // Sign up page or user has changed phone no.
                if (SignUpUserViewModel.PageType == SignUpUserPageType.SignUp ||
                    SignUpUserViewModel.HasPhoneNumberChanged)
                {
                    // Check if phone no. already exist
                    App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.checking_phone_no_please_wait,
                        true,
                        () => App.DataService.CheckIfPhoneIsNotUsedAlready(SignUpUserViewModel.PhoneNo),
                        (response) =>
                        {
                            if (response.Success)
                            {
                                // Verify phone no. with firebase
                                FirebaseResponse firebaseResponse = App.FirebaseService.VerifyPhoneNo(this, SignUpUserViewModel.PhoneNo);
                                // Check response
                                if (firebaseResponse.Success)
                                    // Set current content site to enter sms code
                                    SignUpUserViewModel.CurrentContentSite = SignUpUserPageContentSite.EnterSMSCode;
                                else
                                    // Show error
                                    this.ShowMessage(firebaseResponse.ErrorMessage);
                            }
                            else
                            {
                                // Show error
                                this.ShowMessage(response.ErrorMessage);
                            }
                        });
                }
                // Login page
                else if (SignUpUserViewModel.PageType == SignUpUserPageType.Login)
                {
                    // Check if phone no. already exist
                    App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.checking_phone_no_please_wait,
                        true,
                        () => App.DataService.CheckLogin(SignUpUserViewModel.PhoneNo),
                        (response) =>
                        {
                            if (response.Success)
                            {
                                // Save the user
                                SignUpUserViewModel.ExistingUser = response.Result;
                                // Make the user verify phone no.
                                FirebaseResponse firebaseResponse = App.FirebaseService.VerifyPhoneNo(this, SignUpUserViewModel.PhoneNo);
                                // Check response
                                if (firebaseResponse.Success)
                                    // Set current content site to enter sms code
                                    SignUpUserViewModel.CurrentContentSite = SignUpUserPageContentSite.EnterSMSCode;
                                else
                                    // Show error
                                    this.ShowMessage(firebaseResponse.ErrorMessage);
                            }
                            else
                            {
                                // Show error
                                this.ShowMessage(response.ErrorMessage);
                            }
                        });
                }
                // User is editing their information and they have not changed their phone no.
                else if (SignUpUserViewModel.PageType == SignUpUserPageType.EditInformation)
                {
                    UpdateUser();
                }
            });
            // Approve command
            SignUpUserViewModel.ApproveCommand = new Command(() =>
            {
                var smsCode = SignUpUserViewModel.SMSCode;

                if (smsCode != null && smsCode != string.Empty && smsCode.Length == 6)
                {
                    // Get verification id
                    var verificationId = App.LocalDataService.GetValue(nameof(LocalDataKeys.FirebaseAuthVerificationId));
                    // Sign into firebase
                    FirebaseResponse firebaseResponse = App.FirebaseService.SignIn(this, verificationId, smsCode);
                    // Check response
                    if (!firebaseResponse.Success)
                        // Show error
                        this.ShowMessage(firebaseResponse.ErrorMessage);
                }
                else
                {
                    // Show error
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_enter_sms_code);
                }
            });
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Update user information
        /// </summary>
        private void UpdateUser()
        {
            // Check if phone no. already exist
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.checking_phone_no_please_wait,
                true,
                () => App.DataService.UpdateUser(
                    SignUpUserViewModel.ExistingUser.Id,
                    SignUpUserViewModel.FirstName,
                    SignUpUserViewModel.LastName,
                    SignUpUserViewModel.PhoneNo,
                    SignUpUserViewModel.AgeValue,
                    SignUpUserViewModel.GenderId
                    ),
                async (response) =>
                {
                    if (response.Success)
                    {
                        if (Navigation.NavigationStack.Count > 1)
                            await Navigation.PopAsync();
                        else if (Navigation.ModalStack.Count > 0)
                            await Navigation.PopModalAsync();
                    }
                    else
                    {
                        this.ShowMessage(response.ErrorMessage);
                    }
                });
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
            // Orientation
            CrossDeviceOrientation.Current.LockOrientation(Plugin.DeviceOrientation.Abstractions.DeviceOrientations.Portrait);
        }

        #endregion

        /*****************************************************************/
        // INTERFACE METHODS
        /*****************************************************************/
        #region Interface methods

        // OnVerificationFailed
        public void OnVerificationFailed(string errorMessage)
        {
            this.ShowMessage(errorMessage);
        }
        // OnCodeSent
        public void OnCodeSent(string verificationId)
        {

        }
        // OnCodeAutoRetrievalTimeout
        public void OnCodeAutoRetrievalTimeout(string verificationId)
        {

        }
        // SignedIn
        public void SignedIn()
        {
            switch (SignUpUserViewModel.PageType)
            {
                case SignUpUserPageType.SignUp:
                    {
                        App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.creating_account_please_wait,
                        true,
                        () => App.DataService.SignUpUser(SignUpUserViewModel.FirstName, SignUpUserViewModel.LastName, SignUpUserViewModel.PhoneNo, SignUpUserViewModel.AgeValue, SignUpUserViewModel.GenderId),
                        (response) =>
                        {
                            if (response.Success)
                            {
                                // Get newly created user
                                User user = response.Result;
                                // Save user id
                                AppValuesService.SaveValue(LocalDataKeys.UserId, user.Id.ToString());
                                // Go to home page
                                App.Current.MainPage = new RootTabbedPage();
                            }
                            else
                            {
                                // Show error
                                this.ShowMessage(response.ErrorMessage);
                            }
                        });

                        break;
                    }
                case SignUpUserPageType.Login:
                    {
                        // Go to home page
                        App.Current.MainPage = new RootTabbedPage();
                        break;
                    }
                case SignUpUserPageType.EditInformation:
                    {
                        UpdateUser();
                        break;
                    }
            }
        }

        #endregion
    }
}
