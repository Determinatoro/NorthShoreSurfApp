﻿using NorthShoreSurfApp.ViewCells;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomListDialog : Rg.Plugins.Popup.Pages.PopupPage
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(string), typeof(CustomListDialog), null);
        public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(CustomListDialog), (Color)App.Current.Resources["NSSBlue"]);
        public static readonly BindableProperty HeaderSizeProperty = BindableProperty.Create(nameof(HeaderSize), typeof(double), typeof(CustomListDialog), 24.0);
        public static readonly BindableProperty HeaderColorProperty = BindableProperty.Create(nameof(HeaderColor), typeof(Color), typeof(CustomListDialog), Color.White);
        public static readonly BindableProperty HeaderDecorationsProperty = BindableProperty.Create(nameof(HeaderDecorations), typeof(TextDecorations), typeof(CustomListDialog), TextDecorations.None);
        public static readonly BindableProperty HeaderFontAttributesProperty = BindableProperty.Create(nameof(HeaderDecorations), typeof(FontAttributes), typeof(CustomListDialog), FontAttributes.None);
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(CustomListDialog), null);
        public static readonly BindableProperty CloseOnItemTappedProperty = BindableProperty.Create(nameof(CloseOnItemTapped), typeof(bool), typeof(CustomListDialog), true);
        public static readonly BindableProperty ShowSelectButtonProperty = BindableProperty.Create(nameof(ShowSelectButton), typeof(bool), typeof(CustomListDialog), false,
            propertyChanged: (obj, oldValue, newValue) =>
            {
                ((CustomListDialog)obj).SetSelectRowHeight();
            });
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(CustomListDialog), true);
        public static readonly BindableProperty ListSeparatorVisibilityProperty = BindableProperty.Create(nameof(ListSeparatorVisibility), typeof(SeparatorVisibility), typeof(CustomListDialog), SeparatorVisibility.Default);
        public static readonly BindableProperty SelectCommandProperty = BindableProperty.Create(nameof(SelectCommand), typeof(ICommand), typeof(CustomListDialog), null);
        public static readonly BindableProperty RowSelectHeightProperty = BindableProperty.Create(nameof(RowSelectHeight), typeof(GridLength), typeof(CustomListDialog), new GridLength(61.0, GridUnitType.Absolute));

        public event EventHandler<EventArgs> Canceled;
        public event EventHandler<ItemTappedEventArgs> ItemTapped;

        #endregion

        /*****************************************************************/
        // CONSTRUCTOR
        /*****************************************************************/
        #region Constructor

        public CustomListDialog(Func<ViewCell> dataTemplate, IList itemsSource, string header) : this()
        {
            listView.ItemTemplate = new DataTemplate(dataTemplate);
            ItemsSource = itemsSource;
            Header = header;
        }

        public CustomListDialog(DataTemplate dataTemplate, IList itemsSource, string header) : this()
        {
            listView.ItemTemplate = dataTemplate;
            ItemsSource = itemsSource;
            Header = header;
        }

        private CustomListDialog()
        {
            InitializeComponent();

            // Cancel button pressed
            btnCancel.Clicked += async (sender, args) =>
            {
                await PopupNavigation.Instance.PopAsync();
                Canceled?.Invoke(this, new EventArgs());
            };

            // Item tapped
            listView.ItemTapped += async (sender, args) =>
            {
                if (CloseOnItemTapped)
                    await PopupNavigation.Instance.PopAsync();
                ItemTapped?.Invoke(this, args);
            };

            SetSelectRowHeight();
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Unselect item in list
        /// </summary>
        public void UnselectItem()
        {
            SelectedItem = null;
        }
        /// <summary>
        /// Set select row height
        /// </summary>
        private void SetSelectRowHeight()
        {
            if (ShowSelectButton)
                RowSelectHeight = new GridLength(61.0, GridUnitType.Absolute);
            else
                RowSelectHeight = new GridLength(0, GridUnitType.Auto);
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
        public bool CloseOnItemTapped
        {
            get { return (bool)GetValue(CloseOnItemTappedProperty); }
            set { SetValue(CloseOnItemTappedProperty, value); }
        }
        public bool ShowSelectButton
        {
            get { return (bool)GetValue(ShowSelectButtonProperty); }
            set { SetValue(ShowSelectButtonProperty, value); }
        }
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public SeparatorVisibility ListSeparatorVisibility
        {
            get { return (SeparatorVisibility)GetValue(ListSeparatorVisibilityProperty); }
            set { SetValue(ListSeparatorVisibilityProperty, value); }
        }
        public ICommand SelectCommand
        {
            get { return (ICommand)GetValue(SelectCommandProperty); }
            set { SetValue(SelectCommandProperty, value); }
        }
        public GridLength RowSelectHeight
        {
            get { return (GridLength)GetValue(RowSelectHeightProperty); }
            private set { SetValue(RowSelectHeightProperty, value); }
        }

        #endregion
    }
}
