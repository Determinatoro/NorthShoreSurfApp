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
            webView.IsVisible = true;
            vvWebcam.IsVisible = false;
            SurfingConditionsFullscreenViewModel.WebViewSource = webViewSource;
        }

        public SurfingConditionsFullscreenPage(string webcamUrl) : this()
        {
            webView.IsVisible = false;
            vvWebcam.IsVisible = true;
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
            On<iOS>().SetUseSafeArea(true);
            // Get root grid
            Grid grid = (Grid)Content;
            // Get safe area margins
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            // Set safe area margins
            grid.Margin = safeAreaInset;
            // Orientation for the page
            CrossDeviceOrientation.Current.UnlockOrientation();
            // Hide status bar on android
            App.ScreenService.HideStatusBar();

            // Enable zoom on android
            webView.On<Android>().EnableZoomControls(true);

            btnClose.Clicked += (sender, args) =>
            {
                PopPage();
            };

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

        private async void PopPage()
        {
            await Navigation.PopModalAsync(false);
        }

        #endregion
    }
}