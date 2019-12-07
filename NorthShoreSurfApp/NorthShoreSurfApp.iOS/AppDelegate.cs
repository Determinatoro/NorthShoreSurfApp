using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.CoreKit;
using FFImageLoading.Forms.Platform;
using Foundation;
using LibVLCSharp.Forms.Shared;
using NorthShoreSurfApp.iOS.Services;
using UIKit;

namespace NorthShoreSurfApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // Popup dialog library
            Rg.Plugins.Popup.Popup.Init();
            // Cached image library
            CachedImageRenderer.Init();
            // VLC library
            LibVLCSharpFormsRenderer.Init();

            // Initialize xamarin
            global::Xamarin.Forms.Forms.Init();
            // Create instance of firebase service
            App.FirebaseService = new IOSFirebaseService();
            // Set tint color for navigation bar
            UINavigationBar.Appearance.TintColor = UIColor.White;
            // Initialize forms 9 patch
            Forms9Patch.iOS.Settings.Initialize(this);
            // Initialize cached image library
            CachedImageRenderer.InitImageSourceHandler();
            LoadApplication(new App());
            // Configure firebase
            Firebase.Core.App.Configure();
            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            // We need to handle URLs by passing them to their own OpenUrl in order to make the SSO authentication works.
            return ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
        }

        // Details: https://github.com/wcoder/Xamarin.Plugin.DeviceOrientation#xamarinforms-ios
        [Export("application:supportedInterfaceOrientationsForWindow:")]
        public UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, IntPtr forWindow)
        {
            return Plugin.DeviceOrientation.DeviceOrientationImplementation.SupportedInterfaceOrientations;
        }
    }
}
