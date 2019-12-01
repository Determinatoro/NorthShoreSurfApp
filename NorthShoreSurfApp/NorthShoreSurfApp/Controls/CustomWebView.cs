using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp
{
    public class CustomWebView : WebView
    {
        public event EventHandler<EventArgs> Loaded;

        public void FinishedLoading()
        {
            Loaded?.Invoke(this, new EventArgs());
        }
    }
}
