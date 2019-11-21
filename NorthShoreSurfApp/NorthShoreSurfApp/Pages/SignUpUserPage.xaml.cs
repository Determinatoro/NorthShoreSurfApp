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
    public partial class SignUpUserPage : ContentPage
    {
        public SignUpUserModel SignUpUserModel { get => (SignUpUserModel)this.BindingContext; }

        public SignUpUserPage()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            Grid grid = (Grid)Content;
            var safeAreaInset = On<iOS>().SafeAreaInsets();
            grid.Margin = safeAreaInset;

            Title = "Sign up";

            SignUpUserModel.FirstName = "Jakob";
            btnNext.Button.Clicked += Button_Clicked;

            navigationBar.BackButtonClicked += NavigationBar_BackButtonClicked;
        }

        private void NavigationBar_BackButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
                Navigation.RemovePage(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.OrientationService.Portrait();            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (sender == btnNext.Button)
            {


                return;
                /*var page = new SignUpUserPage();
                page.SignUpUserModel.LastName = "TEST";
                await Navigation.PushAsync(page);
                return;*/

                try
                {
                    App.DataService.GetData(
                        NorthShoreSurfApp.Resources.AppResources.getting_data_please_wait,
                        true,
                        () => App.DataService.GetCars(),
                        async (response) =>
                        {
                            await PopupNavigation.Instance.PushAsync(new CustomDialog(CustomDialogType.Message, response.Result.FirstOrDefault().LicensePlate));
                        });
                }
                catch (Exception mes)
                {
                    string test = "";
                }
            }
        }
    }
}