using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;

namespace NorthShoreSurfApp.ViewModels.CarpoolingPage
{
    public class CarpoolingPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CarpoolingPageViewModel()
        {
            rides = new ObservableCollection<CarpoolRide>();
            requests = new ObservableCollection<CarpoolRequest>();


            

            
        }


        /* private ObservableCollection<CarpoolRide> rides;


        public ObservableCollection<CarpoolRide> Rides
        {
            get { return rides; }
            set
            {
                rides = value;
                OnPropertyChanged(nameof(rides));
            }
        } */

        private ObservableCollection<CarpoolRide> rides;

        public ObservableCollection<CarpoolRide> Rides
        {
            get { return rides; }
            set
            {
                rides = value;
                OnPropertyChanged(nameof(rides));
            }
        }

        private ObservableCollection<CarpoolRequest> requests;

        public ObservableCollection<CarpoolRequest> Requests
        {
            get { return requests; }
            set
            {
                requests = value;
                OnPropertyChanged(nameof(requests));
            }
        }


    }
}
