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
    public partial class CarpoolConfirmationViewCell : ViewCell
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty AcceptCommandProperty = BindableProperty.Create(nameof(AcceptCommand), typeof(ICommand), typeof(CarpoolConfirmationViewCell));
        public static readonly BindableProperty AcceptCommandParameterProperty = BindableProperty.Create(nameof(AcceptCommandParameter), typeof(object), typeof(CarpoolConfirmationViewCell));
        public static readonly BindableProperty DenyCommandProperty = BindableProperty.Create(nameof(DenyCommand), typeof(ICommand), typeof(CarpoolConfirmationViewCell));
        public static readonly BindableProperty DenyCommandParameterProperty = BindableProperty.Create(nameof(DenyCommandParameter), typeof(object), typeof(CarpoolConfirmationViewCell));

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CarpoolConfirmationViewCell()
        {
            InitializeComponent();

            var func = new Func<CarpoolConfirmation>(() => 
            { 
                return this.BindingContext as CarpoolConfirmation; 
            });
            AcceptCommandParameter = func;
            DenyCommandParameter = func;
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        /// <summary>
        /// Command run when pressing the accept icon button
        /// </summary>
        public ICommand AcceptCommand
        {
            get => (ICommand)GetValue(AcceptCommandProperty);
            set => SetValue(AcceptCommandProperty, value);
        }
        /// <summary>
        /// Parameter to send with when executing the accept command
        /// </summary>
        public object AcceptCommandParameter
        {
            get => (object)GetValue(AcceptCommandParameterProperty);
            set => SetValue(AcceptCommandParameterProperty, value);
        }

        /// <summary>
        /// Command run when pressing the accept icon button
        /// </summary>
        public ICommand DenyCommand
        {
            get => (ICommand)GetValue(DenyCommandProperty);
            set => SetValue(DenyCommandProperty, value);
        }
        /// <summary>
        /// Parameter to send with when executing the accept command
        /// </summary>
        public object DenyCommandParameter
        {
            get => (object)GetValue(DenyCommandParameterProperty);
            set => SetValue(DenyCommandParameterProperty, value);
        }

        #endregion
    }
}