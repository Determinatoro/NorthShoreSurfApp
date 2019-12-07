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
        Message,
        YesNo
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
            string classId = button.ClassId;
            var parent = button.FindParentWithType<Grid>();
            Frame frame = null;

            switch (classId)
            {
                case "Yes":
                    frame = parent.FindByName<Frame>("frameYes");
                    break;
                case "No":
                    frame = parent.FindByName<Frame>("frameNo");
                    break;
                case "Cancel":
                    frame = parent.FindByName<Frame>("frameCancel");
                    break;
            }

            frame.BackgroundColor = (Color)App.Current.Resources["NSSBluePressed"];
        }
    }

    // Cancel released trigger
    public class CustomDialogCancelReleasedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            string classId = button.ClassId;
            var parent = button.FindParentWithType<Grid>();
            Frame frame = null;

            switch (classId)
            {
                case "Yes":
                    frame = parent.FindByName<Frame>("frameYes");
                    break;
                case "No":
                    frame = parent.FindByName<Frame>("frameNo");
                    break;
                case "Cancel":
                    frame = parent.FindByName<Frame>("frameCancel");
                    break;
            }

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
        public event EventHandler<EventArgs> Accepted;
        public event EventHandler<EventArgs> Denied;

        private CustomDialogType customDialogType;
        private ICommand cancelCommand;
        private ICommand yesCommand;
        private ICommand noCommand;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomDialog(CustomDialogType customDialogType, string message) : this()
        {
            Message = message;
            CustomDialogType = customDialogType;

            switch (CustomDialogType)
            {
                case CustomDialogType.Progress:
                    break;
                case CustomDialogType.Message:
                case CustomDialogType.YesNo:
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

            // Cancel button command
            CancelCommand = new Command(async () =>
            {
                await PopupNavigation.Instance.PopAsync();
                Canceled?.Invoke(this, new EventArgs());
            });
            // Yes button command
            YesCommand = new Command(async () =>
            {
                await PopupNavigation.Instance.PopAsync();
                Accepted?.Invoke(this, new EventArgs());
            });
            // No button command
            NoCommand = new Command(async () =>
            {
                await PopupNavigation.Instance.PopAsync();
                Denied?.Invoke(this, new EventArgs());
            });
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public CustomDialogType CustomDialogType
        {
            get => customDialogType; 
            set
            {
                customDialogType = value;
                OnPropertyChanged(nameof(CustomDialogType));
                OnPropertyChanged(nameof(ShowYesNoDialog));
                OnPropertyChanged(nameof(ShowCancelButton));
            }
        }
        public ICommand CancelCommand
        {
            get => cancelCommand;
            private set
            {
                cancelCommand = value;
                OnPropertyChanged(nameof(CancelCommand));
            }
        }
        public ICommand YesCommand
        {
            get => yesCommand;
            private set
            {
                yesCommand = value;
                OnPropertyChanged(nameof(YesCommand));
            }
        }
        public ICommand NoCommand
        {
            get => noCommand;
            private set
            {
                noCommand = value;
                OnPropertyChanged(nameof(NoCommand));
            }
        }
        public bool ShowYesNoDialog
        {
            get => customDialogType == CustomDialogType.YesNo;
        }
        public bool ShowCancelButton
        {
            get
            {
                switch (customDialogType)
                {
                    case CustomDialogType.Message:
                    case CustomDialogType.Progress:
                        return true;
                }

                return false;
            }
        }
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
