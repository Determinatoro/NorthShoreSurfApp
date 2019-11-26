using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;

namespace NorthShoreSurfApp.ViewModels
{


    public class NewCarpoolingPageViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NewCarpoolingPageViewModel()
        {

        }

        private CarpoolRide newRide;

        public CarpoolRide NewRide
        {
            get
            {
                return newRide;
            }
            set
            {
                newRide = value;
                OnPropertyChanged(nameof(newRide));
            }
        }

        private CarpoolRequest carpoolRequest;

        public CarpoolRequest CarpoolRequest
        {
            get
            {
                return carpoolRequest;
            }
            set
            {
                carpoolRequest = value;
                OnPropertyChanged(nameof(carpoolRequest));
            }
        }



    }
}


