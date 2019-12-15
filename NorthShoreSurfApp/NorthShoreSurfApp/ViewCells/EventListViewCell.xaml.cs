using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventListViewCell : ViewCell
    {
        public EventListViewCell()
        {
            InitializeComponent();
        }
    }
}