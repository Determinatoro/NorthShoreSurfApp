using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;
using NorthShoreSurfApp.ViewCells;
using System.Runtime.CompilerServices;

namespace NorthShoreSurfApp.ViewModels
{
    public class CarpoolingPageViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<CarpoolRide> rides;
        private ObservableCollection<CarpoolRequest> requests;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CarpoolingPageViewModel()
        {
            rides = new ObservableCollection<CarpoolRide>();
            requests = new ObservableCollection<CarpoolRequest>();
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

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public ObservableCollection<CarpoolRide> Rides
        {
            get { return rides; }
            set
            {
                rides = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CarpoolRequest> Requests
        {
            get { return requests; }
            set
            {
                requests = value;
                OnPropertyChanged();
            }
        }

        public DataTemplate CarpoolRideItemTemplate
        {
            get { return new DataTemplate(() => new CarpoolRideViewCell()); }
        }

        public DataTemplate CarpoolRequestItemTemplate
        {
            get { return new DataTemplate(() => new CarpoolRequestViewCell()); }
        }

        #endregion
    }
}
