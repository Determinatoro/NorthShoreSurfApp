using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;

namespace NorthShoreSurfApp.ViewModels
{
    public class CarpoolConfirmationPageViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<CarpoolConfirmation> requests;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CarpoolConfirmationPageViewModel()
        {
            requests = new ObservableCollection<CarpoolConfirmation>();

        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties


        public ObservableCollection<CarpoolConfirmation> Requests
        {
            get { return requests; }
            set
            {
                requests = value;
                OnPropertyChanged(nameof(Requests));
            }
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
