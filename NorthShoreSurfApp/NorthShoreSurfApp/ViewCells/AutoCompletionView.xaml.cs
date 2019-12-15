using DurianCode.PlacesSearchBar;
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
    public partial class AutoCompletionView : ContentView
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(AutoCompletionView));
        public static readonly BindableProperty SelectedCommandParameterProperty = BindableProperty.Create(nameof(SelectedCommandParameter), typeof(object), typeof(AutoCompletionView));

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public AutoCompletionView()
        {
            InitializeComponent();

            var func = new Func<AutoCompletePrediction>(() =>
            {
                return this.BindingContext as AutoCompletePrediction;
            });
            SelectedCommandParameter = func;
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// Command run when pressing the accept icon button
        /// </summary>
        public ICommand SelectedCommand
        {
            get => (ICommand)GetValue(SelectedCommandProperty);
            set => SetValue(SelectedCommandProperty, value);
        }
        /// <summary>
        /// Parameter to send with when executing the accept command
        /// </summary>
        public object SelectedCommandParameter
        {
            get => (object)GetValue(SelectedCommandParameterProperty);
            set => SetValue(SelectedCommandParameterProperty, value);
        }

        #endregion
    }
}