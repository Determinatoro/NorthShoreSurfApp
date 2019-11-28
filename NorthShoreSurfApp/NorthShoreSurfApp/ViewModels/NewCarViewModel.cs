using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp.ViewModels
{
    public class NewCarViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private string licensePlate;
        private string car_Color;

        public string LicensePlate
        {
            get { return licensePlate; }
            set
            {
                if (licensePlate != value)
                {
                    licensePlate = value;
                    OnPropertyChanged(nameof(LicensePlate));
                }
            }
        }
        public string Color
        {
            get { return car_Color; }
            set
            {
                if (car_Color != value)
                {
                    car_Color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

    }
}
