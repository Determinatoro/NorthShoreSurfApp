using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NorthShoreSurfApp.ViewModels
{
    /*****************************************************************/
    // ENUMS
    /*****************************************************************/
    #region Enums

    public enum NewCarPageType
    {
        Add,
        Edit
    }

    #endregion

    public class NewCarViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;
        private string licensePlate;
        private string color;
        private ICommand buttonCommand;
        private ICommand cancelCommand;
        private Car car;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Contructor

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// Page type
        /// </summary>
        public NewCarPageType PageType
        {
            get
            {
                if (Car == null)
                    return NewCarPageType.Add;
                else
                    return NewCarPageType.Edit;
            }
        }
        /// <summary>
        /// Car object
        /// </summary>
        public Car Car
        {
            get => car;
            set
            {
                car = value;

                if (value != null)
                {
                    LicensePlate = Car.LicensePlate;
                    Color = Car.Color;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(NavigationBarTitle));
                OnPropertyChanged(nameof(ButtonIcon));
                OnPropertyChanged(nameof(ButtonTitle));
            }
        }
        /// <summary>
        /// Navigation bar title
        /// </summary>
        public string NavigationBarTitle
        {
            get
            {
                switch (PageType)
                {
                    case NewCarPageType.Add:
                        return Resources.AppResources.add_new_car;
                    case NewCarPageType.Edit:
                        return Resources.AppResources.edit_car;
                }

                return string.Empty;
            }
        }
        /// <summary>
        /// Icon for the button
        /// </summary>
        public ImageSource ButtonIcon
        {
            get
            {
                if (Car == null)
                    return ImageSource.FromFile("ic_car.png");
                else
                    return ImageSource.FromFile("ic_edit.png");
            }
        }
        // Title for the button
        public string ButtonTitle
        {
            get
            {
                if (Car == null)
                    return Resources.AppResources.add_new_car;
                else
                    return Resources.AppResources.edit_car;
            }
        }
        /// <summary>
        /// Flag for all data given
        /// </summary>
        public bool AllDataGiven
        {
            get
            {
                return LicensePlate != null && LicensePlate.Length >= 2 && LicensePlate.Length <= 7 &&
                    Color != null && Color != string.Empty;
            }
        }
        /// <summary>
        /// The car's license plate
        /// </summary>
        public string LicensePlate
        {
            get { return licensePlate; }
            set
            {
                if (licensePlate != value)
                {
                    licensePlate = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// The car's color
        /// </summary>
        public string Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Create command
        /// </summary>
        public ICommand ButtonCommand
        {
            get => buttonCommand;
            set
            {
                buttonCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Cancel command
        /// </summary>
        public ICommand CancelCommand
        {
            get => cancelCommand;
            set
            {
                cancelCommand = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
