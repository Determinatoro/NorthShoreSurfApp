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

        public void GetInformation()
        {
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                false,
                async () =>
                {
                    var response = await App.DataService.GetTodaysOpeningHours();
                    var response2 = await App.DataService.GetNextCarpoolRide();
                    return new Tuple<DataResponse<string>, DataResponse<CarpoolRide>>(response, response2);
                },
                async (response) =>
                {
                    if (response.Item1.Success)
                    {
                        OpeningHoursContent = response.Item1.Result;
                    }
                    else
                    {
                        // Show error
                        await PopupNavigation.Instance.PushAsync(
                            new CustomDialog(CustomDialogType.Message, response.Item1.ErrorMessage)
                            );
                    }

                    if (response.Item1.Success)
                    {
                        var nextCarpoolRide = response.Item2.Result;

                        var nextCarpoolRideContent = NorthShoreSurfApp.Resources.AppResources.none;
                        if (nextCarpoolRide != null)
                        {
                            var difference = (nextCarpoolRide.DepartureTime - DateTime.Now);
                            if (difference.TotalDays >= 1)
                                nextCarpoolRideContent = string.Format(NorthShoreSurfApp.Resources.AppResources.days, ((int)difference.TotalDays).ToString());
                            else if (difference.TotalHours >= 1)
                                nextCarpoolRideContent = string.Format(NorthShoreSurfApp.Resources.AppResources.hours, ((int)difference.TotalHours).ToString());
                            else if (difference.TotalMinutes >= 1)
                                nextCarpoolRideContent = string.Format(NorthShoreSurfApp.Resources.AppResources.minutes, ((int)difference.TotalMinutes).ToString());
                            else
                                nextCarpoolRideContent = string.Format(NorthShoreSurfApp.Resources.AppResources.seconds, ((int)difference.TotalSeconds).ToString());
                        }

                        NextRideInContent = nextCarpoolRideContent;
                    }
                    else
                    {
                        // Show error
                        await PopupNavigation.Instance.PushAsync(
                            new CustomDialog(CustomDialogType.Message, response.Item1.ErrorMessage)
                            );
                    }
                });
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

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
