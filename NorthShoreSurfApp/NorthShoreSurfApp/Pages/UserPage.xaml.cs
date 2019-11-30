using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewCells;
using NorthShoreSurfApp.ViewModels;
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
    public partial class UserPage : ContentPage
    {
        public UserViewModel UserViewModel { get => (UserViewModel)this.BindingContext; }
        public UserPage()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();

            // Click events
            btnEdit.Clicked += Button_Clicked;
            btnLogOut.Clicked += Button_Clicked;
            btnDelAcc.Clicked += Button_Clicked;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            App.DataService.GetUser("29711907");
            App.DataService.GetData(
                    NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                    false, () => App.DataService.GetUser("29711907"),
                    async (response) =>
                    {
                        if (response.Success)
                        {
                            UserViewModel.FullName = response.Result.FirstName + " " + response.Result.LastName;
                            UserViewModel.PhoneNo = response.Result.PhoneNo;
                            UserViewModel.Age = response.Result.Age.ToString();
                            response.Result.Gender = response.Result.Gender;
                        }
                        else
                        {
                            CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                            await PopupNavigation.Instance.PushAsync(customDialog);
                        }
                    });

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            if (sender == btnEdit)
            {

            }
            else if (sender == btnLogOut)
            {

            }
            else if(sender == btnDelAcc)
            {
                App.DataService.DeleteUser("29711907");
            }
        }
    }
}