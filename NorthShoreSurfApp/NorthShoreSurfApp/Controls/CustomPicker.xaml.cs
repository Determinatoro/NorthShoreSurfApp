using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    /*****************************************************************/
    // TRIGGER ACTIONS
    /*****************************************************************/
    #region Trigger actions

    // Picker pressed trigger
    public class CustomPickerPressedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<Grid>();
            var frame = parent.FindByName<Frame>("frameOverlay");
            frame.BackgroundColor = Color.FromHex("44202020");
        }
    }

    // Picker released trigger
    public class CustomPickerReleasedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<Grid>();
            var frame = parent.FindByName<Frame>("frameOverlay");
            frame.BackgroundColor = Color.Transparent;
        }
    }

    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPicker : ContentView
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomImageTextButton), null);
        public static readonly BindableProperty TitleSizeProperty = BindableProperty.Create(nameof(TitleSize), typeof(double), typeof(CustomImageTextButton), 20.0);
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(CustomImageTextButton), Color.Black);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomImageTextButton), null);
        public static readonly BindableProperty TextSizeProperty = BindableProperty.Create(nameof(TitleSize), typeof(double), typeof(CustomImageTextButton), 24.0);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(CustomImageTextButton), Color.Black);
        public static readonly BindableProperty TextMarginProperty = BindableProperty.Create(nameof(TextMargin), typeof(Thickness), typeof(CustomImageTextButton), new Thickness(5, 1, 5, 0));
        public static readonly BindableProperty TextBackgroundColorProperty = BindableProperty.Create(nameof(TextBackgroundColor), typeof(Color), typeof(CustomImageTextButton), Color.White);
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(CustomImageTextButton), null);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CustomImageTextButton), 10.0f);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(CustomImageTextButton), null);

        public event EventHandler<ItemTappedEventArgs> ListItemTapped;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomPicker()
        {
            InitializeComponent();

            // Picker clicked
            buttonOverlay.Clicked += (sender, args) =>
            {
                CustomListDialog customListDialog = new CustomListDialog(
                    ItemTemplate,
                    ItemsSource,
                    string.Format(NorthShoreSurfApp.Resources.AppResources.select_parameter, Title.ToLower())
                    );
                customListDialog.ItemTapped += (sender, args) =>
                {
                    SelectedItem = args.Item;
                    ListItemTapped?.Invoke(this, args);
                };
                PopupNavigation.Instance.PushAsync(customListDialog);
            };
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public object SelectedItem { get; private set; }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public Color TitleColor
        {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }
        public double TitleSize
        {
            get { return (double)GetValue(TitleSizeProperty); }
            set { SetValue(TitleSizeProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public double TextSize
        {
            get { return (double)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        public Color TextBackgroundColor
        {
            get { return (Color)GetValue(TextBackgroundColorProperty); }
            set { SetValue(TextBackgroundColorProperty, value); }
        }
        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }
        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public float CornerRadius
        {
            get { return (float)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        #endregion
    }
}