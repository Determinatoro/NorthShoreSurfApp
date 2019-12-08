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

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor
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
        #endregion

        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        // OnAppearing
        protected override void OnAppearing()
        {
            //Get userdata from database
            App.DataService.GetData(
                    NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                    false, () => App.DataService.GetUser(AppValuesService.GetUserId().Value),
                    async (response) =>
                    {
                        if (response.Success)
                        {
                            //assign user information
                            UserViewModel.FullName = response.Result.FirstName + " " + response.Result.LastName;
                            UserViewModel.PhoneNo = response.Result.PhoneNo;
                            UserViewModel.Age = response.Result.Age.ToString();
                            response.Result.Gender = response.Result.Gender;
                        }
                        else
                        {
                            //Show error
                            CustomDialog customDialog = new CustomDialog(CustomDialogType.Message, response.ErrorMessage);
                            await PopupNavigation.Instance.PushAsync(customDialog);
                        }
                    });
        }
        #endregion

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        // Buttons clicked event
        private void Button_Clicked(object sender, EventArgs e)
        {
            //Edit user event
            if (sender == btnEdit)
            {
                
            }
            //Log out event
            else if (sender == btnLogOut)
            {
                //Go to welcomepage
                App.Current.MainPage = new WelcomePage();
            }
            //Delete account event
            else if(sender == btnDelAcc)
            {
                //Delete user from database(Only makes user inactive)
                App.DataService.DeleteUser("29711907");
                //Go to welcomepage
                App.Current.MainPage = new WelcomePage();
            }
        }

        #endregion
    }
}