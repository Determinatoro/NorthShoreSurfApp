using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NorthShoreSurfApp.Droid.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidScreenService))]
namespace NorthShoreSurfApp.Droid.Services
{
    public class AndroidScreenService : IScreenService
    {
        public void HideStatusBar()
        {
            // Hide status bar
            MainActivity.Instance.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }
        public void ShowStatusBar()
        {
            // Show status bar
            MainActivity.Instance.Window.ClearFlags(WindowManagerFlags.Fullscreen);
        }
        public void Landscape()
        {
            MainActivity.Instance.RequestedOrientation = ScreenOrientation.Landscape;
        }
        public void Portrait()
        {
            MainActivity.Instance.RequestedOrientation = ScreenOrientation.Portrait;
        }
        public void Unspecified()
        {
            MainActivity.Instance.RequestedOrientation = ScreenOrientation.Unspecified;
        }
    }
}