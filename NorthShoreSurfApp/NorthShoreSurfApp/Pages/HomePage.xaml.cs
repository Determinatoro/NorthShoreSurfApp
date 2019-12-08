using NorthShoreSurfApp.Database;
using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.Services;
using NorthShoreSurfApp.ViewCells;
using NorthShoreSurfApp.ViewModels;
using Plugin.DeviceOrientation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace NorthShoreSurfApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class HomePage : ContentPage, ITabbedPageService
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        private HomeViewModel HomeViewModel { get => (HomeViewModel)this.BindingContext; }

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public HomePage()
        {
            // Hide default navigation bar
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            // Initialize the page
            InitializeComponent();
            // iOS safe area insets
            ((Grid)Content).SetIOSSafeAreaInsets(this);

            // Tapped on weather and ocean forecasts
            TapGestureRecognizer frameTap = new TapGestureRecognizer();
            frameTap.Tapped += async (sender, e) =>
            {
                if (sender == frameOpeningHours)
                    await Navigation.PushModalAsync(new HomeDetailsPage(HomeDetailsPageType.OpeningHours));
                else if (sender == frameContactUs)
                    await Navigation.PushModalAsync(new HomeDetailsPage(HomeDetailsPageType.ContactInfo));
                else if (sender == frameWebcam)
                    await Navigation.PushModalAsync(new SurfingConditionsFullscreenPage(SurfingConditionsViewModel.VideoUrl), false);
                else if (sender == frameNextRide)
                {
                    if (AppValuesService.IsGuest())
                    {
                        var tabbedPage = ((RootTabbedPage)App.Current.MainPage);
                        tabbedPage.SelectedItem = tabbedPage.Children.Where(x => ((Xamarin.Forms.NavigationPage)x).RootPage is WelcomePage).FirstOrDefault();
                    }
                    else
                    {
                        // Show details about next ride
                        GetCars();
                    }
                }
            };
            frameOpeningHours.GestureRecognizers.Add(frameTap);
            frameContactUs.GestureRecognizers.Add(frameTap);
            frameWebcam.GestureRecognizers.Add(frameTap);
            frameNextRide.GestureRecognizers.Add(frameTap);

            cpCar.Clicked += (sender, args) =>
            {
                GetCars();
            };
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Get information for the home page from the data service
        /// </summary>
        public void GetInformation()
        {
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                false,
                async () =>
                {
                    // Todays opening hours
                    var response = await App.DataService.GetTodaysOpeningHours();
                    // Informatio about the next carpool ride
                    var response2 = await App.DataService.GetNextCarpoolRide();
                    return new Tuple<DataResponse<string>, DataResponse<CarpoolRide>>(response, response2);
                },
                (response) =>
                {
                    // Set opening hours content
                    if (response.Item1.Success)
                        HomeViewModel.OpeningHoursContent = response.Item1.Result;
                    // Show error
                    else
                        this.ShowMessage(response.Item1.ErrorMessage);

                    // Set next carpool ride
                    if (response.Item2.Success)
                        HomeViewModel.NextCarpoolRide = response.Item2.Result;
                    // Show error
                    else
                        this.ShowMessage(response.Item2.ErrorMessage);
                });
        }

        private void GetCars()
        {
            App.DataService.GetData(
                NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                true,
                () => App.DataService.GetCars(AppValuesService.GetUserId().Value),
                async (response) =>
                {
                    // Set opening hours content
                    if (response.Success)
                    {
                        // Set car objects
                        HomeViewModel.Cars = new ObservableCollection<Car>(response.Result);
                        // Add new car option
                        HomeViewModel.Cars.Add(new Car() { Title = NorthShoreSurfApp.Resources.AppResources.add_new_car });

                        // Show list selection
                        CustomListDialog customListDialog = new CustomListDialog(
                            HomeViewModel.CarItemTemplate,
                            HomeViewModel.Cars,
                            string.Format(NorthShoreSurfApp.Resources.AppResources.select_parameter, NorthShoreSurfApp.Resources.AppResources.car.ToLower())
                            );
                        // Do not close the dialog when an item is selected
                        customListDialog.CloseOnItemTapped = false;
                        // Set car delete command
                        HomeViewModel.CarDeleteCommand = new Command(async (parameter) =>
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
                                HomeViewModel.CarInfo = item.CarInfo;
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

        private async void AddCar()
        {
            NewCarPage newCarPage = new NewCarPage();
            newCarPage.NewCarAdded = (car) =>
            {
                HomeViewModel.Cars.Insert(HomeViewModel.Cars.Count - 1, car);
            };
            await PopupNavigation.Instance.PushAsync(newCarPage);
        }

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
                                    HomeViewModel.Cars.Remove(car);
                                }
                                // Show error
                                else
                                    this.ShowMessage(response.ErrorMessage);
                            });
        }

        #endregion

        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        // OnAppearing
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Navigation.ModalStack.Count == 0)
            {
                // Orientation
                CrossDeviceOrientation.Current.LockOrientation(Plugin.DeviceOrientation.Abstractions.DeviceOrientations.Portrait);
                // Show status bar on android
                App.ScreenService.ShowStatusBar();
            }
        }

        #endregion

        /*****************************************************************/
        // INTERFACE METHODS
        /*****************************************************************/
        #region Interface methods

        // OnPageSelected
        public void OnPageSelected()
        {
            this.DelayedTask(500, () =>
            {
                GetInformation();
            });
        }

        #endregion
    }
}
