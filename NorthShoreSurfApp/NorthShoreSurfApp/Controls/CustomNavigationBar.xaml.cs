using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    // Back button pressed trigger
    public class BackButtonPressedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<Grid>();
            var frame = parent.FindByName<Frame>("frameBack");
            frame.BackgroundColor = (Color)App.Current.Resources["BarBackgroundPressed"];
        }
    }

    // Back button released trigger
    public class BackButtonReleasedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<Grid>();
            var frame = parent.FindByName<Frame>("frameBack");
            frame.BackgroundColor = Color.Transparent;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavigationBar : ContentView
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty LogoProperty = BindableProperty.Create(nameof(Logo), typeof(ImageSource), typeof(CustomNavigationBar), null);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomNavigationBar), null);
        public static readonly BindableProperty TitleSizeProperty = BindableProperty.Create(nameof(TitleSize), typeof(double), typeof(CustomNavigationBar), null);
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(CustomNavigationBar), null);
        public static readonly BindableProperty BarBackgroundColorProperty = BindableProperty.Create(nameof(BarBackgroundColor), typeof(Color), typeof(CustomNavigationBar), null);
        public static readonly BindableProperty ShowBackButtonProperty = BindableProperty.Create(nameof(ShowBackButton), typeof(bool), typeof(CustomNavigationBar), null);
        public static readonly BindableProperty ShowLogoProperty = BindableProperty.Create(nameof(ShowLogo), typeof(bool), typeof(CustomNavigationBar), null);
        public static readonly BindableProperty ButtonOneIsVisibleProperty = BindableProperty.Create(nameof(ButtonOneIsVisible), typeof(bool), typeof(CustomNavigationBar), false);
        public static readonly BindableProperty ButtonTwoIsVisibleProperty = BindableProperty.Create(nameof(ButtonTwoIsVisible), typeof(bool), typeof(CustomNavigationBar), false);

        public event EventHandler<EventArgs> BackButtonClicked;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomNavigationBar()
        {
            InitializeComponent();

            // Back button clicked
            btnBack.Clicked += (sender, args) =>
            {
                BackButtonClicked?.Invoke(sender, args);
            };
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public CustomImageButton ButtonOne
        {
            get => btn1;
        }

        public CustomImageButton ButtonTwo
        {
            get => btn2;
        }

        public ImageSource Logo
        {
            get { return (ImageSource)GetValue(LogoProperty); }
            set { SetValue(LogoProperty, value); }
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public double TitleSize
        {
            get { return (double)GetValue(TitleSizeProperty); }
            set { SetValue(TitleSizeProperty, value); }
        }
        public Color TitleColor
        {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }
        public Color BarBackgroundColor
        {
            get { return (Color)GetValue(BarBackgroundColorProperty); }
            set { SetValue(BarBackgroundColorProperty, value); }
        }
        public bool ShowBackButton
        {
            get { return (bool)GetValue(ShowBackButtonProperty); }
            set { SetValue(ShowBackButtonProperty, value); }
        }
        public bool ShowLogo
        {
            get { return (bool)GetValue(ShowLogoProperty); }
            set { SetValue(ShowLogoProperty, value); }
        }
        public bool ButtonOneIsVisible
        {
            get { return (bool)GetValue(ButtonOneIsVisibleProperty); }
            set { SetValue(ButtonOneIsVisibleProperty, value); }
        }
        public bool ButtonTwoIsVisible
        {
            get { return (bool)GetValue(ButtonTwoIsVisibleProperty); }
            set { SetValue(ButtonTwoIsVisibleProperty, value); }
        }

        #endregion
    }
}