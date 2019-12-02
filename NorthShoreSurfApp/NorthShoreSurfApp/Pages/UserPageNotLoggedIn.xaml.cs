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
    public partial class UserPageNotLoggedIn : ContentPage
    {
        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor
        public UserPageNotLoggedIn()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();

            // Click events
            btnSignUp.Clicked += Button_Clicked;
            btnLogIn.Clicked += Button_Clicked;
        }
        #endregion

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        // Buttons clicked event
        private void Button_Clicked(object sender, EventArgs e)
        {
            //Sign up event
            if (sender == btnSignUp)
            {

            }
            //Log in event
            else if (sender == btnLogIn)
            {

            }
        }

        #endregion
    }
}