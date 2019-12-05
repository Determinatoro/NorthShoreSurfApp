using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NorthShoreSurfApp.ViewModels
{
    public class SurfingConditionsViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand seeWebcamCommand;
        private ObservableCollection<CarpoolRide> carpoolRides;

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

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// URL for the video stream
        /// </summary>
        public static string VideoUrl { get => "rtsp://127.0.0.1:8080/video/h264";}
        /// <summary>
        /// URL for the ocean info
        /// </summary>
        public WebViewSource OceanInfoUrl { get => "https://servlet.dmi.dk/byvejr/servlet/byvejr?by=9021&tabel=dag1&param=bolger"; }
        /// <summary>
        /// URL for the weather info
        /// </summary>
        public WebViewSource WeatherInfoUrl { get => "https://servlet.dmi.dk/byvejr/servlet/byvejr_dag1?by=9021&tabel=dag1&mode=long"; }
        /// <summary>
        /// Command for "See webcam"
        /// </summary>
        public ICommand SeeWebcamCommand
        {
            get => seeWebcamCommand;
            set
            {
                seeWebcamCommand = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
