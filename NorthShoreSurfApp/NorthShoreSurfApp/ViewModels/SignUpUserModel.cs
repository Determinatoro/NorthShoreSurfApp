using NorthShoreSurfApp.ModelComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace NorthShoreSurfApp.ViewModels
{
    public class SignUpUserModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string firstName;
        private string lastName;
        private string phoneNo;
        private string age;
        private int genderId;

        public SignUpUserModel()
        {
            
        }

        public string[] Genders
        {
            get
            {
                return new string[] { Resources.AppResources.male, Resources.AppResources.female, Resources.AppResources.other };
            }
        }
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged(nameof(LastName));
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
                    OnPropertyChanged(nameof(PhoneNo));
                }
            }
        }
        public int GenderId
        {
            get { return genderId; }
            set
            {
                if (genderId != value)
                {
                    genderId = value;
                    OnPropertyChanged(nameof(GenderId));
                }
            }
        }
        public string Age
        {
            get { return age; }
            set
            {
                if (age != value)
                {
                    age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }
    }
}
