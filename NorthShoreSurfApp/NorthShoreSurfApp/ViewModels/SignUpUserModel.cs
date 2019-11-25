using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

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
        private string smsCode;
        private int genderId;

        public SignUpUserModel()
        {

        }

        public List<Gender> Genders
        {
            get
            {
                var genders = new List<Gender>();
                genders.Add(new Gender()
                {
                    Id = 1,
                    Name = NorthShoreSurfApp.Resources.AppResources.male
                });
                genders.Add(new Gender()
                {
                    Id = 2,
                    Name = NorthShoreSurfApp.Resources.AppResources.female
                });
                genders.Add(new Gender()
                {
                    Id = 3,
                    Name = NorthShoreSurfApp.Resources.AppResources.other
                });
                return genders;
            }
        }
        public bool AllDataGiven
        {
            get {
                return
                    firstName != null && firstName != string.Empty &&
                    lastName != null && lastName != string.Empty &&
                    age != null && age != string.Empty &&
                    phoneNo != null && phoneNo != string.Empty &&
                    genderId != 0;
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
        public string SMSCode
        {
            get { return smsCode; }
            set
            {
                if (smsCode != value)
                {
                    smsCode = value;
                    OnPropertyChanged(nameof(SMSCode));
                }
            }
        }
        public DataTemplate GenderPickerItemTemplate
        {
            get { return new DataTemplate(() => new GenderCustomViewCell()); }
        }
    }
}
