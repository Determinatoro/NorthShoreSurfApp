using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NorthShoreSurfApp.ViewModels;


namespace NorthShoreSurfApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCarpoolingPage : ContentPage
    {
        public NewCarpoolingPageViewModel NewCarpoolingPageViewModel { get => (NewCarpoolingPageViewModel)this.BindingContext; }
        public NewCarpoolingPage()
        {

            InitializeComponent();
           
        }


    }
}