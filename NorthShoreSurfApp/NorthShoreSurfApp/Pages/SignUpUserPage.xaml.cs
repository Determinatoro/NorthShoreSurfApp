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
    public partial class SignUpUserPage : ContentPage
    {
        /*****************************************************************/
        // ENUMS
        /*****************************************************************/
        #region Enums

        #endregion

        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public SignUpUserViewModel SignUpUserViewModel { get => (SignUpUserViewModel)this.BindingContext; }

        private SignUpUserPageContentSite CurrentContentSite { get; set; }

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
            On<iOS>().SetUseSafeArea(true);
            // Get root grid
            Grid grid = (Grid)Content;
            // Get safe area margins
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            // Set safe area margins
            grid.Margin = safeAreaInset;

            // Set page type
            SignUpUserViewModel.SetPageType(signUpUserPageType, existingUser);

            // Back button click event
            navigationBar.BackButtonClicked += (sender, args) =>
            {
                if (CurrentContentSite == SignUpUserPageContentSite.EnterSMSCode)
                    SetCurrentContentSite(SignUpUserPageContentSite.EnterData);
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

            // Set current content site
            SetCurrentContentSite(SignUpUserViewModel.PageType == SignUpUserPageType.EditInformation ? SignUpUserPageContentSite.EditInformation : SignUpUserPageContentSite.EnterData);
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
        public void SetCurrentContentSite(SignUpUserPageContentSite contentSite)
        {
            CurrentContentSite = contentSite;

            gridEnterData.IsVisible = false;
            gridEnterSMSCode.IsVisible = false;

            switch (SignUpUserViewModel.PageType)
            {
                case SignUpUserPageType.Login:
                    tbAge.IsVisible = false;
                    tbFirstName.IsVisible = false;
                    tbLastName.IsVisible = false;
                    pickerGender.IsVisible = false;

                    tbPhoneNo.Margin = new Thickness(0);
                    break;
            }

            switch (CurrentContentSite)
            {
                case SignUpUserPageContentSite.EnterData:
                case SignUpUserPageContentSite.EditInformation:
                    gridEnterData.IsVisible = true;
                    break;
                case SignUpUserPageContentSite.EnterSMSCode:
                    gridEnterSMSCode.IsVisible = true;
                    break;
            }

            switch (CurrentContentSite)
            {
                case SignUpUserPageContentSite.EditInformation:
                    btnNext.Icon = ImageSource.FromFile("ic_check.png");
                    btnNext.Title = NorthShoreSurfApp.Resources.AppResources.approve;
                    break;
                default:
                    btnNext.Icon = ImageSource.FromFile("ic_forward.png");
                    btnNext.Title = NorthShoreSurfApp.Resources.AppResources.next;
                    break;
            }
        }

        #endregion
    }
}
