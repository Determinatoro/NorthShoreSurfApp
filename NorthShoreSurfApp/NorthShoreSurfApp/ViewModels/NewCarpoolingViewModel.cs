using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;
using DurianCode.PlacesSearchBar;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NorthShoreSurfApp.ViewCells;
using System.Linq;

namespace NorthShoreSurfApp.ViewModels
{
    /*****************************************************************/
    // ENUMS
    /*****************************************************************/
    #region Enums

    public enum NewCarpoolingPageType
    {
        NewCarpoolRide,
        NewCarpoolRequest,
        EditCarpoolRide,
        EditCarpoolRequest
    }

    #endregion

    public class NewCarpoolingViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private CarpoolRide carpoolRide;
        private CarpoolRequest carpoolRequest;

        private NewCarpoolingPageType pageType;
        private string comment;
        private DateTime departureDate;
        private DateTime requestDate;
        private TimeSpan fromTime;
        private TimeSpan toTime;
        private TimeSpan departureTime;
        private ObservableCollection<Event> events;
        private ObservableCollection<Event> selectedEvents;
        private ObservableCollection<Car> cars;
        private ToggleType toggleType;
        private ICommand carDeleteCommand;
        private ICommand carEditCommand;
        private ICommand createCommand;
        private ICommand selectEventsCommand;
        private ICommand eventDeleteCommand;
        private Place pickupPlace;
        private Place destinationPlace;
        private Place requestPickupPlace;
        private Place requestDestinationPlace;
        private Car car;
        private string pricePerPassengerString;
        private string numberOfSeatsString;

