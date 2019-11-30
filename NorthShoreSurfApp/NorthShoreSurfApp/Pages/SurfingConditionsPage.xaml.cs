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
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurfingConditionsPage : ContentPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        private SurfingConditionsViewModel SurfingConditionsViewModel { get => (SurfingConditionsViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public SurfingConditionsPage()
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

            // Reload pressed
            navigationBar.ButtonOne.Clicked += (sender, args) =>
            {
                try
                {
                    wvOceanInfo.Reload();
                    wvWeatherInfo.Reload();
                    
                    SetWeatherForecastHeight();
                    SetOceanForecastHeight();
                }
                catch
                {
                    // Ignore exception
                }
            };

            // Size changed event for weather info webview
            wvWeatherInfo.SizeChanged += (sender, args) =>
            {
                SetWeatherForecastHeight();
            };
            // Size changed event for ocean info webview
            wvOceanInfo.SizeChanged += (sender, args) =>
            {
                SetOceanForecastHeight();
            };

            // Tapped on weather and ocean forecasts
            TapGestureRecognizer gridTap = new TapGestureRecognizer();
            gridTap.Tapped += async (sender, e) =>
            {
                // Show weather info in full screen
                if (sender == gridWeatherInfo)
                    await Navigation.PushModalAsync(new SurfingConditionsFullscreenPage(SurfingConditionsViewModel.WeatherInfoUrl), false);
                // Show ocean info in full screen
                else if (sender == gridOceanInfo)
                    await Navigation.PushModalAsync(new SurfingConditionsFullscreenPage(SurfingConditionsViewModel.OceanInfoUrl), false);
            };
            gridWeatherInfo.GestureRecognizers.Add(gridTap);
            gridOceanInfo.GestureRecognizers.Add(gridTap);
            
            // Pressed "See webcam" button
            btnSeeWebcam.Clicked += async (sender, args) =>
            {
                await Navigation.PushModalAsync(new SurfingConditionsFullscreenPage("rtsp://192.168.10.112:8080/video/h264"), false);
            };
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        private void SetOceanForecastHeight()
        {
            // Set width/height ratio to wrap the ocean info from DMI
            wvOceanInfo.HeightRequest = wvOceanInfo.Width * 0.3984375;
        }

        private void SetWeatherForecastHeight()
        {
            // Set width/height ratio to wrap the weather info from DMI
            wvWeatherInfo.HeightRequest = wvWeatherInfo.Width * 0.5625;
        }

        #endregion

        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Navigation.ModalStack.Count == 0)
            {
                // Orientation
                CrossDeviceOrientation.Current.LockOrientation(Plugin.DeviceOrientation.Abstractions.DeviceOrientations.Portrait);
                // Show status bar on android
                App.ScreenService.ShowStatusBar();
            }
        }

        #endregion
    }
}