﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using NorthShoreSurfApp;
using NorthShoreSurfApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Android.Webkit.WebView;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace NorthShoreSurfApp.Droid.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        private static CustomWebView customWebView = null;
        private WebView webView;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomWebViewRenderer(Context context)
            : base(context)
        {
        }

        #endregion

        /*****************************************************************/
        // CLASSES
        /*****************************************************************/
        #region Classes

        private class CustomWebViewClient : WebViewClient
        {
            public override void OnPageFinished(WebView view, string url)
            {
                base.OnPageFinished(view, url);
            }
        }

        #endregion

        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        // ElementChanged
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            customWebView = e.NewElement as CustomWebView;
            webView = Control;

            if (Control != null)
            {
                webView.SetWebViewClient(new CustomWebViewClient());

                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = false;

                Control.Settings.LoadWithOverviewMode = true;
                Control.Settings.UseWideViewPort = true;
            }
        }

        // Draw
        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            customWebView.FinishedLoading();
        }

        #endregion
    }
}