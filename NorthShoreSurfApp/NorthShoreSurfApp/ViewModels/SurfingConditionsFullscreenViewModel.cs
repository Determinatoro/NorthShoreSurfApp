using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp.ViewModels
{
    public class SurfingConditionsFullscreenViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private WebViewSource webViewSource;
        private string videoURL;
        private LibVLC libVLC;
        private MediaPlayer mediaPlayer;
        private SurfingConditionsFullscreenPageType pageType;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public SurfingConditionsFullscreenViewModel()
        {

        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initialize LibVLC and playback when page appears
        /// </summary>
        public void OnAppearing()
        {
            try
            {
                // Initialize VLC library
                Core.Initialize();
                // Create new instance
                LibVLC = new LibVLC();

                if (VideoURL != null)
                {
                    // Create media to run
                    var media = new Media(LibVLC,
                        VideoURL,
                        FromType.FromLocation);
                    // Setup media player
                    MediaPlayer = new MediaPlayer(LibVLC);
                    // Play media
                    MediaPlayer.Play(media);
                }
            }
            catch { }
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// Flag for showing webview
        /// </summary>
        public bool ShowWebViewPage
        {
            get => PageType == SurfingConditionsFullscreenPageType.WebView;
        }
        /// <summary>
        /// Flag for showing video page
        /// </summary>
        public bool ShowVideoPage
        {
            get => PageType == SurfingConditionsFullscreenPageType.Video;
        }
        /// <summary>
        /// Type for the page
        /// </summary>
        public SurfingConditionsFullscreenPageType PageType
        {
            get => pageType;
            set
            {
                pageType = value;
                OnPropertyChanged(nameof(ShowWebViewPage));
                OnPropertyChanged(nameof(ShowVideoPage));
            }
        }
        /// <summary>
        /// Source for webview to show
        /// </summary>
        public WebViewSource WebViewSource
        {
            get { return webViewSource; }
            set
            {
                webViewSource = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.LibVLC"/> instance.
        /// </summary>
        public LibVLC LibVLC
        {
            get { return libVLC; }
            private set
            {
                libVLC = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.MediaPlayer"/> instance.
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get => mediaPlayer;
            private set
            {
                mediaPlayer = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// URL for the stream to be shown in the VLC player
        /// </summary>
        public string VideoURL
        {
            get { return videoURL; }
            set
            {
                videoURL = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
