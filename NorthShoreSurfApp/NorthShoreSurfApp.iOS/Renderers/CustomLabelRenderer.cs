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

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace NorthShoreSurfApp.iOS.Renderers
{
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            CustomLabel customLabel = (CustomLabel)Element;

            if (customLabel != null && customLabel.Lines != -1)
            {
                Control.Lines = customLabel.Lines;
                Control.LineBreakMode = UILineBreakMode.TailTruncation;
            }
        }
    }
}