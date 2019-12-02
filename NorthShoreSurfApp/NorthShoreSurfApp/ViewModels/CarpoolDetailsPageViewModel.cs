using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;

namespace NorthShoreSurfApp.ViewModels
{
    public class CarpoolDetailsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CarpoolRide carpoolRide;
        private string fullName;
        private string phoneNo;
        private int age;
        private string gender;

        private string depatureTime;
        private string price;
        private string address;
        private string destinationAddress;
        private int numberOfSeats;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CarpoolDetailsPageViewModel()
        {
            
        }

        public string GetName
        {
            get { return carpoolRide.Driver.FullName(); }
            set
            {
                if (carpoolRide.Driver.FullName() != value)
                {
                    fullName = value;
                    OnPropertyChanged(nameof(GetName));
                }
            }
        }

        public string PhoneNo
        {
            get { return carpoolRide.Driver.PhoneNo; }
            set
            {
                if (carpoolRide.Driver.PhoneNo != value)
                {
                    phoneNo = value;
                    OnPropertyChanged(nameof(PhoneNo));
                }
            }
        }

        public int Age
        {
            get { return carpoolRide.Driver.Age; }
            set
            {
                if (carpoolRide.Driver.Age != value)
                {
                    age = value;
                    OnPropertyChanged(nameof(PhoneNo));
                }
            }
        }

        public string Gender
        {
            get { return carpoolRide.Driver.Gender.Name; }
            set
            {
                if (carpoolRide.Driver.Gender.Name != value)
                {
                    gender = value;
                    OnPropertyChanged(nameof(Gender));
                }
            }
        }

        public string DepartureTime
        {
            get { return carpoolRide.DepartureTimeDayString; }
            set
            {
                if(carpoolRide.DepartureTimeDayString != value)
                {
                    depatureTime = value;
                    OnPropertyChanged(nameof(DepartureTime));
                }
            }
        }

        public string Price
        {
            get { return carpoolRide.PricePerPassenger.ToString(); }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public string Address
        {
            get { return carpoolRide.Address; }
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public int NumberOfSeats
        {
            get { return carpoolRide.NumberOfSeats; }
            set
            {
                if (numberOfSeats != value)
                {
                    numberOfSeats = value;
                    OnPropertyChanged(nameof(NumberOfSeats));
                }
            }
        }

        public string DestinationAddress
        {
            get { return carpoolRide.DestinationAddress; }
            set
            {
                if (destinationAddress != value)
                {
                    destinationAddress = value;
                    OnPropertyChanged(nameof(DestinationAddress));
                }
            }
        }


    }
}
