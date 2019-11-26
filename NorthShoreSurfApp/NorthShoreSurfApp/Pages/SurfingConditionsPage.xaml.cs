using LibVLCSharp.Shared;
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
        #region Variables
        private LibVLC LibVLC { get; set; }
        
        private const string CamOnLiveVideoUrl = "rtsp://127.0.0.1:8080/video/h264";
        //private const string IpWebcamVideoUrl = "rtsp://192.168.0.101:8080/h264_pcm.sdp";

        #endregion

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
            
            wvWeatherInfo.Source = "https://servlet.dmi.dk/byvejr/servlet/byvejr_dag1?by=9021&tabel=dag1&mode=long";
        }

        #endregion

        #region Override methods

        protected override void OnAppearing()   
        {
            base.OnAppearing();

            vvWebcam.MediaPlayer = new MediaPlayer(LibVLC);
            vvWebcam.MediaPlayer.Fullscreen = true;
            vvWebcam.MediaPlayer.Play(new Media(LibVLC, CamOnLiveVideoUrl, FromType.FromLocation));

            wvWeatherInfo.SizeChanged += (sender, args) =>
            {
                var webview = ((WebView)sender);
                var width = webview.Width;

                webview.HeightRequest = width * 0.5625;
            };

            
        }

        #endregion
    }
}