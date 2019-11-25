using FFImageLoading.Forms;
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
    // ENUMS
    /*****************************************************************/
    #region Enums

    public enum ToggleType
    {
        Left,
        Right
    }

    #endregion

    /*****************************************************************/
    // EVENTARGS
    /*****************************************************************/
    #region EventArgs

    public class CustomToggleEventArgs : EventArgs
    {
        public ToggleType SelectedToggleType { get; set; }

        public CustomToggleEventArgs(ToggleType toggleType)
        {
            SelectedToggleType = toggleType;
        }
    }

    #endregion

    /*****************************************************************/
    // TRIGGER ACTIONS
    /*****************************************************************/
    #region Trigger actions

    // Button pressed trigger
    public class CustomToggleButtonPressedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<CustomToggleButton>();
            string classId = button.ClassId;
            Frame frame = parent.FindByName<Frame>($"frameSelected{classId}");

            string enumName = Enum.GetName(typeof(ToggleType), parent.SelectedToggleType);
            frame.BackgroundColor = classId == enumName ? parent.BackgroundSelectedPressed : parent.BackgroundUnselectedPressed;
        }
    }

    // Button released trigger
    public class CustomToggleButtonReleasedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<CustomToggleButton>();
            string classId = button.ClassId;
            Frame frame = parent.FindByName<Frame>($"frameSelected{classId}");

            string enumName = Enum.GetName(typeof(ToggleType), parent.SelectedToggleType);
            frame.BackgroundColor = classId == enumName ? parent.BackgroundSelected : Color.Transparent;
        }
    }

    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomToggleButton : ContentView
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty TitleLeftProperty = BindableProperty.Create(nameof(TitleLeft), typeof(string), typeof(CustomToggleButton), null);
        public static readonly BindableProperty TitleLeftSizeProperty = BindableProperty.Create(nameof(TitleLeftSize), typeof(double), typeof(CustomToggleButton), null);
        public static readonly BindableProperty TitleRightProperty = BindableProperty.Create(nameof(TitleRight), typeof(string), typeof(CustomToggleButton), null);
        public static readonly BindableProperty TitleRightSizeProperty = BindableProperty.Create(nameof(TitleRightSize), typeof(double), typeof(CustomToggleButton), null);
        public static readonly BindableProperty IconLeftProperty = BindableProperty.Create(nameof(IconLeft), typeof(Xamarin.Forms.ImageSource), typeof(CustomToggleButton), null);
        public static readonly BindableProperty IconLeftPaddingProperty = BindableProperty.Create(nameof(IconLeftPadding), typeof(Thickness), typeof(CustomToggleButton), new Thickness(5));
        public static readonly BindableProperty IconRightProperty = BindableProperty.Create(nameof(IconRight), typeof(Xamarin.Forms.ImageSource), typeof(CustomToggleButton), null);
        public static readonly BindableProperty IconRightPaddingProperty = BindableProperty.Create(nameof(IconRightPadding), typeof(Thickness), typeof(CustomToggleButton), new Thickness(5));
        public static readonly BindableProperty IconRightTransformationsProperty = BindableProperty.Create(nameof(IconRightTransformations), typeof(List<ITransformation>), typeof(CustomToggleButton), null);
        public static readonly BindableProperty IconLeftTransformationsProperty = BindableProperty.Create(nameof(IconLeftTransformations), typeof(List<ITransformation>), typeof(CustomToggleButton), null);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(CustomToggleButton), new CornerRadius(10), propertyChanged:
            (b, oldValue, newValue) =>
            {
                var button = (CustomToggleButton)b;
                button.SetCornerRadius();
            });
        public static readonly BindableProperty CornerRadiusLeftProperty = BindableProperty.Create(nameof(CornerRadiusLeft), typeof(CornerRadius), typeof(CustomToggleButton), new CornerRadius(10, 0, 10, 0));
        public static readonly BindableProperty CornerRadiusRightProperty = BindableProperty.Create(nameof(CornerRadiusRight), typeof(CornerRadius), typeof(CustomToggleButton), new CornerRadius(0, 10, 0, 10));

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomToggleButton), Color.Black);
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(float), typeof(CustomToggleButton), 1.0f, propertyChanged:
            (b, oldValue, newValue) =>
            {
                var button = (CustomToggleButton)b;
                var value = (double)newValue;
                button.ContentPadding = new Thickness(value);
            });
        public static readonly BindableProperty ContentPaddingProperty = BindableProperty.Create(nameof(ContentPadding), typeof(Thickness), typeof(CustomToggleButton), new Thickness(1));

        public static readonly BindableProperty BackgroundUnselectedProperty = BindableProperty.Create(nameof(BackgroundUnselected), typeof(Color), typeof(CustomToggleButton), Color.White);
        public static readonly BindableProperty BackgroundSelectedProperty = BindableProperty.Create(nameof(BackgroundSelected), typeof(Color), typeof(CustomToggleButton), Color.Black, propertyChanged:
            (b, oldValue, newValue) =>
            {
                var button = (CustomToggleButton)b;
                button.UpdateLayout();
            });
        public static readonly BindableProperty BackgroundUnselectedPressedProperty = BindableProperty.Create(nameof(BackgroundUnselectedPressed), typeof(Color), typeof(CustomToggleButton), Color.Gray);
        public static readonly BindableProperty BackgroundSelectedPressedProperty = BindableProperty.Create(nameof(BackgroundSelectedPressed), typeof(Color), typeof(CustomToggleButton), Color.DarkGray);
        public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(CustomToggleButton), Color.White, propertyChanged:
            (b, oldValue, newValue) =>
            {
                var button = (CustomToggleButton)b;
                button.UpdateLayout();
            });
        public static readonly BindableProperty UnselectedColorProperty = BindableProperty.Create(nameof(UnselectedColor), typeof(Color), typeof(CustomToggleButton), Color.Black, propertyChanged:
            (b, oldValue, newValue) =>
            {
                var button = (CustomToggleButton)b;
                button.UpdateLayout();
            });
        public static readonly BindableProperty IconLeftWidthProperty = BindableProperty.Create(nameof(IconLeftWidth), typeof(GridLength), typeof(CustomToggleButton), new GridLength(60, GridUnitType.Absolute));
        public static readonly BindableProperty IconRightWidthProperty = BindableProperty.Create(nameof(IconRightWidth), typeof(GridLength), typeof(CustomToggleButton), new GridLength(60, GridUnitType.Absolute));

        public event EventHandler<CustomToggleEventArgs> Toggled;

        private ToggleType selectedToggleType;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomToggleButton()
        {
            InitializeComponent();

            btnLeft.Clicked += Button_Clicked;
            btnRight.Clicked += Button_Clicked;
            
            SelectedToggleType = ToggleType.Left;
            SetCornerRadius();
            imageLeft.BitmapOptimizations = false;
        }

        #endregion

        /*****************************************************************/
        // EVENTS
        /*****************************************************************/
        #region Events

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            // ClassId
            string classId = button.ClassId;

            // Get the selected toggle type
            if (Enum.TryParse(typeof(ToggleType), classId, true, out object toggleType))
            {
                SelectedToggleType = (ToggleType)toggleType;
            }
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public ToggleType SelectedToggleType
        {
            get => selectedToggleType;
            set
            {
                bool toggleTypeChanged = false;
                if (selectedToggleType != value)
                    toggleTypeChanged = true;
                selectedToggleType = value;

                UpdateLayout(toggleTypeChanged);
            }
        }

        public string TitleLeft
        {
            get { return (string)GetValue(TitleLeftProperty); }
            set { SetValue(TitleLeftProperty, value); }
        }
        public double TitleLeftSize
        {
            get { return (double)GetValue(TitleLeftSizeProperty); }
            set { SetValue(TitleLeftSizeProperty, value); }
        }
        public string TitleRight
        {
            get { return (string)GetValue(TitleRightProperty); }
            set { SetValue(TitleRightProperty, value); }
        }
        public double TitleRightSize
        {
            get { return (double)GetValue(TitleRightSizeProperty); }
            set { SetValue(TitleRightSizeProperty, value); }
        }
        public Xamarin.Forms.ImageSource IconLeft
        {
            get { return (Xamarin.Forms.ImageSource)GetValue(IconLeftProperty); }
            set { SetValue(IconLeftProperty, value); }
        }
        public Thickness IconLeftPadding
        {
            get { return (Thickness)GetValue(IconLeftPaddingProperty); }
            set { SetValue(IconLeftPaddingProperty, value); }
        }
        public Xamarin.Forms.ImageSource IconRight
        {
            get { return (Xamarin.Forms.ImageSource)GetValue(IconRightProperty); }
            set { SetValue(IconRightProperty, value); }
        }
        public Thickness IconRightPadding
        {
            get { return (Thickness)GetValue(IconRightPaddingProperty); }
            set { SetValue(IconRightPaddingProperty, value); }
        }
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
        public float BorderThickness
        {
            get { return (float)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }
        public Color BackgroundUnselected
        {
            get { return (Color)GetValue(BackgroundUnselectedProperty); }
            set { SetValue(BackgroundUnselectedProperty, value); }
        }
        public Color BackgroundSelected
        {
            get { return (Color)GetValue(BackgroundSelectedProperty); }
            set { SetValue(BackgroundSelectedProperty, value); }
        }
        public Color BackgroundSelectedPressed
        {
            get { return (Color)GetValue(BackgroundSelectedPressedProperty); }
            set { SetValue(BackgroundSelectedPressedProperty, value); }
        }
        public Color BackgroundUnselectedPressed
        {
            get { return (Color)GetValue(BackgroundUnselectedPressedProperty); }
            set { SetValue(BackgroundUnselectedPressedProperty, value); }
        }
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        public Color UnselectedColor
        {
            get { return (Color)GetValue(UnselectedColorProperty); }
            set { SetValue(UnselectedColorProperty, value); }
        }
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public CornerRadius CornerRadiusLeft
        {
            get { return (CornerRadius)GetValue(CornerRadiusLeftProperty); }
            private set { SetValue(CornerRadiusLeftProperty, value); }
        }
        public CornerRadius CornerRadiusRight
        {
            get { return (CornerRadius)GetValue(CornerRadiusRightProperty); }
            private set { SetValue(CornerRadiusRightProperty, value); }
        }
        public List<ITransformation> IconRightTransformations
        {
            get { return (List<ITransformation>)GetValue(IconRightTransformationsProperty); }
            private set { SetValue(IconRightTransformationsProperty, value); }
        }
        public List<ITransformation> IconLeftTransformations
        {
            get { return (List<ITransformation>)GetValue(IconLeftTransformationsProperty); }
            private set { SetValue(IconLeftTransformationsProperty, value); }
        }
        public GridLength IconLeftWidth
        {
            get { return (GridLength)GetValue(IconLeftWidthProperty); }
            set { SetValue(IconLeftWidthProperty, value); }
        }
        public GridLength IconRightWidth
        {
            get { return (GridLength)GetValue(IconRightWidthProperty); }
            set { SetValue(IconRightWidthProperty, value); }
        }
        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            private set { SetValue(ContentPaddingProperty, value); }
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        private void SetCornerRadius()
        {
            var newValue = CornerRadius;
            CornerRadiusLeft = new CornerRadius(newValue.TopLeft, 0, newValue.BottomLeft, 0);
            CornerRadiusRight = new CornerRadius(0, newValue.TopRight, 0, newValue.BottomRight);
        }

        private void UpdateLayout(bool toggleTypeChanged = false)
        {
            // Reset selected frames background
            frameSelectedLeft.BackgroundColor = Color.Transparent;
            frameSelectedRight.BackgroundColor = Color.Transparent;
            // Reset title colors
            lblTitleLeft.TextColor = UnselectedColor;
            lblTitleRight.TextColor = UnselectedColor;
            // Reset icon colors
            IconRightTransformations = GetTransformations(UnselectedColor);
            IconLeftTransformations = GetTransformations(UnselectedColor);

            string enumName = Enum.GetName(typeof(ToggleType), SelectedToggleType);

            // Set color background selected color
            Frame frame = this.FindByName<Frame>($"frameSelected{enumName}");
            frame.BackgroundColor = BackgroundSelected;
            // Set title selected color
            Label label = this.FindByName<Label>($"lblTitle{enumName}");
            label.TextColor = SelectedColor;
            // Set icon color
            switch (SelectedToggleType)
            {
                case ToggleType.Left:
                    IconLeftTransformations = GetTransformations(SelectedColor);
                    break;
                case ToggleType.Right:
                    IconRightTransformations = GetTransformations(SelectedColor);
                    break;
            }

            if (!toggleTypeChanged)
                return;
            // Invoke toggle event
            Toggled?.Invoke(this, new CustomToggleEventArgs(SelectedToggleType));
        }

        /// <summary>
        /// Set icon color
        /// </summary>
        private List<ITransformation> GetTransformations(Color color)
        {
            TintTransformation tt = new TintTransformation();
            tt.EnableSolidColor = true;
            tt.R = (int)(color.R * 255);
            tt.G = (int)(color.G * 255);
            tt.B = (int)(color.B * 255);
            tt.A = (int)(color.A * 255);
            var tempList = new List<ITransformation>();
            tempList.Add(tt);
            return tempList;
        }

        #endregion
    }
}