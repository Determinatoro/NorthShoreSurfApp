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

        public static readonly BindableProperty NumberOfSeatsProperty = BindableProperty.Create(nameof(NumberOfSeats), typeof(int), typeof(CustomStackLayout), null);
        public static readonly BindableProperty AvailableSeatsProperty = BindableProperty.Create(nameof(AvailableSeats), typeof(int), typeof(CustomStackLayout), null);


        public CustomStackLayout()
        {

            
            this.Orientation = StackOrientation.Horizontal;
            this.HeightRequest = 20;
            this.Margin = 0;
            this.Padding = 0;

            TintTransformation tt = new TintTransformation();
            tt.EnableSolidColor = true;
            tt.R = (int)(255);
            tt.G = (int)(255);
            tt.B = (int)(255);
            tt.A = (int)(255);
            var tempList = new List<ITransformation>();
            tempList.Add(tt);


            NumberOfSeats = 4;
            AvailableSeats = 1;
            for (int i = 0; i < NumberOfSeats-AvailableSeats; i++)
            {
                this.Children.Add(new CachedImage()
                {
                    Source = "ic_seat_red.png",
                    HeightRequest = 20,
                    Aspect = Aspect.AspectFit
                });
                Console.WriteLine("Heya");
               
            }
            for (int i = NumberOfSeats-AvailableSeats; i < NumberOfSeats; i++)
            {
                this.Children.Add(new CachedImage()
                {
                    Source = "ic_seat_red.png",
                    HeightRequest = 20,
                    Aspect = Aspect.AspectFit,
                    Transformations = tempList


                });
                Console.WriteLine("YO!!");
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
