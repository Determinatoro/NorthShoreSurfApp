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

        private ObservableCollection<Object> ride;

        private ObservableCollection<User> user;

        private string message;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CarpoolDetailsPageViewModel()
        {
            ride = new ObservableCollection<object>();
            user = new ObservableCollection<User>();
        }

        public ObservableCollection<Object> Ride
        {
            get { return ride; }
            set
            {
                ride = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<User> User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        public string  Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public DataTemplate CarpoolRideItemTemplate
        {
            get { return new DataTemplate(() => new CarpoolRideViewCell().View); }
        }


        public DataTemplate UserItemTemplate
        {
            get { return new DataTemplate(() => new UserViewCell().View); }
        }


    }
}
