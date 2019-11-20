using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;
using NorthShoreSurfApp.ModelComponents;

namespace NorthShoreSurfApp.ViewModels.CarpoolingPage
{
    public class CarpoolingPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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


    }
}
