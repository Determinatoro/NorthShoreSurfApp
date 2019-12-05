using LibVLCSharp.Shared;
using NorthShoreSurfApp.ViewModels;
using Plugin.DeviceOrientation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    /*****************************************************************/
    // ENUMS
    /*****************************************************************/
    #region Enums

    public enum SurfingConditionsFullscreenPageType
    {
        WebView,
        Video
    }

    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurfingConditionsFullscreenPage : ContentPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        private SurfingConditionsFullscreenViewModel SurfingConditionsFullscreenViewModel { get => (SurfingConditionsFullscreenViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public SurfingConditionsFullscreenPage(WebViewSource webViewSource) : this()
        {
            SurfingConditionsFullscreenViewModel.PageType = SurfingConditionsFullscreenPageType.WebView;
            SurfingConditionsFullscreenViewModel.WebViewSource = webViewSource;
        }

        public SurfingConditionsFullscreenPage(string webcamUrl) : this()
        {
            SurfingConditionsFullscreenViewModel.PageType = SurfingConditionsFullscreenPageType.Video;
            SurfingConditionsFullscreenViewModel.VideoURL = webcamUrl;
        }

        private SurfingConditionsFullscreenPage()
        {
            // Remove navigation native bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            // Initialize
            InitializeComponent();
            // Use safe area on iOS
            ((Grid)Content).SetIOSSafeAreaInsets(this);
            // Orientation for the page
            CrossDeviceOrientation.Current.UnlockOrientation();
            // Hide status bar on android
            App.ScreenService.HideStatusBar();

            // Enable zoom on android
            webView.On<Android>().EnableZoomControls(true);

            // Icon close button
            btnClose.Clicked += (sender, args) =>
            {
                PopPage();
            };

            // App resumed
            App.Resumed += (sender, args) =>
            {
                OnAppearing();
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
            // Orientation for the page
            CrossDeviceOrientation.Current.UnlockOrientation();
            // Hide status bar on android
            App.ScreenService.HideStatusBar();

            SurfingConditionsFullscreenViewModel.OnAppearing();
        }

        // OnDisappearing
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Orientation for the page
            CrossDeviceOrientation.Current.LockOrientation(Plugin.DeviceOrientation.Abstractions.DeviceOrientations.Portrait);
            // Hide status bar on android
            App.ScreenService.ShowStatusBar();
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

        // Pop this page
        private async void PopPage()
        {
            await Navigation.PopModalAsync(false);
        }

        #endregion
    }
}