using Rg.Plugins.Popup.Pages;
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
    public partial class CustomTableDialog : PopupPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(string), typeof(CustomDialog), null);
        public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(CustomDialog), (Color)App.Current.Resources["NSSBlue"]);
        public static readonly BindableProperty HeaderSizeProperty = BindableProperty.Create(nameof(HeaderSize), typeof(double), typeof(CustomDialog), 24.0);
        public static readonly BindableProperty HeaderColorProperty = BindableProperty.Create(nameof(HeaderColor), typeof(Color), typeof(CustomDialog), Color.White);
        public static readonly BindableProperty HeaderDecorationsProperty = BindableProperty.Create(nameof(HeaderDecorations), typeof(TextDecorations), typeof(CustomDialog), TextDecorations.None);
        public static readonly BindableProperty HeaderFontAttributesProperty = BindableProperty.Create(nameof(HeaderDecorations), typeof(FontAttributes), typeof(CustomDialog), FontAttributes.None);
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(CustomDialog), null);

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomTableDialog()
        {
            InitializeComponent();


        }

        #endregion

        /*****************************************************************/
        // PROPERTIES
        /*****************************************************************/
        #region Properties

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public Color HeaderBackgroundColor
        {
            get { return (Color)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }
        public double HeaderSize
        {
            get { return (double)GetValue(HeaderSizeProperty); }
            set { SetValue(HeaderSizeProperty, value); }
        }
        public Color HeaderColor
        {
            get { return (Color)GetValue(HeaderColorProperty); }
            set { SetValue(HeaderColorProperty, value); }
        }
        public TextDecorations HeaderDecorations
        {
            get { return (TextDecorations)GetValue(HeaderDecorationsProperty); }
            set { SetValue(HeaderDecorationsProperty, value); }
        }
        public FontAttributes HeaderFontAttributes
        {
            get { return (FontAttributes)GetValue(HeaderFontAttributesProperty); }
            set { SetValue(HeaderFontAttributesProperty, value); }
        }
        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        #endregion
    }
}