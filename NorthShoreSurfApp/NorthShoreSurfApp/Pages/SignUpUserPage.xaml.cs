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

        public SignUpUserModel SignUpUserModel { get => (SignUpUserModel)this.BindingContext; }

        private ContentSite CurrentContentSite { get; set; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public SignUpUserPage()
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
            App.OrientationService.Portrait();
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
                SetCurrentContentSite(ContentSite.EnterSMSCode, true);
                return;

                if (SignUpUserModel.AllDataGiven)
                {
                    await App.FirebaseService.VerifyPhoneNo(this, SignUpUserModel.PhoneNo);
                    SetCurrentContentSite(ContentSite.EnterSMSCode, true);
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, "Please enter data"));
                }

                return;
                var page = new SignUpUserPage();
                page.SignUpUserModel.LastName = "TEST";
                await Navigation.PushAsync(page);
                return;

                App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                        true,
                        () => App.DataService.CreateCar(1, "AM60657", "Midnight Blue"),
                        async (response) =>
                        {
                            if (response.Success)
                            {
                                await Navigation.PopAsync();
                            } 
                            else
                            {
                                CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                await PopupNavigation.Instance.PushAsync(customDialog);
                            }
                            
                            await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, response.Result.LicensePlate));
                        });
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
        public async void SignedIn()
        {
            await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, "Signed in"));
        }

        #endregion
    }
}
