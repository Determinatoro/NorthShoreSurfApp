using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

namespace NorthShoreSurfApp
{
    public class CarpoolingSeatLayout : WrapLayout
    {
        #region Variables

        public static readonly BindableProperty NumberOfSeatsProperty = BindableProperty.Create(nameof(NumberOfSeats), typeof(int), typeof(CarpoolingSeatLayout), 0,
            propertyChanged: (b, oldValue, newValue) => ((CarpoolingSeatLayout)b).SetLayout());
        public static readonly BindableProperty AvailableSeatsProperty = BindableProperty.Create(nameof(AvailableSeats), typeof(int), typeof(CarpoolingSeatLayout), 0,
            propertyChanged: (b, oldValue, newValue) => ((CarpoolingSeatLayout)b).SetLayout());

        #endregion

        #region Constructor

        public CarpoolingSeatLayout()
        {
            Margin = 0;
            Padding = 0;
        }

        #endregion

        #region Properties

        public int NumberOfSeats
        {
            get => (int)GetValue(NumberOfSeatsProperty);
            set => SetValue(NumberOfSeatsProperty, value);
        }
        public int AvailableSeats
        {
            get => (int)GetValue(AvailableSeatsProperty);
            set => SetValue(AvailableSeatsProperty, value);
        }

        #endregion

        #region Methods

        private void SetLayout()
        {
            // Clear old images
            Children.Clear();

            // Make transformation
            TintTransformation tt = new TintTransformation();
            tt.EnableSolidColor = true;
            tt.R = (int)(255);
            tt.G = (int)(255);
            tt.B = (int)(255);
            tt.A = (int)(255);
            var tempList = new List<ITransformation>();
            tempList.Add(tt);

            // Add red and white seats
            int i;
            for (i = 0; i < NumberOfSeats - AvailableSeats; i++)
            {
                Children.Add(new CachedImage()
                {
                    Source = "ic_seat_red.png",
                    HeightRequest = 20,
                    WidthRequest = 20,
                    Aspect = Aspect.AspectFit
                });
            }
            for (; i < NumberOfSeats; i++)
            {
                Children.Add(new CachedImage()
                {
                    Source = "ic_seat_red.png",
                    HeightRequest = 20,
                    WidthRequest = 20,
                    Aspect = Aspect.AspectFit,
                    Transformations = tempList
                });
            }
        }

        #endregion
    }
}
