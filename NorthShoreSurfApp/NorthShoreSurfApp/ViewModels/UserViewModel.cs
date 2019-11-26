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

        private string firstName = "Emil";
        private string lastName = "Danielsen";
        private string phoneNo = "29711907";
        private string age = "21";
        private string gender = "Male";

        public string FirstName
        {
            get { return firstName; }
        }

        public string LastName
        {
            get { return lastName; }
        }

        public string PhoneNo
        {
            get { return phoneNo; }
        }

        public string Age
        {
            get { return age; }
        }

        public string Gender
        {
            get { return gender; }
        }
    }
}