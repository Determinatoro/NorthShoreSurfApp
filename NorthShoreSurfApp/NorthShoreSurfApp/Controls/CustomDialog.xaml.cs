using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    /*****************************************************************/
    // ENUMS
    /*****************************************************************/
    #region Enums

    public enum CustomDialogType
    {
        Progress,
        Message
    }

    #endregion

    /*****************************************************************/
    // TRIGGER ACTIONS
    /*****************************************************************/
    #region Trigger actions

    // Cancel pressed trigger
    public class CustomDialogCancelPressedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<Grid>();
            var frame = parent.FindByName<Frame>("frameCancel");
            frame.BackgroundColor = (Color)App.Current.Resources["NSSBluePressed"];
        }
    }

    // Cancel released trigger
    public class CustomDialogCancelReleasedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<Grid>();
            var frame = parent.FindByName<Frame>("frameCancel");
            frame.BackgroundColor = (Color)App.Current.Resources["NSSBlue"];
        }
    }

    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomDialog : Rg.Plugins.Popup.Pages.PopupPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string), typeof(CustomDialog), null);
        public static readonly BindableProperty MessagePaddingProperty = BindableProperty.Create(nameof(MessagePadding), typeof(Thickness), typeof(CustomDialog), new Thickness(0));
        public static readonly BindableProperty CancelTitleProperty = BindableProperty.Create(nameof(CancelTitle), typeof(string), typeof(CustomDialog), null);

        public event EventHandler<EventArgs> Canceled;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomDialog(CustomDialogType customDialogType, string message) : this()
        {
            this.Message = message;
            this.CustomDialogType = customDialogType;

            switch (CustomDialogType)
            {
                case CustomDialogType.Progress:
                    break;
                case CustomDialogType.Message:
                    activityIndicator.IsVisible = false;
                    MessagePadding = new Thickness(0, 10, 0, 10);
                    break;
            }
        }

        private CustomDialog()
        {
            InitializeComponent();

            // Background pressed
            var tgr = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            tgr.Tapped += async (sender, args) =>
            {
                if (this.CloseWhenBackgroundIsClicked)
                    await PopupNavigation.Instance.PopAsync();
            };
            var views = new View[] { rlBackground, rlBackground2, rlBackground3, rlBackground4 };
            foreach (var view in views)
                view.GestureRecognizers.Add(tgr);

            // Cancel button pressed
            button.Clicked += async (sender, args) =>
            {
                await PopupNavigation.Instance.PopAsync();
                Canceled?.Invoke(this, new EventArgs());
            };
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public CustomDialogType CustomDialogType { get; set; }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public Thickness MessagePadding
        {
            get { return (Thickness)GetValue(MessagePaddingProperty); }
            set { SetValue(MessagePaddingProperty, value); }
        }
        public string CancelTitle
        {
            get { return (string)GetValue(CancelTitleProperty); }
            set { SetValue(CancelTitleProperty, value); }
        }

      

        #endregion
    }
}
