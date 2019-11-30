using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NorthShoreSurfApp.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string fullName;
        private string phoneNo;
        private string age;
        private string gender;

        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public UserViewModel()
        {
            
        }

        public string PhoneNo
        {
            get { return phoneNo; }
            set
            {
                phoneNo = value;
                OnPropertyChanged(nameof(PhoneNo));
            }
        }

        public string Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
    }
}