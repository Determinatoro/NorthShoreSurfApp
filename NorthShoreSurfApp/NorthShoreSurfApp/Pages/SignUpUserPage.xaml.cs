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

    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpUserPage : ContentPage, IFirebaseServiceCallBack
    {
        /*****************************************************************/
        // ENUMS
        /*****************************************************************/
        #region Enums

        private enum ContentSite
        {
            EnterData,
            EnterSMSCode
        }

        #endregion

        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public SignUpUserViewModel SignUpUserModel { get => (SignUpUserViewModel)this.BindingContext; }

        private ContentSite CurrentContentSite { get; set; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public SignUpUserPage(SignUpUserPageType signUpUserPageType, User existingUser = null)
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

            // Set page type
            SignUpUserModel.SetPageType(signUpUserPageType, existingUser);

            // Click events
            btnNext.Clicked += Button_Clicked;
            btnApprove.Clicked += Button_Clicked;

            // Back button click event
            navigationBar.BackButtonClicked += (sender, args) =>
            {
                if (Navigation.NavigationStack.Count > 1)
                    Navigation.RemovePage(this);
                else
                    SetCurrentContentSite(ContentSite.EnterData, true);
            };
            navigationBar.ButtonOneIsVisible = true;
            navigationBar.ButtonTwoIsVisible = true;

            // List item tapped in gender picker
            pickerGender.ListItemTapped += (sender, args) =>
            {
                var gender = (Gender)args.Item;
                pickerGender.Text = gender.Name;
                SignUpUserModel.GenderId = gender.Id;
            };

            // Set current content site
            SetCurrentContentSite(ContentSite.EnterData, false);
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
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Set current content site in the page
        /// </summary>
        /// <param name="contentSite">The wanted content site</param>
        /// <param name="animate">Animate the change</param>
        private async void SetCurrentContentSite(ContentSite contentSite, bool animate = false)
        {
            CurrentContentSite = contentSite;

            gridEnterData.IsVisible = false;
            gridEnterSMSCode.IsVisible = false;

            switch (SignUpUserModel.PageType)
            {
                case SignUpUserPageType.Login:
                    tbAge.IsVisible = false;
                    tbFirstName.IsVisible = false;
                    tbLastName.IsVisible = false;
                    pickerGender.IsVisible = false;
                    break;
            }

            double displacement = gridEnterData.Width;
            double displacement2 = gridEnterSMSCode.Width;

            switch (CurrentContentSite)
            {
                case ContentSite.EnterData:
                    if (animate)
                    {
                        gridEnterSMSCode.IsVisible = true;
                        gridEnterData.IsVisible = false;

                        await Task.WhenAll(
                                    gridEnterSMSCode.FadeTo(0, 1),
                                    gridEnterSMSCode.TranslateTo(0, 0, 1),
                                    gridEnterData.FadeTo(0, 1),
                                    gridEnterData.TranslateTo(-displacement, 0, 1),
                                    gridEnterSMSCode.FadeTo(0, 250, Easing.Linear),
                                    gridEnterSMSCode.TranslateTo(displacement2, 0, 250, Easing.CubicInOut)
                                    );

                        gridEnterData.IsVisible = true;
                        gridEnterSMSCode.IsVisible = false;

                        await Task.WhenAll(
                                    gridEnterData.FadeTo(1, 250, Easing.Linear),
                                    gridEnterData.TranslateTo(0, 0, 250, Easing.CubicInOut)
                                    );
                    }
                    else
                    {
                        gridEnterData.IsVisible = true;
                    }
                    break;
                case ContentSite.EnterSMSCode:
                    if (animate)
                    {
                        gridEnterSMSCode.IsVisible = false;
                        gridEnterData.IsVisible = true;

                        await Task.WhenAll(
                                    gridEnterData.FadeTo(0, 1),
                                    gridEnterData.TranslateTo(0, 0, 1),
                                    gridEnterData.FadeTo(0, 250, Easing.Linear),
                                    gridEnterData.TranslateTo(-displacement, 0, 250, Easing.CubicInOut),
                                    gridEnterSMSCode.TranslateTo(displacement2, 0, 1),
                                    gridEnterSMSCode.FadeTo(0, 1)
                                    );

                        gridEnterData.IsVisible = false;
                        gridEnterSMSCode.IsVisible = true;

                        await Task.WhenAll(
                                    gridEnterSMSCode.FadeTo(1, 250, Easing.Linear),
                                    gridEnterSMSCode.TranslateTo(0, 0, 250, Easing.CubicInOut)
                                    );
                    }
                    else
                    {
                        gridEnterSMSCode.IsVisible = true;
                    }
                    break;
            }
        }

        #endregion

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        // Buttons clicked event
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (sender == btnNext)
            {
                if (SignUpUserModel.AllDataGiven)
                {
                    // Check if phone no. already exist
                    App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.checking_phone_no_please_wait,
                        true,
                        () => App.DataService.CheckIfPhoneIsNotUsedAlready(SignUpUserModel.PhoneNo),
                        async (response) =>
                        {
                            if (response.Success)
                            {
                                await App.FirebaseService.VerifyPhoneNo(this, SignUpUserModel.PhoneNo);
                                SetCurrentContentSite(ContentSite.EnterSMSCode);
                            }
                            else
                            {
                                CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                await PopupNavigation.Instance.PushAsync(customDialog);
                            }
                        });
                }
                else
                {
                    // Tell the user to fill out all fields on the page
                    await PopupNavigation.Instance.PushAsync(
                        new CustomDialog(
                            CustomDialogType.Message, 
                            NorthShoreSurfApp.Resources.AppResources.please_fill_out_all_the_empty_fields
                            )
                        );
                }

                return;
            }
            else if (sender == btnApprove)
            {
                App.Current.MainPage = new RootTabbedPage();
                return;

                var smsCode = SignUpUserModel.SMSCode;

                if (smsCode != null && smsCode != string.Empty && smsCode.Length == 6)
                {
                    var verId = App.LocalDataService.GetValue(nameof(LocalDataKeys.FirebaseAuthVerificationId));
                    await App.FirebaseService.SignIn(this, verId, smsCode);
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, "Please enter SMS code"));
                }
            }
        }

        #endregion

        /*****************************************************************/
        // INTERFACE METHODS
        /*****************************************************************/
        #region Interface methods

        // OnVerificationFailed
        public async void OnVerificationFailed(string errorMessage)
        {
            await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, errorMessage));
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
            switch (SignUpUserModel.PageType)
            {
                case SignUpUserPageType.SignUp:
                    {
                        App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.creating_account_please_wait,
                        true,
                        SignUpUserModel.SignUpUserTask,
                        async (response) =>
                        {
                            if (response.Success)
                            {

                            }
                            else
                            {
                                CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                await PopupNavigation.Instance.PushAsync(customDialog);
                            }
                        });

                        break;
                    }
                case SignUpUserPageType.Login:
                    {


                        break;
                    }
                case SignUpUserPageType.EditInformation:
                    break;
            }
        }

        #endregion
    }
}
