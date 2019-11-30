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

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            Color barIconsAndTextColor = (Color)App.Current.Resources["NSSBlue"];
            UnselectedTabColor = barIconsAndTextColor;
            SelectedTabColor = barIconsAndTextColor;           
        }
    }
}