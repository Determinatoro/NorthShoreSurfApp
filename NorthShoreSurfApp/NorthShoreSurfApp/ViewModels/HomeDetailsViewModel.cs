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
        private string pageTitle;
        private string openingHoursDetails;
        private string contactInfoDetails;
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

        /// <summary>
        /// Page type deciding the content
        /// </summary>
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
        /// <summary>
        /// Title for the page used in the navigation bar
        /// </summary>
        public string PageTitle
        {
            get => pageTitle;
            set
            {
                pageTitle = value;
                OnPropertyChanged();
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
            get => contactInfoDetails;
            set
            {
                contactInfoDetails = value;
                OnPropertyChanged();
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
                ContactInfoDetails = $"North Shore Surf Løkken\n\n{contactInfo.Email}\n{contactInfo.PhoneNo}\n\n{contactInfo.Address}\n{contactInfo.ZipCode} {contactInfo.City}";
            }
        }
        /// <summary>
        /// Flag for showing the opening hours
        /// </summary>
        public bool ShowOpeningHoursDetails
        {
            get => HomeDetailsPageType == HomeDetailsPageType.OpeningHours;
        }
        /// <summary>
        /// Flag for showing the contact info
        /// </summary>
        public bool ShowContactInfoDetails
        {
            get => HomeDetailsPageType == HomeDetailsPageType.ContactInfo;
        }

        #endregion
    }
}
