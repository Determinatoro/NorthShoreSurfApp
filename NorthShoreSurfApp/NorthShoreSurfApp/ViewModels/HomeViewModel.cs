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

        private string openingHoursContent;
        private string nextRideInContent;
        private CarpoolRide nextCarpoolRide;

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
        /// Object for the next carpool ride content
        /// </summary>
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
                        nextCarpoolRideContent = string.Format(Resources.AppResources.days, ((int)difference.TotalDays).ToString());
                    // Hours
                    else if (difference.TotalHours >= 1)
                        nextCarpoolRideContent = string.Format(Resources.AppResources.hours, ((int)difference.TotalHours).ToString());
                    // Minutes
                    else if (difference.TotalMinutes >= 1)
                        nextCarpoolRideContent = string.Format(Resources.AppResources.minutes, ((int)difference.TotalMinutes).ToString());
                    // Seconds
                    else
                        nextCarpoolRideContent = string.Format(Resources.AppResources.seconds, ((int)difference.TotalSeconds).ToString());
                }

                NextRideInContent = nextCarpoolRideContent;
            }
        }
        /// <summary>
        /// Content is how long there is to the next departure time for a carpool ride
        /// </summary>
        public string NextRideInContent
        {
            get { return nextRideInContent; }
            set
            {
                nextRideInContent = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Content is opening hours for the current day
        /// </summary>
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
