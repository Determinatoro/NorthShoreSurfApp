using LibVLCSharp.Shared;
using Microsoft.EntityFrameworkCore;
using NorthShoreSurfApp.Database;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NorthShoreSurfApp
{
    public partial class App : Application
    {
        public static IFacebookService FacebookService { get; set; }
        public static IFirebaseService FirebaseService { get; set; }
        public static IDataService DataService { get; set; }
        public static ILocalDataService LocalDataService { get; set; }
        public static IOrientationService OrientationService { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new RootTabbedPage();
            Core.Initialize();

            LocalDataService = DependencyService.Get<ILocalDataService>();
            OrientationService = DependencyService.Get<IOrientationService>();

            LocalDataService.InitializeFiles(true);

            DataService = new NSSDatabaseService<NSSDatabaseContext>();
            DataService.Initialize();

            // TEST
            //LocalDataService.SaveValue(nameof(LocalDataKeys.UserId), "1");
            
        }

        protected override void OnStart()
        {
            NorthShoreSurfApp.App.DataService.CreateCarpoolRide(1, new DateTime(2019, 1, 1, 13, 0, 0), "Æblevej", "9000", "Aalborg", 1, 5, 50, new List<ModelComponents.Event>(), "Hook me up!");

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
