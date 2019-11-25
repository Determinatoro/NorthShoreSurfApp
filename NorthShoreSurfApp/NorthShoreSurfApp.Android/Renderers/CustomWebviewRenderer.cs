using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
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
        public CustomWebViewRenderer(Context context)
            : base(context)
        {
        }

        static CustomWebView _xwebView = null;
        WebView _webView;

        class CustomWebViewClient : WebViewClient
        {
            public async override void OnPageFinished(WebView view, string url)
            {
                if (_xwebView != null)
                {
                    view.Settings.JavaScriptEnabled = true;
                    await Task.Delay(100);
                    string result = await _xwebView.EvaluateJavaScriptAsync("(function(){return document.body.scrollHeight;})()");
                    _xwebView.HeightRequest = Convert.ToDouble(result);
                }
                base.OnPageFinished(view, url);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            _xwebView = e.NewElement as CustomWebView;
            _webView = Control;

            if (Control != null)
            {
                _webView.SetWebViewClient(new CustomWebViewClient());

                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = false;

                Control.Settings.LoadWithOverviewMode = true;
                Control.Settings.UseWideViewPort = true;
            }
        }
    }
}