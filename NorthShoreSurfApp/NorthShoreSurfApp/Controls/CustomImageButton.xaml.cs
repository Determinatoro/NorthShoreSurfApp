using FFImageLoading.Transformations;
using FFImageLoading.Work;
using System;
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

    // Button pressed trigger
    public class CustomImageButtonTextPressedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<CustomImageButton>();
            var frame = parent.FindByName<Frame>("frameBackground");
            frame.BackgroundColor = parent.BackgroundPressed;
        }
    }

    // Button released trigger
    public class CustomImageButtonReleasedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<CustomImageButton>();
            var frame = parent.FindByName<Frame>("frameBackground");
            frame.BackgroundColor = parent.Background;
        }
    }

    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomImageButton : ContentView
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty BackgroundPressedProperty = BindableProperty.Create(nameof(BackgroundPressed), typeof(Color), typeof(CustomImageButton), Color.Gray);
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Color), typeof(CustomImageButton), Color.White);
        public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(Xamarin.Forms.ImageSource), typeof(CustomImageButton), null);
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(Icon), typeof(Color), typeof(CustomImageButton), Color.White, propertyChanged:
            (b, oldValue, newValue) =>
            {
                var button = (CustomImageButton)b;
                button.IconTransformations = button.GetTransformations();
            });
        public static readonly BindableProperty IconPaddingProperty = BindableProperty.Create(nameof(IconPadding), typeof(Thickness), typeof(CustomImageButton), new Thickness(5));
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CustomImageButton), 10.0f);
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(Thickness), typeof(CustomImageButton), new Thickness(0));
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomImageButton), Color.Black);
        public static readonly BindableProperty IconTransformationsProperty = BindableProperty.Create(nameof(IconTransformations), typeof(List<ITransformation>), typeof(CustomImageButton), null);

        public event EventHandler<EventArgs> Clicked;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomImageButton()
        {
            InitializeComponent();

            // Button clicked
            button.Clicked += (sender, args) =>
            {
                Clicked?.Invoke(this, args);
            };

            IconTransformations = GetTransformations();
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public Color BackgroundPressed
        {
            get { return (Color)GetValue(BackgroundPressedProperty); }
            set { SetValue(BackgroundPressedProperty, value); }
        }
        public Color Background
        {
            get { return (Color)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public Xamarin.Forms.ImageSource Icon
        {
            get { return (Xamarin.Forms.ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public Color IconColor
        {
            get { return (Color)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }
        public float CornerRadius
        {
            get { return (float)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }
        public Thickness IconPadding
        {
            get { return (Thickness)GetValue(IconPaddingProperty); }
            set { SetValue(IconPaddingProperty, value); }
        }
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
        public List<ITransformation> IconTransformations
        {
            get { return (List<ITransformation>)GetValue(IconTransformationsProperty); }
            private set { SetValue(IconTransformationsProperty, value); }
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Set icon color
        /// </summary>
        private List<ITransformation> GetTransformations()
        {
            TintTransformation tt = new TintTransformation();
            tt.EnableSolidColor = true;
            tt.R = (int)(IconColor.R * 255);
            tt.G = (int)(IconColor.G * 255);
            tt.B = (int)(IconColor.B * 255);
            tt.A = (int)(IconColor.A * 255);
            var tempList = new List<ITransformation>();
            tempList.Add(tt);
            return tempList;
        }

        #endregion
    }
}