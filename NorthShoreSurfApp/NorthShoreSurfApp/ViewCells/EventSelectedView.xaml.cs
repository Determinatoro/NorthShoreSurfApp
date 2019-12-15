using NorthShoreSurfApp.ModelComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventSelectedView : ContentView
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty DeleteCommandProperty = BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(EventSelectedView));
        public static readonly BindableProperty DeleteCommandParameterProperty = BindableProperty.Create(nameof(DeleteCommandParameter), typeof(object), typeof(EventSelectedView));

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public EventSelectedView()
        {
            InitializeComponent();
            DeleteCommandParameter = new Func<Event>(() => { return this.BindingContext as Event; });
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// Command run when pressing the delete button
        /// </summary>
        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            set => SetValue(DeleteCommandProperty, value);
        }
        /// <summary>
        /// Parameter to send with when executing the delete command
        /// </summary>
        public object DeleteCommandParameter
        {
            get => (object)GetValue(DeleteCommandParameterProperty);
            private set => SetValue(DeleteCommandParameterProperty, value);
        }

        #endregion
    }
}