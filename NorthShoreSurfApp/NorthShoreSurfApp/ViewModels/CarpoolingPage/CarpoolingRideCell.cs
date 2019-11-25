using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace NorthShoreSurfApp.ViewModels.CarpoolingPage
{
    public class RideCell : ViewCell
    {
        public RideCell()
        {
            StackLayout cellWrapper = new StackLayout();
            StackLayout horizontalLayout = new StackLayout();
            CustomTextBox pickUpTextBox = new CustomTextBox();
            CustomTextBox destinationTextBox = new CustomTextBox();
            CustomTextBox dateAndPrice = new CustomTextBox();

            // Setting Bindings
            pickUpTextBox.SetBinding(CustomTextBox.TextProperty, "PickUp");
            destinationTextBox.SetBinding(CustomTextBox.TextProperty, "Destination");
            dateAndPrice.SetBinding(CustomTextBox.TextProperty, "DateAndPrice");

            // Properties for design
            cellWrapper.BackgroundColor = Color.FromHex("#2C3B4E");
            horizontalLayout.Orientation = StackOrientation.Horizontal;

            // Add Views
            horizontalLayout.Children.Add(pickUpTextBox);
            horizontalLayout.Children.Add(destinationTextBox);
            horizontalLayout.Children.Add(dateAndPrice);
            cellWrapper.Children.Add(horizontalLayout);
            View = cellWrapper;
        }
    }
}
