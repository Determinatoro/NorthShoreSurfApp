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

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public HomeDetailsViewModel()
        {

        }

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

        /// <summary>
        /// Get information for the page
        /// </summary>
        public void GetInformation()
        {
            switch (HomeDetailsPageType)
            {
                case HomeDetailsPageType.OpeningHours:
                    {
                        // Get opening hours information
                        App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                            true,
                            () => App.DataService.GetOpeningHoursInformation(),
                            async (response) =>
                            {
                                if (response.Success)
                                {
                                    // Set opening hours information
                                    OpeningHoursDetails = response.Result;
                                }
                                else
                                {
                                    // Show error
                                    CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                    await PopupNavigation.Instance.PushAsync(customDialog);
                                }
                            });

                        break;
                    }
                case HomeDetailsPageType.ContactInfo:
                    {
                        // Get contact info from the database
                        App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                            true,
                            () => App.DataService.GetContactInfo(),
                            async (response) =>
                            {
                                if (response.Success)
                                {
                                    // Set contact info object
                                    ContactInfo = response.Result;
                                }
                                else
                                {
                                    // Show error
                                    CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                    await PopupNavigation.Instance.PushAsync(customDialog);
                                }
                            });

                        break;
                    }
            }
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        private HomeDetailsPageType homeDetailsPageType;
        public HomeDetailsPageType HomeDetailsPageType
        {
            get => homeDetailsPageType;
            set
            {
                homeDetailsPageType = value;

                switch (value)
                {
                    case HomeDetailsPageType.OpeningHours:
                        PageTitle = Resources.AppResources.opening_hours;
                        break;
                    case HomeDetailsPageType.ContactInfo:
                        PageTitle = Resources.AppResources.contact_us;
                        break;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowOpeningHoursDetails));
                OnPropertyChanged(nameof(ShowContactInfoDetails));
            }
        }
        private string pageTitle;
        public string PageTitle
        {
            get => pageTitle;
            set
            {
                pageTitle = value;
                OnPropertyChanged();
            }
        }
        private string openingHoursDetails;
        public string OpeningHoursDetails
        {
            get => openingHoursDetails;
            set
            {
                openingHoursDetails = value;
                OnPropertyChanged();
            }
        }
        private string contactInfoDetails;
        public string ContactInfoDetails
        {
            get => contactInfoDetails;
            set
            {
                contactInfoDetails = value;
                OnPropertyChanged();
            }
        }
        private ContactInfo contactInfo;
        public ContactInfo ContactInfo
        {
            get => contactInfo;
            set
            {
                contactInfo = value;
                ContactInfoDetails = $"North Shore Surf Løkken\n\n{contactInfo.Email}\n{contactInfo.PhoneNo}\n\n{contactInfo.Address}\n{contactInfo.ZipCode} {contactInfo.City}";
            }
        }
        public bool ShowOpeningHoursDetails
        {
            get => HomeDetailsPageType == HomeDetailsPageType.OpeningHours;
        }
        public bool ShowContactInfoDetails
        {
            get => HomeDetailsPageType == HomeDetailsPageType.ContactInfo;
        }

        #endregion
    }
}
