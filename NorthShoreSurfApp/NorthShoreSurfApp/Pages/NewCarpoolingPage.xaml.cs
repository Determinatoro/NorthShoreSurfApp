using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NorthShoreSurfApp.ViewModels;
using DurianCode.PlacesSearchBar;
using NorthShoreSurfApp.ModelComponents;
using System.Collections.ObjectModel;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCarpoolingPage : ContentPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public NewCarpoolingPageViewModel NewCarpoolingPageViewModel { get => (NewCarpoolingPageViewModel)this.BindingContext; }

        public Action<CarpoolRide> CarpoolRideUpdated;
        public Action<CarpoolRequest> CarpoolRequestUpdated;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public NewCarpoolingPage(CarpoolRequest carpoolRequest) : this()
        {
            cpbRequestDestination.DisableSearch = true;
            cpbRequestPickupPoint.DisableSearch = true;

            NewCarpoolingPageViewModel.PageType = NewCarpoolingPageType.EditCarpoolRequest;
            NewCarpoolingPageViewModel.CarpoolRequest = carpoolRequest;
            NewCarpoolingPageViewModel.ToggleType = ToggleType.Right;

            cpbRequestPickupPoint.DisableSearch = false;
            cpbRequestDestination.DisableSearch = false;

            NewCarpoolingPageViewModel.ButtonCommand = new Command(() =>
            {
                if (!NewCarpoolingPageViewModel.AllDataGiven)
                {
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_fill_out_all_the_empty_fields);
                    return;
                }

                if (NewCarpoolingPageViewModel.CompleteFromTime.Value.CompareTo(DateTime.Now) < 0)
                {
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_select_a_time_in_the_future);
                    return;
                }
                if (NewCarpoolingPageViewModel.CompleteFromTime.Value.CompareTo(NewCarpoolingPageViewModel.CompleteToTime.Value) >= 0)
                {
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_select_valid_to_and_from_times);
                    return;
                }

                EditCarpoolRequest();
            });
        }

        public NewCarpoolingPage(CarpoolRide carpoolRide) : this()
        {
            cpbDestination.DisableSearch = true;
            cpbPickupPoint.DisableSearch = true;

            NewCarpoolingPageViewModel.PageType = NewCarpoolingPageType.EditCarpoolRide;
            NewCarpoolingPageViewModel.CarpoolRide = carpoolRide;
            NewCarpoolingPageViewModel.ToggleType = ToggleType.Left;

            cpbPickupPoint.DisableSearch = false;
            cpbDestination.DisableSearch = false;

            // Create button command
            NewCarpoolingPageViewModel.ButtonCommand = new Command(() =>
            {
                if (!NewCarpoolingPageViewModel.AllDataGiven)
                {
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_fill_out_all_the_empty_fields);
                    return;
                }

                if (NewCarpoolingPageViewModel.CompleteDepartureTime.Value.CompareTo(DateTime.Now) < 0)
                {
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_select_a_time_in_the_future);
                    return;
                }

                EditCarpoolRide();
            });
        }

        public NewCarpoolingPage(NewCarpoolingPageType pageType) : this()
        {
            NewCarpoolingPageViewModel.PageType = pageType;

            switch (pageType)
            {
                case NewCarpoolingPageType.NewCarpoolRide:
                    NewCarpoolingPageViewModel.ToggleType = ToggleType.Left;
                    break;
                case NewCarpoolingPageType.NewCarpoolRequest:
                    NewCarpoolingPageViewModel.ToggleType = ToggleType.Right;
                    break;
            }

            cpbDestination.DisableSearch = true;
            cpbRequestDestination.DisableSearch = true;
            NewCarpoolingPageViewModel.SetDefaultDestinationPlace();
            cpbDestination.DisableSearch = false;
            cpbRequestDestination.DisableSearch = false;

            // Toggled event
            ctbNewCarpool.Toggled += (sender, args) =>
            {
                cpbRequestDestination.ResetPredictions();
                cpbDestination.ResetPredictions();
                cpbPickupPoint.ResetPredictions();
                cpbRequestPickupPoint.ResetPredictions();
                // Change page type
                NewCarpoolingPageViewModel.PageType = NewCarpoolingPageViewModel.ToggleType == ToggleType.Left ? NewCarpoolingPageType.NewCarpoolRide : NewCarpoolingPageType.NewCarpoolRequest;
            };

            // Create button command
            NewCarpoolingPageViewModel.ButtonCommand = new Command(() =>
            {
                if (!NewCarpoolingPageViewModel.AllDataGiven)
                {
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_fill_out_all_the_empty_fields);
                    return;
                }

                if (NewCarpoolingPageViewModel.ShowRide)
                {
                    if (NewCarpoolingPageViewModel.CompleteDepartureTime.Value.CompareTo(DateTime.Now) < 0)
                    {
                        this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_select_a_time_in_the_future);
                        return;
                    }

                    SaveCarpoolRide();
                }
                else if (NewCarpoolingPageViewModel.ShowRequest)
                {
                    if (NewCarpoolingPageViewModel.CompleteFromTime.Value.CompareTo(DateTime.Now) < 0)
                    {
                        this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_select_a_time_in_the_future);
                        return;
                    }
                    if (NewCarpoolingPageViewModel.CompleteFromTime.Value.CompareTo(NewCarpoolingPageViewModel.CompleteToTime.Value) >= 0)
                    {
                        this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_select_valid_to_and_from_times);
                        return;
                    }

                    SaveCarpoolRequest();
                }
            });
        }

        private NewCarpoolingPage()
        {
            // Hide default navigation bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            // Initialize the page
            InitializeComponent();
            // iOS safe area insets
            ((Grid)Content).SetIOSSafeAreaInsets(this);

            // Back button clicked
            navigationBar.BackButtonClicked += (sender, args) =>
            {
                PopPage();
            };
            // Car picker
            cpCar.Clicked += (sender, args) =>
            {
                GetCars();
            };
            // Select events command
            NewCarpoolingPageViewModel.SelectEventsCommand = new Command(() =>
            {
                GetEvents();
            });
            // Delete event command
            NewCarpoolingPageViewModel.EventDeleteCommand = new Command((parameter) =>
            {
                var _event = ((Func<Event>)parameter).Invoke();

                this.ShowYesNo(string.Format(NorthShoreSurfApp.Resources.AppResources.are_you_sure_you_want_to_remove_this, NorthShoreSurfApp.Resources.AppResources._event.ToLower()),
                    () =>
                    {
                        NewCarpoolingPageViewModel.SelectedEvents.Remove(_event);
                    });
            });
        }

        #endregion

        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        // OnBackButtonPressed
        protected override bool OnBackButtonPressed()
        {
            PopPage();
            return true;
        }

        // OnAppearing
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Pop the current modal page
        /// </summary>
        private async void PopPage()
        {
            while (Navigation.ModalStack.Count > 0 && Navigation.ModalStack.Contains(this))
                await Navigation.PopModalAsync();
        }
        /// <summary>
        /// Get cars
        /// </summary>
        private void GetCars()
        {
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                true,
                () => App.DataService.GetCars(AppValuesService.GetUserId().Value),
                async (response) =>
                {
                    // Check response
                    if (response.Success)
                    {
                        // Set car objects
                        NewCarpoolingPageViewModel.Cars = new ObservableCollection<Car>(response.Result);
                        // Add new car option
                        NewCarpoolingPageViewModel.Cars.Add(new Car() { Title = NorthShoreSurfApp.Resources.AppResources.add_new_car });

                        // Show list selection
                        CustomListDialog customListDialog = new CustomListDialog(
                            NewCarpoolingPageViewModel.CarItemTemplate,
                            NewCarpoolingPageViewModel.Cars,
                            string.Format(NorthShoreSurfApp.Resources.AppResources.select_parameter, NorthShoreSurfApp.Resources.AppResources.car.ToLower())
                            );
                        // Do not close the dialog when an item is selected
                        customListDialog.CloseOnItemTapped = false;
                        // Set car delete command
                        NewCarpoolingPageViewModel.CarDeleteCommand = new Command((parameter) =>
                        {
                            // Get car from view cell
                            var car = ((Func<Car>)parameter).Invoke();
                            // Yes no dialog
                            this.ShowYesNo(NorthShoreSurfApp.Resources.AppResources.are_you_sure_you_want_to_delete_this_car, () =>
                            {
                                // Remove the selected car
                                RemoveCar(car);
                            });
                        });
                        // List item tapped
                        customListDialog.ItemTapped += async (sender, args) =>
                        {
                            // Selected car
                            var item = (Car)args.Item;
                            // A car has been selected
                            if (!item.IsTitle)
                            {
                                // Car selected
                                await PopupNavigation.Instance.PopAsync();
                                NewCarpoolingPageViewModel.Car = item;
                            }
                            // Add new car selected
                            else
                            {
                                // Unselect item
                                customListDialog.UnselectItem();
                                // Add new car
                                AddCar();

                            }
                        };
                        await PopupNavigation.Instance.PushAsync(customListDialog);
                    }
                    // Show error
                    else
                        this.ShowMessage(response.ErrorMessage);
                });
        }
        /// <summary>
        /// Add car
        /// </summary>
        private async void AddCar()
        {
            NewCarPage newCarPage = new NewCarPage();
            newCarPage.NewCarAdded = (car) =>
            {
                NewCarpoolingPageViewModel.Cars.Insert(NewCarpoolingPageViewModel.Cars.Count - 1, car);
            };
            await PopupNavigation.Instance.PushAsync(newCarPage);
        }
        /// <summary>
        /// Remove car
        /// </summary>
        /// <param name="car"></param>
        private void RemoveCar(Car car)
        {
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                            true,
                            () => App.DataService.DeleteCar(car.Id),
                            (response) =>
                            {
                                // Set opening hours content
                                if (response.Success)
                                {
                                    NewCarpoolingPageViewModel.Cars.Remove(car);
                                }
                                // Show error
                                else
                                    this.ShowMessage(response.ErrorMessage);
                            });
        }
        /// <summary>
        /// Get events
        /// </summary>
        private void GetEvents()
        {
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                true,
                () => App.DataService.GetEvents(),
                async (response) =>
                {
                    // Set opening hours content
                    if (response.Success)
                    {
                        // Set event objects
                        NewCarpoolingPageViewModel.Events = new ObservableCollection<Event>(response.Result);
                        // Set flag for all the events that are already selected
                        if (NewCarpoolingPageViewModel.SelectedEvents != null)
                        {
                            foreach (var _event in NewCarpoolingPageViewModel.Events)
                            {
                                _event.IsSelected = NewCarpoolingPageViewModel.SelectedEvents.Contains(_event);
                            }
                        }
                        // Show list selection
                        CustomListDialog customListDialog = new CustomListDialog(
                            NewCarpoolingPageViewModel.EventListItemTemplate,
                            NewCarpoolingPageViewModel.Events,
                            string.Format(NorthShoreSurfApp.Resources.AppResources.select_parameter, NorthShoreSurfApp.Resources.AppResources.events.ToLower())
                            );
                        // Show the select button if any events are selected
                        if (NewCarpoolingPageViewModel.Events.Any(x => x.IsSelected))
                            customListDialog.ShowSelectButton = true;
                        // Do not close the dialog when an item is selected
                        customListDialog.CloseOnItemTapped = false;
                        // Set dialog select command
                        customListDialog.SelectCommand = new Command(async () =>
                        {
                            var selectedEvents = ((ObservableCollection<Event>)customListDialog.ItemsSource).Where(x => x.IsSelected).ToObservableCollection();
                            NewCarpoolingPageViewModel.SelectedEvents = selectedEvents;
                            await PopupNavigation.Instance.PopAsync();
                        });
                        // List item tapped
                        customListDialog.ItemTapped += (sender, args) =>
                        {
                            // Unselect item in list
                            customListDialog.UnselectItem();
                            // Selected event
                            var item = (Event)args.Item;
                            item.IsSelected = !item.IsSelected;
                            customListDialog.ShowSelectButton = ((ObservableCollection<Event>)customListDialog.ItemsSource).Any(x => x.IsSelected);
                        };
                        await PopupNavigation.Instance.PushAsync(customListDialog);
                    }
                    // Show error
                    else
                        this.ShowMessage(response.ErrorMessage);
                });
        }
        /// <summary>
        /// Save carpool ride
        /// </summary>
        private void SaveCarpoolRide()
        {
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.CreateCarpoolRide(
                             AppValuesService.UserId.Value,
                             NewCarpoolingPageViewModel.CompleteDepartureTime.Value,
                             NewCarpoolingPageViewModel.PickupPlace.Address,
                             NewCarpoolingPageViewModel.PickupPlace.ZipCode,
                             NewCarpoolingPageViewModel.PickupPlace.City,
                             NewCarpoolingPageViewModel.DestinationPlace.Address,
                             NewCarpoolingPageViewModel.DestinationPlace.ZipCode,
                             NewCarpoolingPageViewModel.DestinationPlace.City,
                             NewCarpoolingPageViewModel.Car.Id,
                             NewCarpoolingPageViewModel.NumberOfSeats,
                             NewCarpoolingPageViewModel.PricePerPassenger,
                             NewCarpoolingPageViewModel.SelectedEvents == null ? new List<Event>() : NewCarpoolingPageViewModel.SelectedEvents.ToList(),
                             NewCarpoolingPageViewModel.Comment
                             ),
                            (response) =>
                            {
                                // Close this page
                                if (response.Success)
                                {
                                    PopPage();
                                }
                                // Show error
                                else
                                    this.ShowMessage(response.ErrorMessage);
                            });
        }
        /// <summary>
        /// Edit carpool ride
        /// </summary>
        private void EditCarpoolRide()
        {
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.UpdateCarpoolRide(
                                NewCarpoolingPageViewModel.CarpoolRide.Id,
                                NewCarpoolingPageViewModel.CompleteDepartureTime.Value,
                                NewCarpoolingPageViewModel.PickupPlace.Address,
                                NewCarpoolingPageViewModel.PickupPlace.ZipCode,
                                NewCarpoolingPageViewModel.PickupPlace.City,
                                NewCarpoolingPageViewModel.DestinationPlace.Address,
                                NewCarpoolingPageViewModel.DestinationPlace.ZipCode,
                                NewCarpoolingPageViewModel.DestinationPlace.City,
                                NewCarpoolingPageViewModel.Car.Id,
                                NewCarpoolingPageViewModel.PricePerPassenger,
                                NewCarpoolingPageViewModel.SelectedEvents == null ? new List<Event>() : NewCarpoolingPageViewModel.SelectedEvents.ToList(),
                                NewCarpoolingPageViewModel.Comment
                            ),
                            (response) =>
                            {
                                // Close this page
                                if (response.Success)
                                {
                                    // Invoke callback
                                    CarpoolRideUpdated?.Invoke(response.Result);
                                    PopPage();
                                }
                                // Show error
                                else
                                    this.ShowMessage(response.ErrorMessage);
                            });
        }
        /// <summary>
        /// Save carpool request
        /// </summary>
        private void SaveCarpoolRequest()
        {
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.CreateCarpoolRequest(
                                AppValuesService.UserId.Value,
                                NewCarpoolingPageViewModel.CompleteFromTime.Value,
                                NewCarpoolingPageViewModel.CompleteToTime.Value,
                                NewCarpoolingPageViewModel.RequestPickupPlace.ZipCode,
                                NewCarpoolingPageViewModel.RequestPickupPlace.City,
                                NewCarpoolingPageViewModel.RequestDestinationPlace.Address,
                                NewCarpoolingPageViewModel.RequestDestinationPlace.ZipCode,
                                NewCarpoolingPageViewModel.RequestDestinationPlace.City,
                                NewCarpoolingPageViewModel.SelectedEvents == null ? new List<Event>() : NewCarpoolingPageViewModel.SelectedEvents.ToList(),
                                NewCarpoolingPageViewModel.Comment
                            ),
                            (response) =>
                            {
                                // Close this page
                                if (response.Success)
                                {
                                    PopPage();
                                }
                                // Show error
                                else
                                    this.ShowMessage(response.ErrorMessage);
                            });
        }
        /// <summary>
        /// Edit carpool request
        /// </summary>
        private void EditCarpoolRequest()
        {
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.UpdateCarpoolRequest(
                                NewCarpoolingPageViewModel.CarpoolRequest.Id,
                                NewCarpoolingPageViewModel.CompleteFromTime.Value,
                                NewCarpoolingPageViewModel.CompleteToTime.Value,
                                NewCarpoolingPageViewModel.RequestPickupPlace.ZipCode,
                                NewCarpoolingPageViewModel.RequestPickupPlace.City,
                                NewCarpoolingPageViewModel.RequestDestinationPlace.Address,
                                NewCarpoolingPageViewModel.RequestDestinationPlace.ZipCode,
                                NewCarpoolingPageViewModel.RequestDestinationPlace.City,
                                NewCarpoolingPageViewModel.SelectedEvents == null ? new List<Event>() : NewCarpoolingPageViewModel.SelectedEvents.ToList(),
                                NewCarpoolingPageViewModel.Comment
                            ),
                            (response) =>
                            {
                                // Close this page
                                if (response.Success)
                                {
                                    // Invoke callback
                                    CarpoolRequestUpdated?.Invoke(response.Result);
                                    PopPage();
                                }
                                // Show error
                                else
                                    this.ShowMessage(response.ErrorMessage);
                            });
        }

        #endregion
    }
}