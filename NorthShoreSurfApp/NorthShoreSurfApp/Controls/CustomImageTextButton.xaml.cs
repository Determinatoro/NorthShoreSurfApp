using System;
﻿using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;

namespace NorthShoreSurfApp
{
    /*****************************************************************/
    // TRIGGER ACTIONS
    /*****************************************************************/
    #region Trigger actions

    // Button pressed trigger
    public class CustomButtonImageTextPressedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<CustomImageTextButton>();
            var frame = parent.FindByName<Frame>("frame");
            frame.BackgroundColor = parent.BackgroundPressed;
        }
    }

    // Button released trigger
    public class CustomButtonImageTextReleasedTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            var parent = button.FindParentWithType<CustomImageTextButton>();
            var frame = parent.FindByName<Frame>("frame");
            frame.BackgroundColor = parent.Background;
        }
    }

    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomImageTextButton : ContentView
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty BackgroundPressedProperty = BindableProperty.Create(nameof(BackgroundPressed), typeof(Color), typeof(CustomImageTextButton), Color.Gray);
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Color), typeof(CustomImageTextButton), Color.White);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomImageTextButton), null);
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(CustomImageTextButton), Color.Black);
        public static readonly BindableProperty TitleSizeProperty = BindableProperty.Create(nameof(TitleSize), typeof(double), typeof(CustomImageTextButton), 24.0);
        public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(Xamarin.Forms.ImageSource), typeof(CustomImageTextButton), null);
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(CustomImageTextButton), Color.White, propertyChanged:
            (b, oldValue, newValue) =>
            {
                var button = (CustomImageTextButton)b;
                button.IconTransformations = button.GetTransformations();
            });
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CustomImageTextButton), 10.0f);
        public static readonly BindableProperty IconPaddingProperty = BindableProperty.Create(nameof(IconPadding), typeof(Thickness), typeof(CustomImageTextButton), new Thickness(5));
        public static readonly BindableProperty IconTransformationsProperty = BindableProperty.Create(nameof(IconTransformations), typeof(List<ITransformation>), typeof(CustomImageTextButton), null);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomImageTextButton), null);

        public event EventHandler<EventArgs> Clicked;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomImageTextButton()
        {
            InitializeComponent();

            Color color = IconColor;

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
        public Thickness IconPadding
        {
            get { return (Thickness)GetValue(IconPaddingProperty); }
            set { SetValue(IconPaddingProperty, value); }
        }
        public List<ITransformation> IconTransformations
        {
            get { return (List<ITransformation>)GetValue(IconTransformationsProperty); }
            private set { SetValue(IconTransformationsProperty, value); }
        }
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            private set { SetValue(CommandProperty, value); }
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Get transformations for image
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
