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
            rides = new ObservableCollection<CarpoolEvent>();
            rides.Add(new CarpoolEvent()
            {
                ZipCode = "8000",
                Address = "Aalborg",
                NumberOfSeats = 2,
                DepartureTime = new DateTime(2019, 1, 1,13,0,0),
                PricePerPassenger = 50

            });
            rides.Add(new CarpoolEvent()
            {
                ZipCode = "9000",
                Address = "Hjørring",
                NumberOfSeats = 5,
                DepartureTime = new DateTime(2019,1,1,14,30,0),
                PricePerPassenger = 50
            });
        }


        private ObservableCollection<CarpoolEvent> rides;


        public ObservableCollection<CarpoolEvent> Rides
        {
            get { return rides; }
            set
            {
                rides = value;
                OnPropertyChanged(nameof(rides));
            }
        }


    }
}
