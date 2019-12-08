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

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public NewCarPage()
        {
            InitializeComponent();

            // Create command
            NewCarViewModel.CreateCommand = new Command(() =>
            {
                if (!NewCarViewModel.AllDataGiven)
                {
                    // Tell the user to fill out all fields on the page
                    this.ShowMessage(NorthShoreSurfApp.Resources.AppResources.please_fill_out_all_the_empty_fields);
                    return;
                }

                CreateCar();
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
                            NorthShoreSurfApp.Resources.AppResources.creating_car_please_wait,
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

        #endregion
    }
}