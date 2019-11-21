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
        public static readonly BindableProperty TextSizeProperty = BindableProperty.Create(nameof(TitleSize), typeof(double), typeof(CustomImageTextButton), 20.0);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(CustomImageTextButton), Color.Black);
        public static readonly BindableProperty TextBackgroundColorProperty = BindableProperty.Create(nameof(TextBackgroundColor), typeof(Color), typeof(CustomImageTextButton), Color.White);
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(CustomImageTextButton), null);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CustomImageTextButton), 10.0f);

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomPicker()
        {
            InitializeComponent();
        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

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

        #endregion
    }
}