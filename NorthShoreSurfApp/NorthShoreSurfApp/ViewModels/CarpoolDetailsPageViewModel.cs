using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NorthShoreSurfApp.ViewModels
{
    /*****************************************************************/
    // DATA TEMPLATE SELECTOR
    /*****************************************************************/
    #region DataTemplateSelector

    public class CarpoolDetailDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CarpoolRideItemTemplate { get; set; }
        public DataTemplate CarpoolRequestItemTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is CarpoolRide)
                return CarpoolRideItemTemplate;
            else if (item is CarpoolRequest)
                return CarpoolRequestItemTemplate;
            return null;
        }
    }

    #endregion

    /*****************************************************************/
    // ENUMS
    /*****************************************************************/
    #region Enums

    public enum CarpoolDetailsPageType
    {
        CarpoolRide_Join,
        CarpoolRide_Leave,
        CarpoolRide_Own,
        CarpoolRequest_Own,
        CarpoolRequest_Other
    }

    public enum CarpoolDetailsPageObject
    {
        CarpoolRide,
        CarpoolRequest
    }

    #endregion

    public class CarpoolDetailsPageViewModel : INotifyPropertyChanged
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private CarpoolRide carpoolRide;
        private CarpoolRequest carpoolRequest;
        private User user;
        private List<CarpoolConfirmation> carpoolConfirmations;
        private List<CarpoolRide> carpoolRides;
        private ICommand buttonCommand;
        private ICommand editCommand;
        private ICommand deleteCommand;
        private ICommand backCommand;

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
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Add carpool confirmation
        /// </summary>
        /// <param name="carpoolConfirmation"></param>
        public void AddCarpoolConfirmation(CarpoolConfirmation carpoolConfirmation)
        {
            if (CarpoolRide != null)
                CarpoolRide.CarpoolConfirmations.Add(carpoolConfirmation);
            else if (CarpoolRequest != null)
                CarpoolConfirmations.Add(carpoolConfirmation);
            OnPropertyChanged(nameof(ShowButton));
            OnPropertyChanged(nameof(ButtonTitle));
            OnPropertyChanged(nameof(ButtonIcon));
        }
        /// <summary>
        /// Remove carpool confirmations from carpool ride
        /// </summary>
        /// <param name="carpoolConfirmations">The carpool confirmations to remove</param>
        public void RemoveCarpoolConfirmations(List<CarpoolConfirmation> carpoolConfirmations)
        {
            foreach (var confirmation in carpoolConfirmations)
            {
                CarpoolRide.CarpoolConfirmations.Remove(confirmation);
            }
            OnPropertyChanged(nameof(ShowButton));
            OnPropertyChanged(nameof(ButtonTitle));
            OnPropertyChanged(nameof(ButtonIcon));
        }
        /// <summary>
        /// Update properties for when setting the object to show details for
        /// </summary>
        private void UpdateProperties()
        {
            OnPropertyChanged(nameof(Details));
            OnPropertyChanged(nameof(ShowButton));
            OnPropertyChanged(nameof(ButtonTitle));
            OnPropertyChanged(nameof(ButtonIcon));
            OnPropertyChanged(nameof(NavigationBarTitle));
            OnPropertyChanged(nameof(UserInformation));
            OnPropertyChanged(nameof(Comment));
            OnPropertyChanged(nameof(ShowComment));
            OnPropertyChanged(nameof(Car));
            OnPropertyChanged(nameof(ShowCar));
            OnPropertyChanged(nameof(Passengers));
            OnPropertyChanged(nameof(ShowPassengers));
            OnPropertyChanged(nameof(Events));
            OnPropertyChanged(nameof(ShowEvents));
            OnPropertyChanged(nameof(UserInformationTitle));
            OnPropertyChanged(nameof(NavigationBarButtonOneCommand));
            OnPropertyChanged(nameof(NavigationBarButtonTwoCommand));
            OnPropertyChanged(nameof(NavigationBarButtonOneIsVisible));
            OnPropertyChanged(nameof(NavigationBarButtonTwoIsVisible));
            OnPropertyChanged(nameof(NavigationBarButtonOneIcon));
            OnPropertyChanged(nameof(NavigationBarButtonTwoIcon));
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// Type of page 
        /// </summary>
        public CarpoolDetailsPageType PageType
        {
            get
            {
                // User is not set yet
                if (User == null)
                    return default;

                if (CarpoolRide != null)
                {
                    // The user is the driver
                    if (CarpoolRide.DriverId == User.Id)
                        return CarpoolDetailsPageType.CarpoolRide_Own;
                    // The user has requested to join the carpool ride or is already confirmed
                    if (CarpoolRide.CarpoolConfirmations.Any(x => x.PassengerId == User.Id && !x.IsInvite))
                        return CarpoolDetailsPageType.CarpoolRide_Leave;
                    // The user has no relation to the carpool ride
                    else if (!CarpoolRide.CarpoolConfirmations.Any(x => x.PassengerId == User.Id))
                        return CarpoolDetailsPageType.CarpoolRide_Join;
                }
                else if (CarpoolRequest != null)
                {
                    // The user is the one who has made the request
                    if (CarpoolRequest.PassengerId == User.Id)
                        return CarpoolDetailsPageType.CarpoolRequest_Own;
                    // This is not the user's request
                    else
                        return CarpoolDetailsPageType.CarpoolRequest_Other;
                }

                return default;
            }
        }
        /// <summary>
        /// Type of object that is shown details for
        /// </summary>
        public CarpoolDetailsPageObject PageObject
        {
            get
            {
                switch (PageType)
                {
                    case CarpoolDetailsPageType.CarpoolRide_Join:
                    case CarpoolDetailsPageType.CarpoolRide_Leave:
                    case CarpoolDetailsPageType.CarpoolRide_Own:
                        return CarpoolDetailsPageObject.CarpoolRide;
                    case CarpoolDetailsPageType.CarpoolRequest_Own:
                    case CarpoolDetailsPageType.CarpoolRequest_Other:
                        return CarpoolDetailsPageObject.CarpoolRequest;
                }

                return default;
            }
        }
        /// <summary>
        /// Back command for the page
        /// </summary>
        public ICommand BackCommand
        {
            get => backCommand;
            set
            {
                backCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Flag telling if the object in question is owned by the user
        /// </summary>
        private bool IsUsersOwn
        {
            get
            {
                if (User == null)
                    return false;

                if (CarpoolRide != null)
                    return CarpoolRide.DriverId == User.Id;
                else if (CarpoolRequest != null)
                    return CarpoolRequest.PassengerId == User.Id;

                return false;
            }
        }
        /// <summary>
        /// Edit command for the page
        /// </summary>
        public ICommand EditCommand
        {
            get => editCommand;
            set
            {
                editCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Delete command for the page
        /// </summary>
        public ICommand DeleteCommand
        {
            get => deleteCommand;
            set
            {
                deleteCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Navigation bar button one command
        /// </summary>
        public ICommand NavigationBarButtonOneCommand
        {
            get
            {
                return DeleteCommand;
            }
        }
        /// <summary>
        /// Navigation bar button two command
        /// </summary>
        public ICommand NavigationBarButtonTwoCommand
        {
            get
            {
                return EditCommand;
            }
        }
        /// <summary>
        /// Flag for navigation bar button one visibility
        /// </summary>
        public bool NavigationBarButtonOneIsVisible
        {
            get
            {
                if (!IsUsersOwn)
                    return false;

                if (CarpoolRide != null)
                    return true;
                else if (CarpoolRequest != null)
                    return CarpoolRequest.IsActive;

                return false;
            }
        }
        /// <summary>
        /// Flag for navigation bar button two visibility
        /// </summary>
        public bool NavigationBarButtonTwoIsVisible
        {
            get
            {
                if (!IsUsersOwn)
                    return false;

                if (CarpoolRide != null)
                    return !CarpoolRide.IsLocked;
                else if (CarpoolRequest != null)
                    return CarpoolRequest.IsActive;

                return false;
            }
        }
        /// <summary>
        /// Navigation bar button one icon
        /// </summary>
        public ImageSource NavigationBarButtonOneIcon
        {
            get
            {
                return ImageSource.FromFile("ic_delete.png");
            }
        }
        /// <summary>
        /// Navigation bar button two icon
        /// </summary>
        public ImageSource NavigationBarButtonTwoIcon
        {
            get
            {
                return ImageSource.FromFile("ic_edit.png");
            }
        }
        /// <summary>
        /// List of the user's carpool confirmations
        /// </summary>
        public List<CarpoolConfirmation> CarpoolConfirmations
        {
            get => carpoolConfirmations;
            set
            {
                carpoolConfirmations = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowButton));
                OnPropertyChanged(nameof(ButtonIcon));
            }
        }
        /// <summary>
        /// List of the user's carpool rides
        /// </summary>
        public List<CarpoolRide> CarpoolRides
        {
            get => carpoolRides;
            set
            {
                carpoolRides = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowButton));
                OnPropertyChanged(nameof(ButtonIcon));
            }
        }
        /// <summary>
        /// The object holding the user info
        /// </summary>
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowButton));
                OnPropertyChanged(nameof(Passengers));
                OnPropertyChanged(nameof(ShowPassengers));
                OnPropertyChanged(nameof(NavigationBarButtonOneCommand));
                OnPropertyChanged(nameof(NavigationBarButtonTwoCommand));
                OnPropertyChanged(nameof(NavigationBarButtonOneIsVisible));
                OnPropertyChanged(nameof(NavigationBarButtonTwoIsVisible));
                OnPropertyChanged(nameof(NavigationBarButtonOneIcon));
                OnPropertyChanged(nameof(NavigationBarButtonTwoIcon));
            }
        }
        /// <summary>
        /// The object holding the car info
        /// </summary>
        public Car Car
        {
            get
            {
                if (CarpoolRide != null)
                    return CarpoolRide.Car;

                return null;
            }
        }
        /// <summary>
        /// Flag deciding if the car information should be shown
        /// </summary>
        public bool ShowCar
        {
            get
            {
                return Car != null;
            }
        }
        /// <summary>
        /// Selected carpool ride
        /// </summary>
        public CarpoolRide CarpoolRide
        {
            get { return carpoolRide; }
            set
            {
                carpoolRide = value;
                OnPropertyChanged();
                UpdateProperties();
            }
        }
        /// <summary>
        /// Selected carpool request
        /// </summary>
        public CarpoolRequest CarpoolRequest
        {
            get { return carpoolRequest; }
            set
            {
                carpoolRequest = value;
                OnPropertyChanged();
                UpdateProperties();
            }
        }
        /// <summary>
        /// Command for the button at the bottom
        /// </summary>
        public ICommand ButtonCommand
        {
            get => buttonCommand;
            set
            {
                buttonCommand = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Title for the navigation bar
        /// </summary>
        public string NavigationBarTitle
        {
            get
            {
                if (CarpoolRide != null)
                    return Resources.AppResources.ride;
                else if (CarpoolRequest != null)
                    return Resources.AppResources.request;

                return string.Empty;
            }
        }
        /// <summary>
        /// Title for the navigation bar
        /// </summary>
        public string UserInformationTitle
        {
            get
            {
                if (CarpoolRide != null)
                    return Resources.AppResources.driver;
                else if (CarpoolRequest != null)
                    return Resources.AppResources.passenger;

                return string.Empty;
            }
        }
        /// <summary>
        /// Title for the button at the bottom of the page
        /// </summary>
        public string ButtonTitle
        {
            get
            {
                if (CarpoolRide != null)
                {
                    if (PageType == CarpoolDetailsPageType.CarpoolRide_Leave)
                        return Resources.AppResources.leave_ride;
                    else
                        return Resources.AppResources.join_ride;
                }
                else if (CarpoolRequest != null)
                    return Resources.AppResources.invite_to_ride;

                return string.Empty;
            }
        }
        /// <summary>
        /// Icon for the button at the bottom of the page
        /// </summary>
        public ImageSource ButtonIcon
        {
            get
            {
                if (CarpoolRide != null)
                {
                    if (PageType == CarpoolDetailsPageType.CarpoolRide_Leave)
                        return ImageSource.FromFile("ic_log_out.png");

                    return ImageSource.FromFile("ic_request.png");
                }
                else if (CarpoolRequest != null)
                    return ImageSource.FromFile("ic_plus.png");

                return string.Empty;
            }
        }
        /// <summary>
        /// Flag for showing the bottom button
        /// </summary>
        public bool ShowButton
        {
            get
            {
                // If user is not set yet return false
                if (User == null)
                    return false;

                // Carpool ride
                if (CarpoolRide != null)
                {
                    // Do not show the button if it is the user's carpool ride
                    if (User.Id == CarpoolRide.DriverId)
                        return false;
                    else
                    {
                        // Show a leave button if the passenger already 
                        // has a confirmation for this carpool ride
                        if (CarpoolRide.CarpoolConfirmations.Any(x => x.PassengerId == User.Id))
                            return true;
                        // The user can only request to join a carpool ride 
                        // if there is still available seats
                        return CarpoolRide.AvailableSeats > 0;
                    }
                }
                // Carpool request
                else if (CarpoolRequest != null)
                {
                    // Do not show the button if it is the user's carpool request
                    if (User.Id == CarpoolRequest.PassengerId)
                        return false;
                    else
                    {
                        bool showButton = false;
                        // If the user has no carpool rides they cannot invite anyone
                        // So the button should not be shown
                        if (CarpoolRides == null || CarpoolRides.Count == 0)
                            return false;

                        // If there is any carpool ride that the passenger 
                        // has not been invited till yet show the button
                        foreach (var ride in CarpoolRides)
                        {
                            if (!ride.CarpoolConfirmations.Any(x => x.PassengerId == CarpoolRequest.PassengerId && x.CarpoolRequestId == CarpoolRequest.Id))
                            {
                                showButton = true;
                                break;
                            }
                        }

                        return showButton;
                    }
                }

                return false;
            }
        }
        /// <summary>
        /// Collection for the details view
        /// </summary>
        public ObservableCollection<object> Details
        {
            get
            {
                var collection = new ObservableCollection<object>();

                if (CarpoolRide != null)
                {
                    collection.Add(CarpoolRide);
                }
                else if (CarpoolRequest != null)
                {
                    collection.Add(CarpoolRequest);
                }

                return collection;
            }
        }
        /// <summary>
        /// Collection for the user information view
        /// </summary>
        public ObservableCollection<User> UserInformation
        {
            get
            {
                var collection = new ObservableCollection<User>();

                if (CarpoolRide != null)
                {
                    collection.Add(CarpoolRide.Driver);
                }
                else if (CarpoolRequest != null)
                {
                    collection.Add(CarpoolRequest.Passenger);
                }

                return collection;
            }
        }
        /// <summary>
        /// Collection for the passengers information view
        /// </summary>
        public ObservableCollection<User> Passengers
        {
            get
            {
                var collection = new ObservableCollection<User>();
                // Carpool ride and user is not set yet
                if (CarpoolRide == null || User == null)
                    return null;
                // The user is the driver or the user is one of the passengers confirmed for the ride
                if (CarpoolRide.DriverId == User.Id || CarpoolRide.CarpoolConfirmations.Any(x => x.IsConfirmed && x.PassengerId == User.Id))
                {
                    // Get all the confirmed passengers
                    var passengers = CarpoolRide.CarpoolConfirmations
                                                .Where(x => x.IsConfirmed)
                                                .Select(x => x.Passenger)
                                                .ToList();
                    if (passengers.Count == 0)
                        return null;
                    return new ObservableCollection<User>(passengers);
                }

                return null;
            }
        }
        /// <summary>
        /// Collection for the events view
        /// </summary>
        public ObservableCollection<Event> Events
        {
            get
            {
                var collection = new ObservableCollection<Event>();

                if (CarpoolRide != null)
                {
                    // Get related events for the carpool ride
                    collection = new ObservableCollection<Event>(
                        CarpoolRide.CarpoolRides_Events_Relations
                                   .Select(x => x.Event)
                                   .ToList());
                }
                else if (CarpoolRequest != null)
                {
                    // Get related events for the carpool request
                    collection = new ObservableCollection<Event>(
                        CarpoolRequest.CarpoolRequests_Events_Relations
                                      .Select(x => x.Event)
                                      .ToList());
                }

                // Return null if there is no events
                if (collection.Count == 0)
                    return null;
                return collection;
            }
        }
        /// <summary>
        /// Flag used to know when to show the passengers information
        /// </summary>
        public bool ShowEvents
        {
            get => Events != null;
        }
        /// <summary>
        /// Flag used to know when to show the passengers information
        /// </summary>
        public bool ShowPassengers
        {
            get => Passengers != null;
        }
        /// <summary>
        /// Comment to show on the details page
        /// </summary>
        public string Comment
        {
            get
            {
                if (CarpoolRide != null)
                    return CarpoolRide.Comment;
                else if (CarpoolRequest != null)
                    return CarpoolRequest.Comment;

                return string.Empty;
            }
        }
        /// <summary>
        /// Only show the comment if it is not null or empty
        /// </summary>
        public bool ShowComment
        {
            get => Comment != null && Comment != string.Empty;
        }
        /// <summary>
        /// Item template for a carpool ride
        /// </summary>
        public DataTemplate CarpoolRideItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    View view = new CarpoolRideViewCell().View;
                    view.Margin = new Thickness(10, 0, 10, 0);
                    return view;
                });
            }
        }
        /// <summary>
        /// Item template for a carpool request
        /// </summary>
        public DataTemplate CarpoolRequestItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    View view = new CarpoolRequestViewCell().View;
                    view.Margin = new Thickness(10, 0, 10, 0);
                    return view;
                }
              );
            }
        }
        /// <summary>
        /// Item template for the user information
        /// </summary>
        public DataTemplate UserItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    CustomFrame frame = (CustomFrame)new UserViewCell().View;
                    frame.Margin = new Thickness(0, 0, 0, 0);
                    frame.Padding = new Thickness(10, 0, 10, 5);
                    return frame;
                });
            }
        }
        /// <summary>
        /// Item template for the events
        /// </summary>
        public DataTemplate EventItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    CustomFrame frame = (CustomFrame)new EventViewCell().View;
                    frame.Margin = new Thickness(0, 0, 0, 0);
                    frame.Padding = new Thickness(10, 0, 10, 5);
                    return frame;
                });
            }
        }
        /// <summary>
        /// Item template for the passengers
        /// </summary>
        public DataTemplate PassengerItemTemplate
        {
            get
            {
                return new DataTemplate(() =>
                {
                    CustomFrame frame = (CustomFrame)new UserViewCell().View;
                    frame.Margin = new Thickness(0, 0, 0, 0);
                    frame.Padding = new Thickness(10, 0, 10, 5);
                    frame.BackgroundColor = Color.Transparent;
                    return frame;
                });
            }
        }
        /// <summary>
        /// Details data template selector
        /// </summary>
        public DataTemplateSelector DetailsDataTemplateSelector
        {
            get
            {
                CarpoolDetailDataTemplateSelector dataTemplateSelector = new CarpoolDetailDataTemplateSelector();
                dataTemplateSelector.CarpoolRequestItemTemplate = CarpoolRequestItemTemplate;
                dataTemplateSelector.CarpoolRideItemTemplate = CarpoolRideItemTemplate;
                return dataTemplateSelector;
            }
        }
        /// <summary>
        /// Item template for a carpool ride in a list
        /// </summary>
        public DataTemplate CarpoolRideListItemTemplate
        {
            get { return new DataTemplate(() => new CarpoolRideViewCell()); }
        }
        /// <summary>
        /// List for carpool rides selector dialog
        /// </summary>
        public List<CarpoolRide> CarpoolRidesList
        {
            get => CarpoolRides.Where(x => !x.CarpoolConfirmations.Any(x2 => x2.CarpoolRequestId == CarpoolRequest.Id))
                               .ToList();
        }

        #endregion
    }
}