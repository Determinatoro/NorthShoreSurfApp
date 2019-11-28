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
        private string city;
        private string addressZip;
        private string destinationAddress;
        private string destinationCity;
        private string destinationZip;
        private string zipAndCity;
        private string zipAndCityDestination;
        private int numberOfSeats;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CarpoolDetailsPageViewModel()
        {
            carpoolRide = new CarpoolRide
            {
                Driver = new User
                {
                    FirstName = "Thomas",
                    LastName = "Schjødte",
                    Gender = new Gender { Name = Resources.AppResources.male },
                    Age = 20,
                    PhoneNo = "22278273"
                },
                Address = "Godsbanen",
                DestinationAddress = "Strand",
                PricePerPassenger = 50,
                ZipCode = "9000",
                DestinationZipCode = "9480",
                City = "Aalborg",
                DestinationCity = "Løkken"
                
            };



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

        public string DepatureTime
        {
            get { return carpoolRide.DepartureTimeString; }
            set
            {
                if(carpoolRide.DepartureTimeString != value)
                {
                    depatureTime = value;
                    OnPropertyChanged(nameof(DepatureTime));
                }
            }
        }

        public string Price
        {
            get { return carpoolRide.PricePerPassenger.ToString() + " DKK"; }
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

        public string AddressZip
        {
            get { return carpoolRide.ZipCode; }
            set
            {
                if (addressZip != value)
                {
                    addressZip = value;
                    OnPropertyChanged(nameof(AddressZip));
                }
            }
        }

        public string DestinationZip
        {
            get { return carpoolRide.DestinationZipCode; }
            set
            {
                if (destinationZip != value)
                {
                    destinationZip = value;
                    OnPropertyChanged(nameof(DestinationZip));
                }
            }
        }


        public string City
        {
            get { return carpoolRide.City; }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        public string DestinationCity
        {
            get { return "Godsbanen"; }
            set
            {
                if (destinationCity != value)
                {
                    destinationCity = value;
                    OnPropertyChanged(nameof(DestinationCity));
                }
            }
        }

        public string ZipAndCity
        {
            get { return "9000 Aalborg"; }
            set
            {
                if ($"{carpoolRide.ZipCode} {carpoolRide.City}" != value)
                {
                    zipAndCity = value;
                    OnPropertyChanged(nameof(ZipAndCity));
                }
            }
        }

        public string ZipAndCityDestination
        {
            get { return $"{carpoolRide.DestinationZipCode} {carpoolRide.DestinationCity}"; }
            set
            {
                if ($"{carpoolRide.DestinationZipCode} {carpoolRide.DestinationCity}" != value)
                {
                    zipAndCityDestination = value;
                    OnPropertyChanged(nameof(ZipAndCityDestination));
                }
            }
        }

    }
}
