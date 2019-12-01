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
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        private CustomWebView CustomWebView { get; set; }

        #endregion

        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        // OnElementChanged
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

        // Draw
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            CustomWebView.FinishedLoading();
        }

        #endregion
    }
}