using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using NorthShoreSurfApp.iOS.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(IOSOrientationService))]
namespace NorthShoreSurfApp.iOS.Services
{
    public class IOSOrientationService : IOrientationService
    {
        public void Landscape()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));
        }

        public void Portrait()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
        }

        public void Unspecified()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Unknown), new NSString("orientation"));
        }
    }
}