        // Default destination
        private string DefaultAddress = "Sdr. Strandvej 18";
        private string DefaultZipCode = "9480";
        private string DefaultCity = "Løkken";

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public NewCarpoolingViewModel()
        {
            PageType = NewCarpoolingPageType.NewCarpoolRide;
            // Default values
            NumberOfSeats = 3;
            PricePerPassenger = 20;
            // Default departure date and time
            DepartureDate = DateTime.Now;
            DepartureTime = new TimeSpan(8, 0, 0);
            // Default request date and time interval
            RequestDate = DateTime.Now;
            FromTime = new TimeSpan(8, 0, 0);
            ToTime = new TimeSpan(12, 0, 0);
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
        /// Set default destination place
        /// </summary>
        public void SetDefaultDestinationPlace()
        {
            DestinationPlace = new Place(DefaultAddress, DefaultZipCode, DefaultCity);
            RequestDestinationPlace = new Place(DefaultAddress, DefaultZipCode, DefaultCity);
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// Title for the navigation bar
        /// </summary>
        public string NavigationBarTitle
        {
            get
            {
                switch (PageType)
                {
                    case NewCarpoolingPageType.NewCarpoolRide:
                        return string.Format(Resources.AppResources.new_this, Resources.AppResources.ride.ToLower());
                    case NewCarpoolingPageType.NewCarpoolRequest:
                        return string.Format(Resources.AppResources.new_this, Resources.AppResources.request.ToLower());
                    case NewCarpoolingPageType.EditCarpoolRide:
                        return string.Format(Resources.AppResources.edit_this, Resources.AppResources.ride.ToLower());
                    case NewCarpoolingPageType.EditCarpoolRequest:
                        return string.Format(Resources.AppResources.edit_this, Resources.AppResources.request.ToLower());
                }

                return null;
            }
        }
        /// <summary>
        /// Existing carpool ride
        /// </summary>
        public CarpoolRide CarpoolRide
        {
            get
            {
                return carpoolRide;
            }
            set
            {
                carpoolRide = value;

                if (value != null)
                {
                    Comment = CarpoolRide.Comment;
                    DepartureDate = CarpoolRide.DepartureTime;
                    DepartureTime = CarpoolRide.DepartureTime.TimeOfDay;
                    SelectedEvents = new ObservableCollection<Event>(CarpoolRide.CarpoolRides_Events_Relations.Select(x => x.Event).ToList());
                    Car = CarpoolRide.Car;
                    PricePerPassenger = CarpoolRide.PricePerPassenger;
                    NumberOfSeats = CarpoolRide.NumberOfSeats;
                    PickupPlace = new Place(CarpoolRide.Address, CarpoolRide.ZipCode, CarpoolRide.City);
                    DestinationPlace = new Place(CarpoolRide.DestinationAddress, CarpoolRide.DestinationZipCode, CarpoolRide.DestinationCity);
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNumberOfSeatsReadOnly));
            }
        }
        /// <summary>
        /// Existing carpool request
        /// </summary>
        public CarpoolRequest CarpoolRequest
        {
            get
            {
                return carpoolRequest;
            }
            set
            {
                carpoolRequest = value;

                if (value != null)
                {
                    Comment = CarpoolRequest.Comment;
                    RequestDate = CarpoolRequest.FromTime.Date;
                    FromTime = CarpoolRequest.FromTime.TimeOfDay;
                    ToTime = CarpoolRequest.ToTime.TimeOfDay;
                    SelectedEvents = new ObservableCollection<Event>(CarpoolRequest.CarpoolRequests_Events_Relations.Select(x => x.Event).ToList());
                    RequestPickupPlace = new Place(string.Empty, CarpoolRequest.ZipCode, CarpoolRequest.City);
                    RequestDestinationPlace = new Place(CarpoolRequest.DestinationAddress, CarpoolRequest.DestinationZipCode, CarpoolRequest.DestinationCity);
                }

                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Tpye of page
        /// </summary>
        public NewCarpoolingPageType PageType
        {
            get => pageType;
            set
            {
                pageType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowRequest));
                OnPropertyChanged(nameof(ShowRide));
                OnPropertyChanged(nameof(ShowToggleButton));
                OnPropertyChanged(nameof(NavigationBarTitle));
            }
        }
        /// <summary>
        /// Flag for read only number of seats
        /// </summary>
        public bool IsNumberOfSeatsReadOnly
        {
            get => PageType == NewCarpoolingPageType.EditCarpoolRide;
        }
        /// <summary>
        /// Flag for showing the toggle button
        /// </summary>
        public bool ShowToggleButton
        {
            get
            {
                switch (PageType)
                {
                    case NewCarpoolingPageType.NewCarpoolRide:
                    case NewCarpoolingPageType.NewCarpoolRequest:
                        return true;
                    case NewCarpoolingPageType.EditCarpoolRide:
                    case NewCarpoolingPageType.EditCarpoolRequest:
                        return false;
                }

                return false;
            }
        }
        /// <summary>
        /// Flag for all data given
        /// </summary>
        public bool AllDataGiven
        {
            get
            {
                if (ShowRide)
                    return PickupPlace != null && DestinationPlace != null && Car != null && NumberOfSeats > 0;
                else if (ShowRequest)
                    return RequestPickupPlace != null && RequestDestinationPlace != null;

                return false;
            }
        }
        /// <summary>
        /// Today's date
        /// </summary>
        public DateTime DateToday { get => DateTime.Now; }
        /// <summary>
        /// API key for Google Places API
        /// </summary>
        public string ApiKey
        {
            get => "pVdnZ1DWpiG8OvYpuXPZ7V/4QEkMtRz+nWEaLsDhe4qD0Z7s19znkE6eA5ZYmpDe5C2CyiYeCSVuYH4cKVfBkO7LGMvLIwx7fkyoof4FxjGwcXsYuJmSdx0gHaovAfy9y7tG6/IjIcyBEVTSc7Q01sOYEkJVimKDCFLe/6A/z0Q=".Decrypt("NSSApp");
        }
        /// <summary>
        /// Complete departure time with date and time
        /// </summary>
        public DateTime? CompleteDepartureTime
        {
            get
            {
                if (DepartureTime != null && DepartureDate != null)
                {
                    return DepartureDate.Date + DepartureTime;
                }

                return null;
            }
        }
        /// <summary>
        /// Date for a request
        /// </summary>
        public DateTime RequestDate
        {
            get => requestDate;
            set
            {
                requestDate = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Complete from time
        /// </summary>
        public DateTime? CompleteFromTime
        {
            get
            {
                if (FromTime != null && RequestDate != null)
                {
                    return RequestDate.Date + FromTime;
                }

                return null;
            }
        }
        /// <summary>
        /// Complete to time
        /// </summary>
        public DateTime? CompleteToTime
        {
            get
            {
                if (ToTime != null && RequestDate != null)
                {
                    return RequestDate.Date + ToTime;
                }

                return null;
            }
        }
        /// <summary>
        /// Departure time
        /// </summary>
        public TimeSpan DepartureTime
        {
            get => departureTime;
            set
            {
                departureTime = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Departure date
        /// </summary>
        public DateTime DepartureDate
        {
            get => departureDate;
            set
            {
                departureDate = value;
                OnPropertyChanged();
            }
        }
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
                OnPropertyChanged(nameof(ShowRide));
                OnPropertyChanged(nameof(ShowRequest));
                OnPropertyChanged(nameof(ButtonIcon));
                OnPropertyChanged(nameof(ButtonTitle));
                OnPropertyChanged(nameof(NavigationBarTitle));
            }
        }
        /// <summary>
        /// Flag deciding whether the rides should be shown
        /// </summary>
        public bool ShowRide
        {
            get
            {
                switch (PageType)
                {
                    case NewCarpoolingPageType.NewCarpoolRide:
                    case NewCarpoolingPageType.NewCarpoolRequest:
                        return toggleType == ToggleType.Left;
                    case NewCarpoolingPageType.EditCarpoolRide:
                        return true;
                    case NewCarpoolingPageType.EditCarpoolRequest:
                        return false;
                }

                return false;
            }
        }
        /// <summary>
        /// Flag deciding whether the requests should be shown
        /// </summary>
        public bool ShowRequest
        {
            get
            {
                switch (PageType)
                {
                    case NewCarpoolingPageType.NewCarpoolRide:
                    case NewCarpoolingPageType.NewCarpoolRequest:
                        return toggleType == ToggleType.Right;
                    case NewCarpoolingPageType.EditCarpoolRide:
                        return false;
                    case NewCarpoolingPageType.EditCarpoolRequest:
                        return true;
                }

                return false;
            }
        }
        /// <summary>
        /// From time in interval
        /// </summary>
        public TimeSpan FromTime
        {
            get => fromTime;
            set
            {
                fromTime = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// To time in interval
        /// </summary>
        public TimeSpan ToTime
        {
            get => toTime;
            set
            {
                toTime = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Events
        /// </summary>
        public ObservableCollection<Event> Events
        {
            get => events;
            set
            {
                events = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Item template for the list when selecting events
        /// </summary>
        public DataTemplate EventListItemTemplate
        {
            get
            {
                return new DataTemplate(() => new EventListViewCell());
            }
        }
        /// <summary>
        /// Selected events
        /// </summary>
        public ObservableCollection<Event> SelectedEvents
        {
            get => selectedEvents;
            set
            {
                selectedEvents = value;

                if (value != null)
                {
                    SelectedEvents.CollectionChanged += (sender, args) =>
                    {
                        OnPropertyChanged(nameof(EventsSelected));
                    };
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(EventsSelected));
            }
        }
        /// <summary>
        /// Events selected flag
        /// </summary>
        public bool EventsSelected
        {
            get => SelectedEvents != null && SelectedEvents.Count > 0;
        }
        /// <summary>
        /// Item template for the selected events
        /// </summary>
        public DataTemplate EventSelectedItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    var view = new EventSelectedView();
                    view.DeleteCommand = EventDeleteCommand;
                    return view;
                });
            }
        }
        /// <summary>
        /// Select events command
        /// </summary>
        public ICommand SelectEventsCommand
        {
            get => selectEventsCommand;
            set
            {
                selectEventsCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Event delete command
        /// </summary>
        public ICommand EventDeleteCommand
        {
            get => eventDeleteCommand;
            set
            {
                eventDeleteCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Info shown in the select car picker
        /// </summary>
        public string CarInfo
        {
            get => Car != null ? Car.CarInfo : null;
        }
        /// <summary>
        /// Selected car
        /// </summary>
        public Car Car
        {
            get => car;
            set
            {
                car = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CarInfo));
            }
        }
        /// <summary>
        /// Command for when the user wants to delete a car
        /// </summary>
        public ICommand CarDeleteCommand
        {
            get => carDeleteCommand;
            set
            {
                carDeleteCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Command for when the user wants to edit a car
        /// </summary>
        public ICommand CarEditCommand
        {
            get => carEditCommand;
            set
            {
                carEditCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Collection of cars for the select car picker
        /// </summary>
        public ObservableCollection<Car> Cars
        {
            get => cars;
            set
            {
                cars = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Item template for the select car picker
        /// </summary>
        public DataTemplate CarItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    var viewCell = new CarViewCell();
                    viewCell.DeleteCommand = CarDeleteCommand;
                    viewCell.EditCommand = CarEditCommand;
                    return viewCell;
                });
            }
        }
        /// <summary>
        /// Comment
        /// </summary>
        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Pickup place object for a carpool request
        /// </summary>
        public Place RequestPickupPlace
        {
            get => requestPickupPlace;
            set
            {
                requestPickupPlace = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Destination place object for a carpool request
        /// </summary>
        public Place RequestDestinationPlace
        {
            get => requestDestinationPlace;
            set
            {
                requestDestinationPlace = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Destination place object
        /// </summary>
        public Place DestinationPlace
        {
            get => destinationPlace;
            set
            {
                destinationPlace = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Pickup place object
        /// </summary>
        public Place PickupPlace
        {
            get => pickupPlace;
            set
            {
                pickupPlace = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Number of seats as integer
        /// </summary>
        public int NumberOfSeats
        {
            get => string.IsNullOrEmpty(NumberOfSeatsString) ? 0 : int.Parse(NumberOfSeatsString);
            set => NumberOfSeatsString = value.ToString();
        }
        /// <summary>
        /// Number of seats string
        /// </summary>
        public string NumberOfSeatsString
        {
            get => numberOfSeatsString;
            set
            {
                numberOfSeatsString = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Price per passenger as integer
        /// </summary>
        public int PricePerPassenger
        {
            get => string.IsNullOrEmpty(PricePerPassengerString) ? 0 : int.Parse(PricePerPassengerString);
            set => PricePerPassengerString = value.ToString();
        }
        /// <summary>
        /// Price per passenger string
        /// </summary>
        public string PricePerPassengerString
        {
            get => pricePerPassengerString;
            set
            {
                pricePerPassengerString = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Create command
        /// </summary>
        public ICommand ButtonCommand
        {
            get => createCommand;
            set
            {
                createCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Title for the bottom button
        /// </summary>
        public string ButtonTitle
        {
            get
            {
                switch (PageType)
                {
                    case NewCarpoolingPageType.NewCarpoolRide:
                        return string.Format(Resources.AppResources.create_this, Resources.AppResources.ride.ToLower());
                    case NewCarpoolingPageType.NewCarpoolRequest:
                        return string.Format(Resources.AppResources.create_this, Resources.AppResources.request.ToLower());
                    case NewCarpoolingPageType.EditCarpoolRide:
                        return string.Format(Resources.AppResources.edit_this, Resources.AppResources.ride.ToLower());
                    case NewCarpoolingPageType.EditCarpoolRequest:
                        return string.Format(Resources.AppResources.edit_this, Resources.AppResources.request.ToLower());
                }

                return null;
            }
        }
        /// <summary>
        /// Icon for the bottom button
        /// </summary>
        public ImageSource ButtonIcon
        {
            get
            {
                switch (PageType)
                {
                    case NewCarpoolingPageType.NewCarpoolRide:
                        return ImageSource.FromFile("ic_car.png");
                    case NewCarpoolingPageType.NewCarpoolRequest:
                        return ImageSource.FromFile("ic_request.png");
                    case NewCarpoolingPageType.EditCarpoolRide:
                    case NewCarpoolingPageType.EditCarpoolRequest:
                        return ImageSource.FromFile("ic_edit.png");
                }

                return ImageSource.FromFile("ic_plus.png");
            }
        }

        #endregion
    }
}