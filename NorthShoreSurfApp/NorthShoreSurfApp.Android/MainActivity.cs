using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Facebook;
using Android.Content;
using System.Text;
using System.Security.Cryptography;
using NorthShoreSurfApp.Droid.Service;
using NorthShoreSurfApp.Droid.Services;
using Firebase;
using Firebase.Auth;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Platform.Android;
using Plugin.CurrentActivity;
using Android.Content.Res;
using Plugin.DeviceOrientation;

namespace NorthShoreSurfApp.Droid
{
    [Activity(Label = "NorthShoreSurfApp", Icon = "@mipmap/icon", Theme = "@style/splashscreen", ScreenOrientation = ScreenOrientation.Unspecified, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private AndroidFacebookService facebookService;
        private AndroidFirebaseService firebaseService;
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.SetTheme(Resource.Style.MainTheme);

            Instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CrossCurrentActivity.Current.Activity = this;

            base.OnCreate(savedInstanceState);

            FirebaseApp.InitializeApp(this);

            facebookService = new AndroidFacebookService();
            firebaseService = new AndroidFirebaseService();

            App.FacebookService = facebookService;
            App.FirebaseService = firebaseService;

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            facebookService.CallbackManager.OnActivityResult(requestCode, Convert.ToInt32(resultCode), data);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            DeviceOrientationImplementation.NotifyOrientationChange(newConfig.Orientation);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Pages are in the popup stack
            }
            else
            {
                //base.OnBackPressed();
            }
        }

    }
}