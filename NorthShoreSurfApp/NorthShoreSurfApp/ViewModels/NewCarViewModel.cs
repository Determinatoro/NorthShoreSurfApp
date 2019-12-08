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
    public class NewCarViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;
        private string licensePlate;
        private string color;
        private ICommand createCommand;
        private ICommand cancelCommand;

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
        public ICommand CreateCommand
        {
            get => createCommand;
            set
            {
                createCommand = value;
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
