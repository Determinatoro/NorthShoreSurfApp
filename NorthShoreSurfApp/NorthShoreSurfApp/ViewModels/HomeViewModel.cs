using NorthShoreSurfApp.ModelComponents;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace NorthShoreSurfApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
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

        public HomeViewModel()
        {

        }

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

        private CarpoolRide nextCarpoolRide;
        public CarpoolRide NextCarpoolRide
        {
            get => nextCarpoolRide; 
            set
            {
                nextCarpoolRide = value;

                // No next carpool ride
                var nextCarpoolRideContent = NorthShoreSurfApp.Resources.AppResources.none;
                if (nextCarpoolRide != null)
                {
                    var difference = (nextCarpoolRide.DepartureTime - DateTime.Now);
                    // Days
                    if (difference.TotalDays >= 1)
                        nextCarpoolRideContent = string.Format(NorthShoreSurfApp.Resources.AppResources.days, ((int)difference.TotalDays).ToString());
                    // Hours
                    else if (difference.TotalHours >= 1)
                        nextCarpoolRideContent = string.Format(NorthShoreSurfApp.Resources.AppResources.hours, ((int)difference.TotalHours).ToString());
                    // Minutes
                    else if (difference.TotalMinutes >= 1)
                        nextCarpoolRideContent = string.Format(NorthShoreSurfApp.Resources.AppResources.minutes, ((int)difference.TotalMinutes).ToString());
                    // Seconds
                    else
                        nextCarpoolRideContent = string.Format(NorthShoreSurfApp.Resources.AppResources.seconds, ((int)difference.TotalSeconds).ToString());
                }

                NextRideInContent = nextCarpoolRideContent;
            }
        }
        private string nextRideInContent;
        public string NextRideInContent
        {
            get { return nextRideInContent; }
            set
            {
                nextRideInContent = value;
                OnPropertyChanged();
            }
        }
        private string openingHoursContent;
        public string OpeningHoursContent
        {
            get { return openingHoursContent; }
            set
            {
                openingHoursContent = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
