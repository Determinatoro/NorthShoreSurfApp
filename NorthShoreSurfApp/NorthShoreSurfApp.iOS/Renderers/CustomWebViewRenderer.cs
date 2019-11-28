using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using NorthShoreSurfApp;
using NorthShoreSurfApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace NorthShoreSurfApp.iOS.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var view = Element as CustomWebView;
            if (view == null || NativeView == null)
            {
                return;
            }
            this.ScalesPageToFit = true;
        }
    }
}