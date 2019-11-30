using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

namespace NorthShoreSurfApp
{
    public class CustomStackLayout : StackLayout
    {

        public static readonly BindableProperty NumberOfSeatsProperty = BindableProperty.Create(nameof(NumberOfSeats), typeof(int), typeof(CustomStackLayout));
        public static readonly BindableProperty AvailableSeatsProperty = BindableProperty.Create(nameof(AvailableSeats), typeof(int), typeof(CustomStackLayout));

        public CustomStackLayout() : base()
        {

            
            base.Orientation = StackOrientation.Horizontal;
            base.HeightRequest = 20;
            base.Margin = 0;
            base.Padding = 0;

            CachedImage SeatTakenImage = new CachedImage();
            SeatTakenImage.Source = "ic_seat_red.png";
            SeatTakenImage.BackgroundColor = Color.Transparent;
            SeatTakenImage.HeightRequest = 20;
            SeatTakenImage.Aspect = Aspect.AspectFit;

            CachedImage SeatAvailableImage = new CachedImage();
            SeatAvailableImage.Source = "ic_seat_red.png";
            SeatAvailableImage.BackgroundColor = Color.Transparent;
            SeatAvailableImage.HeightRequest = 20;
            SeatAvailableImage.Aspect = Aspect.AspectFit;

            TintTransformation tt = new TintTransformation();
            tt.EnableSolidColor = true;
            tt.R = (int)(255);
            tt.G = (int)(255);
            tt.B = (int)(255);
            tt.A = (int)(255);
            var tempList = new List<ITransformation>();
            tempList.Add(tt);

            SeatAvailableImage.Transformations = tempList;

            for (int i = 0; i < NumberOfSeats; i++)
            {
                base.Children.Add(SeatTakenImage);
            }
            for (int i = NumberOfSeats-AvailableSeats; i < NumberOfSeats; i++)
            {
                base.Children.Add(SeatAvailableImage);
            }
        }

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

        

    }



    
    
}
