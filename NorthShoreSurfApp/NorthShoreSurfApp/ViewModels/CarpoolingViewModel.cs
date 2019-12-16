using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;
using NorthShoreSurfApp.ViewCells;
using System.Runtime.CompilerServices;

namespace NorthShoreSurfApp.ViewModels
{
    /*****************************************************************/
    // DATA TEMPLATE SELECTOR
    /*****************************************************************/
    #region DataTemplateSelector

    public class CarpoolRideDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DividerItemTemplate { get; set; }
        public DataTemplate CarpoolRideItemTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var carpoolRide = item as CarpoolRide;
            if (carpoolRide == null)
                return null;

            if (carpoolRide.IsDivider)
                return DividerItemTemplate;
            else
                return CarpoolRideItemTemplate;
        }
    }

    public class CarpoolRequestDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DividerItemTemplate { get; set; }
        public DataTemplate CarpoolRequestItemTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var carpoolRequest = item as CarpoolRequest;
            if (carpoolRequest == null)
                return null;

            if (carpoolRequest.IsDivider)
                return DividerItemTemplate;
            else
                return CarpoolRequestItemTemplate;
        }
    }

    #endregion

    public class CarpoolingViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private List<CarpoolRide> rides;
        private List<CarpoolRequest> requests;
        private List<CarpoolConfirmation> carpoolConfirmations;
        private ToggleType toggleType;

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
        /// Toggle type for the page
        /// </summary>
        public ToggleType ToggleType
        {
            get => toggleType;
            set
            {
                toggleType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowRides));
                OnPropertyChanged(nameof(ShowRequests));
            }
        }
        /// <summary>
        /// Flag deciding whether the rides should be shown
        /// </summary>
        public bool ShowRides
        {
            get => toggleType == ToggleType.Left;
        }
        /// <summary>
        /// Flag deciding whether the requests should be shown
        /// </summary>
        public bool ShowRequests
        {
            get => toggleType == ToggleType.Right;
        }
        /// <summary>
        /// Rides list
        /// </summary>
        public List<CarpoolRide> Rides
        {
            get { return rides; }
            set
            {
                rides = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Requests list
        /// </summary>
        public List<CarpoolRequest> Requests
        {
            get { return requests; }
            set
            {
                requests = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Confirmations list
        /// </summary>
        public List<CarpoolConfirmation> CarpoolConfirmations
        {
            get { return carpoolConfirmations; }
            set
            {
                carpoolConfirmations = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NavigationBarButtonTwoIcon));
            }
        }
        /// <summary>
        /// Navigation bar button two icon
        /// </summary>
        public ImageSource NavigationBarButtonTwoIcon
        {
            get
            {
                if (CarpoolConfirmations != null && CarpoolConfirmations.Count > 0)
                    return ImageSource.FromFile("ic_message_pending.png");
                else
                    return ImageSource.FromFile("ic_message.png");
            }
        }
        /// <summary>
        /// Item template for a carpool ride
        /// </summary>
        public DataTemplate CarpoolRideItemTemplate
        {
            get { return new DataTemplate(() => new CarpoolRideViewCell()); }
        }
        /// <summary>
        /// Item template for a divider
        /// </summary>
        public DataTemplate CarpoolRequestDividerItemTemplate
        {
            get { return new DataTemplate(() => new DividerViewCell()); }
        }
        /// <summary>
        /// Item template for a divider
        /// </summary>
        public DataTemplate CarpoolRideDividerItemTemplate
        {
            get { return new DataTemplate(() => new DividerViewCell()); }
        }
        /// <summary>
        /// Item template for a carpool request
        /// </summary>
        public DataTemplate CarpoolRequestItemTemplate
        {
            get { return new DataTemplate(() => new CarpoolRequestViewCell()); }
        }
        /// <summary>
        /// Item template selector for the carpool list
        /// </summary>
        public CarpoolRideDataTemplateSelector CarpoolRideDataTemplateSelector
        {
            get
            {
                CarpoolRideDataTemplateSelector carpoolRideDataTemplateSelector = new CarpoolRideDataTemplateSelector();
                carpoolRideDataTemplateSelector.CarpoolRideItemTemplate = CarpoolRideItemTemplate;
                carpoolRideDataTemplateSelector.DividerItemTemplate = CarpoolRideDividerItemTemplate;
                return carpoolRideDataTemplateSelector;
            }
        }
        /// <summary>
        /// Item template selector for the request list
        /// </summary>
        public CarpoolRequestDataTemplateSelector CarpoolRequestDataTemplateSelector
        {
            get
            {
                CarpoolRequestDataTemplateSelector carpoolRequestDataTemplateSelector = new CarpoolRequestDataTemplateSelector();
                carpoolRequestDataTemplateSelector.CarpoolRequestItemTemplate = CarpoolRequestItemTemplate;
                carpoolRequestDataTemplateSelector.DividerItemTemplate = CarpoolRequestDividerItemTemplate;
                return carpoolRequestDataTemplateSelector;
            }
        }

        #endregion
    }
}
