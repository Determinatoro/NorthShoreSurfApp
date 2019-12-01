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
                Core.Initialize();

                LibVLC = new LibVLC();

                if (VideoURL != null)
                {
                    var media = new Media(LibVLC,
                        VideoURL,
                        FromType.FromLocation);

                    MediaPlayer = new MediaPlayer(LibVLC);
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

        private WebViewSource webViewSource;
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
        private LibVLC libVLC;
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
        private MediaPlayer mediaPlayer;
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
        private string videoURL;
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
