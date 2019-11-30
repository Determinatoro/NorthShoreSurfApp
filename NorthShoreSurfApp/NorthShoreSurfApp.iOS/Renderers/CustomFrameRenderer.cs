using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using NorthShoreSurfApp;
using NorthShoreSurfApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace NorthShoreSurfApp.iOS.Renderers
{
    public class CustomFrameRenderer : FrameRenderer
    {
        /*****************************************************************/
        // OVERRIDE METHODS
        /*****************************************************************/
        #region Override methods

        // LayoutSubviews
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            UpdateLayout();
        }

        // OnElementPropertyChanged
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(CustomFrame.CornerRadius) ||
                e.PropertyName == nameof(CustomFrame))
            {
                UpdateLayout();
            }
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// A very basic way of retrieving same one value for all of the corners
        /// </summary>
        /// <param name="cornerRadius">CornerRadius object</param>
        /// <returns></returns>
        private double RetrieveCommonCornerRadius(CornerRadius cornerRadius)
        {
            var commonCornerRadius = cornerRadius.TopLeft;
            if (commonCornerRadius <= 0)
            {
                commonCornerRadius = cornerRadius.TopRight;
                if (commonCornerRadius <= 0)
                {
                    commonCornerRadius = cornerRadius.BottomLeft;
                    if (commonCornerRadius <= 0)
                    {
                        commonCornerRadius = cornerRadius.BottomRight;
                    }
                }
            }

            return commonCornerRadius;
        }

        /// <summary>
        /// Create a UIRectCorner object from cornerradius
        /// </summary>
        /// <param name="cornerRadius">CornerRadius object</param>
        /// <returns></returns>
        private UIRectCorner RetrieveRoundedCorners(CornerRadius cornerRadius)
        {
            var roundedCorners = default(UIRectCorner);

            if (cornerRadius.TopLeft > 0)
                roundedCorners |= UIRectCorner.TopLeft;

            if (cornerRadius.TopRight > 0)
                roundedCorners |= UIRectCorner.TopRight;

            if (cornerRadius.BottomLeft > 0)
                roundedCorners |= UIRectCorner.BottomLeft;

            if (cornerRadius.BottomRight > 0)
                roundedCorners |= UIRectCorner.BottomRight;

            return roundedCorners;
        }

        /// <summary>
        /// Update layout for frame
        /// </summary>
        private void UpdateLayout()
        {
            var cornerRadius = (Element as CustomFrame)?.CornerRadius;
            if (!cornerRadius.HasValue)
            {
                return;
            }

            var roundedCornerRadius = RetrieveCommonCornerRadius(cornerRadius.Value);
            if (roundedCornerRadius <= 0)
            {
                return;
            }

            var frame = (Element as CustomFrame);

            var roundedCorners = RetrieveRoundedCorners(cornerRadius.Value);

            var path = UIBezierPath.FromRoundedRect(Bounds, roundedCorners, new CGSize(roundedCornerRadius, roundedCornerRadius));

            var mask = new CAShapeLayer
            {
                Path = path.CGPath,
                Frame = Bounds
            };
            NativeView.Layer.Mask = mask;

            var borderLayer = new CAShapeLayer { 
                Path = mask.Path,
                LineWidth = frame.BorderWidth * 2,
                StrokeColor = frame.BorderColor.ToCGColor(),
                FillColor = UIColor.Clear.CGColor,
                Frame = Bounds
            };
            
            NativeView.Layer.AddSublayer(borderLayer);
            // Remove default border width on frame
            NativeView.Layer.BorderWidth = 0;
            NativeView.Layer.BorderColor = UIColor.Clear.CGColor;
        }

        #endregion
    }
}