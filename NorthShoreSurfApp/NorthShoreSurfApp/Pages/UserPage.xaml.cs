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
        string PhoneNo;
        public UserPage(string phoneNo)
        {
            PhoneNo = phoneNo;
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();

            // Click events
            btnEdit.Clicked += Button_Clicked;
            btnLogOut.Clicked += Button_Clicked;
            btnDelAcc.Clicked += Button_Clicked;

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (sender == btnEdit)
            {
                App.DataService.GetData(
                       NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                       true,
                       () => App.DataService.GetUser(PhoneNo),
                       async (response) =>
                       {
                           if (response.Success)
                           {
                               await Navigation.PopAsync();
                           }
                           else
                           {
                               CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                               await PopupNavigation.Instance.PushAsync(customDialog);
                           }

                           await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, response.Result.FirstName));
                       });
            }
            else if (sender == btnLogOut)
            {
                App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                        true,
                        () => App.DataService.SignUpUser("Emil", "Danielsen", "29711907", 21, 1),
                        async (response) =>
                        {
                            if (response.Success)
                            {
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                await PopupNavigation.Instance.PushAsync(customDialog);
                            }

                            await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, response.Result.PhoneNo));
                        });
            }
            else if(sender == btnDelAcc)
            {
                App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                        true,
                        () => App.DataService.DeleteUser("29711907"),
                        async (response) =>
                        {
                            if (response.Success)
                            {
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                                await PopupNavigation.Instance.PushAsync(customDialog);
                            }
                        });
            }
        }
    }
}