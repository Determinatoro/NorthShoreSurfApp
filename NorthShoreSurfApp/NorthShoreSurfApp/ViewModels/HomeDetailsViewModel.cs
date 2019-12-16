using NorthShoreSurfApp.ModelComponents;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace NorthShoreSurfApp.ViewModels
{
    /*****************************************************************/
    // ENUMS
    /*****************************************************************/
    #region Enums

    public enum HomeDetailsPageType
    {
        OpeningHours,
        ContactInfo
    }

    #endregion

    public class HomeDetailsViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private HomeDetailsPageType homeDetailsPageType;
        private string openingHoursDetails;
        private ContactInfo contactInfo;

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Property changed default method
        /// </summary>
        /// <param name="propertyName">Name for the property that will be notified about</param>
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
        /// Page type deciding the content
        /// </summary>
        public HomeDetailsPageType PageType
        {
            get => homeDetailsPageType;
            set
            {
                homeDetailsPageType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowOpeningHoursDetails));
                OnPropertyChanged(nameof(ShowContactInfoDetails));
                OnPropertyChanged(nameof(PageTitle));
            }
        }
        /// <summary>
        /// Title for the page used in the navigation bar
        /// </summary>
        public string PageTitle
        {
            get  
            {
                switch (PageType)
                {
                    case HomeDetailsPageType.OpeningHours:
                        return Resources.AppResources.opening_hours;
                    case HomeDetailsPageType.ContactInfo:
                        return Resources.AppResources.contact_us;
                }

                return string.Empty;
            }
        }
        /// <summary>
        /// Opening hours details as a string
        /// </summary>
        public string OpeningHoursDetails
        {
            get => openingHoursDetails;
            set
            {
                openingHoursDetails = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Contact info details as a string
        /// </summary>
        public string ContactInfoDetails
        {
            get
            {
                if (ContactInfo == null)
                    return string.Empty;

                return $"North Shore Surf Løkken\n\n{ContactInfo.Email}\n{ContactInfo.PhoneNo}\n\n{ContactInfo.Address}\n{ContactInfo.ZipCode} {ContactInfo.City}";
            }
        }
        /// <summary>
        /// Contact info object
        /// </summary>
        public ContactInfo ContactInfo
        {
            get => contactInfo;
            set
            {
                contactInfo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ContactInfoDetails));
            }
        }
        /// <summary>
        /// Flag for showing the opening hours
        /// </summary>
        public bool ShowOpeningHoursDetails
        {
            get => PageType == HomeDetailsPageType.OpeningHours;
        }
        /// <summary>
        /// Flag for showing the contact info
        /// </summary>
        public bool ShowContactInfoDetails
        {
            get => PageType == HomeDetailsPageType.ContactInfo;
        }

        #endregion
    }
}
