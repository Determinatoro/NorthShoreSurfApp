using NorthShoreSurfApp.UIComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp.ViewModels
{
    public class CarpoolConfirmationViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private List<CarpoolConfirmationItem> itemsSource;

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

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
        /// User's gender
        /// </summary>
        public List<CarpoolConfirmationItem> ItemsSource
        {
            get { return itemsSource; }
            set
            {
                itemsSource = value;
                OnPropertyChanged();
            }
        }

        public DataTemplate CarpoolConfirmationItemItemTemplate
        {
            get { return new DataTemplate(() => new CarpoolConfirmationItemViewCell()); }
        }

        #endregion
    }
}
