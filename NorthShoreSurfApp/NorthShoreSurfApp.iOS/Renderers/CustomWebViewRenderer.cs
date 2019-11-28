using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
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
        #region Variables

        private CustomWebView CustomWebView { get; set; }

        #endregion

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            CustomWebView = Element as CustomWebView;
            if (CustomWebView == null || NativeView == null)
            {
                return;
            }
            this.ScalesPageToFit = true;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            CustomWebView.FinishedLoading();
        }
    }
}