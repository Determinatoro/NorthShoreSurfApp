using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(true)]
    public partial class RootTabbedPage : CustomTabbedPage
    {
        public RootTabbedPage()
        {
            InitializeComponent();
            // Set toolbar placement for Android
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            // Set tab text and icons colors
            Color barIconsAndTextColor = (Color)App.Current.Resources["NSSBlue"];
            UnselectedTabColor = barIconsAndTextColor;
            SelectedTabColor = barIconsAndTextColor;
            // Is guest
            if (AppValuesService.IsGuest)
            {
                // Remove user page
                Page page = Children.FirstOrDefault(x => ((NavigationPage)x).RootPage is UserPage);
                Children.Remove(page);
                // Remove carpooling page
                page = Children.FirstOrDefault(x => ((NavigationPage)x).RootPage is CarpoolingPage);
                Children.Remove(page);
                // Show user not logged in page at carpooling tab
                CustomNavigationPage newPage = new CustomNavigationPage(new WelcomePage(ViewModels.WelcomePageContentSite.UserNotLoggedIn));
                newPage.IconSelectedSource = "ic_carpooling_selected.png";
                newPage.IconUnselectedSource = "ic_carpooling.png";
                newPage.Title = NorthShoreSurfApp.Resources.AppResources.carpool;
                Children.Add(newPage);
            }
        }
    }
}