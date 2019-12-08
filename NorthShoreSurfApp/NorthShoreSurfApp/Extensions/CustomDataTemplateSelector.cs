using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using NorthShoreSurfApp.ViewCells;
using NorthShoreSurfApp.ModelComponents;

namespace NorthShoreSurfApp
{
    public class CustomDataTemplateSelector : DataTemplateSelector
    {


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is CarpoolRide)
                return new DataTemplate(() => new CarpoolRideViewCell().View);
            else if (item is CarpoolRequest)
                return new DataTemplate(() => new CarpoolRequestViewCell().View);
            else
                return null;
        }
    }
}
