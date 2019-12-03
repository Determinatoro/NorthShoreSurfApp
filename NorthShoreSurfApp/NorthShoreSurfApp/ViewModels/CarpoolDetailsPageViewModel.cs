using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        private string zipCode;
        private string destinationZipCode;
        private string depatureTime;
        private string depatureTimeDay;
        private string price;
        private string address;
        private string destinationAddress;
        private int numberOfSeats;
        private int availableSeats;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CarpoolDetailsPageViewModel()
        {
            
        }


        public string Name
        {
            get { return fullName; }
            set
            {
                if (fullName != value)
                {
                    fullName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ZipCode
        {
            get { return zipCode; }
            set
            {
                if (zipCode != value)
                {
                    zipCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DestinationZipCode
        {
            get { return destinationZipCode; }
            set
            {
                if (destinationZipCode != value)
                {
                    destinationZipCode
                        
              = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PhoneNo
        {
            get { return phoneNo; }
            set
            {
                if (phoneNo != value)
                {
                    phoneNo = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Age
        {
            get { return age; }
            set
            {
                if (age != value)
                {
                    age = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Gender
        {
            get { return gender; }
            set
            {
                if (gender != value)
                {
                    gender = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DepartureTimeDay
        {
            get { return depatureTimeDay; }
            set
            {
                if(depatureTimeDay != value)
                {
                    depatureTimeDay = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DepartureTime
        {
            get { return depatureTime; }
            set
            {
                if (depatureTime != value)
                {
                    depatureTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged();
                }
            }
        }

        public int NumberOfSeats
        {
            get { return numberOfSeats; }
            set
            {
                if (numberOfSeats != value)
                {
                    numberOfSeats = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DestinationAddress
        {
            get { return destinationAddress; }
            set
            {
                if (destinationAddress != value)
                {
                    destinationAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AvailableSeats
        {
            get { return availableSeats; }
            set
            {
                if (availableSeats != value)
                {
                    availableSeats = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
