using LibVLCSharp.Shared;
using NorthShoreSurfApp.ViewModels;
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

        private LibVLC LibVLC { get; set; }
        
        private const string CamOnLiveVideoUrl = "rtsp://127.0.0.1:8080/video/h264";
        //private const string IpWebcamVideoUrl = "rtsp://192.168.0.101:8080/h264_pcm.sdp";

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

            // VLC setup
            LibVLC = new LibVLC();

            navigationBar.ButtonOne.Clicked += (sender, args) =>
            {
                wvOceanInfo.Reload();
                wvWeatherInfo.Reload();
            };

            wvOceanInfo.Focused += (s, e) =>
            {
                DisplayAlert("TEST", "TEST", "Cancel");
                wvOceanInfo.Unfocus();
            };

            wvWeatherInfo.Loaded += (sender, args) =>
            {
                var webview = ((WebView)sender);
                // Set width/height ratio to wrap the weather info from DMI
                webview.HeightRequest = webview.Width * 0.5625;
            };
            wvOceanInfo.SizeChanged += (sender, args) =>
            {
                var webview = ((WebView)sender);
                // Set width/height ratio to wrap the ocean info from DMI
                webview.HeightRequest = webview.Width * 0.3984375;
            };
        }

        #endregion

        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        protected override void OnAppearing()   
        {
            base.OnAppearing();

            //vvWebcam.MediaPlayer = new MediaPlayer(LibVLC);
            //vvWebcam.MediaPlayer.Fullscreen = true;
            //vvWebcam.MediaPlayer.Play(new Media(LibVLC, CamOnLiveVideoUrl, FromType.FromLocation));
            var width = wvWeatherInfo.Width;
            
        }

        #endregion
    }
}