using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using NorthShoreSurfApp.ViewModels;
using Plugin.DeviceOrientation;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCarPage : PopupPage
    {

        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public NewCarViewModel NewCarViewModel { get => (NewCarViewModel)this.BindingContext; }
        public Action<Car> NewCarAdded;
        public Action<Car> CarValuesChanged;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public NewCarPage(Car car = null)
        {
            InitializeComponent();
            // Set car object
            NewCarViewModel.Car = car;

            // Bottom button command
            NewCarViewModel.ButtonCommand = new Command(() =>
            {
                if (!NewCarViewModel.AllDataGiven)
                {
                    // Tell the user to fill out all fields on the page
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_fill_out_all_the_empty_fields);
                    return;
                }

                switch (NewCarViewModel.PageType)
                {
                    case NewCarPageType.Add:
                        CreateCar();
                        break;
                    case NewCarPageType.Edit:
                        EditCar();
                        break;
                }
                
            });
            // Cancel command
            NewCarViewModel.CancelCommand = new Command(async () =>
            {
                await PopupNavigation.Instance.PopAsync();
            });
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Create a new car in the database
        /// </summary>
        private void CreateCar()
        {
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.CreateCar(AppValuesService.UserId.Value, NewCarViewModel.LicensePlate, NewCarViewModel.Color),
                            async (response) =>
                            {
                                // Set opening hours content
                                if (response.Success)
                                {
                                    await PopupNavigation.Instance.PopAsync();
                                    NewCarAdded?.Invoke(response.Result);
                                }
                                // Show error
                                else
                                    this.ShowMessage(response.ErrorMessage);
                            });
        }
        /// <summary>
        /// Edit car
        /// </summary>
        private void EditCar()
        {
            App.DataService.GetData(
                            NorthShoreSurfApp.Resources.AppResources.saving_your_action_please_wait,
                            true,
                            () => App.DataService.UpdateCar(NewCarViewModel.Car.Id, NewCarViewModel.LicensePlate, NewCarViewModel.Color),
                            async (response) =>
                            {
                                // Set opening hours content
                                if (response.Success)
                                {
                                    await PopupNavigation.Instance.PopAsync();
                                    CarValuesChanged?.Invoke(response.Result);
                                }
                                // Show error
                                else
                                    this.ShowMessage(response.ErrorMessage);
                            });
        }

        #endregion
    }
}