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

            
            LocalDataService.SaveValue(nameof(LocalDataKeys.UserId), "1");
            


        }

        protected override void OnStart()
        {
            NorthShoreSurfApp.App.DataService.CreateCarpoolRide(1, new DateTime(2019, 1, 1, 13, 0, 0), "Gundorslund", "9000", "Aalborg", "Løkkenvej 1", "9440", "Løkken", 1, 5, 50, new List<ModelComponents.Event>(), "Hook me up!");
            NorthShoreSurfApp.App.DataService.CreateCarpoolRide(1, new DateTime(2019, 5, 6, 10, 0, 0), "Østerå", "9000", "Aalborg", "Løkkenvej 1", "9440", "Løkken", 1, 5, 50, new List<ModelComponents.Event>(), "Hook me up!");
            NorthShoreSurfApp.App.DataService.CreateCarpoolRide(1, new DateTime(2019, 5, 6, 11, 30, 0), "Jomfru Ane Gade", "9000", "Aalborg", "Løkkenvej 1", "9440", "Løkken", 1, 5, 70, new List<ModelComponents.Event>(), "Hook me up!");
            NorthShoreSurfApp.App.DataService.CreateCarpoolRide(1, new DateTime(2019, 8, 9, 13, 0, 0), "Skelagervej", "9000", "Aalborg", "Løkkenvej 1", "9440", "Løkken", 1, 5, 90, new List<ModelComponents.Event>(), "Hook me up!");
            NorthShoreSurfApp.App.DataService.CreateCarpoolRide(1, new DateTime(2019, 1, 1, 17, 30, 0), "Smutvejen", "9000", "Aalborg", "Løkkenvej 1", "9440", "Løkken", 1, 5, 100, new List<ModelComponents.Event>(), "Hook me up!");
            NorthShoreSurfApp.App.DataService.CreateCarpoolRide(1, new DateTime(2019, 10, 5, 19, 0, 0), "Ved Stranden", "9000", "Aalborg", "Løkkenvej 1", "9440", "Løkken", 1, 5, 10, new List<ModelComponents.Event>(), "Hook me up!");
            NorthShoreSurfApp.App.DataService.CreateCarpoolRide(1, new DateTime(2019, 1, 1, 7, 0, 0), "Æblevej", "9000", "Aalborg", "Løkkenvej 1", "9440", "Løkken", 1, 5, 4, new List<ModelComponents.Event>(), "Hook me up!");
            NorthShoreSurfApp.App.DataService.CreateCarpoolRequest(1, new DateTime(2019, 1, 5, 7, 0, 0), new DateTime(2019, 1, 1, 8, 0, 0), "9000", "Aalborg", new List<ModelComponents.Event>());
            NorthShoreSurfApp.App.DataService.CreateCarpoolRequest(1, new DateTime(2019, 2, 6, 10, 0, 0), new DateTime(2019, 1, 1, 11, 0, 0), "9000", "Aalborg", new List<ModelComponents.Event>());
            NorthShoreSurfApp.App.DataService.CreateCarpoolRequest(1, new DateTime(2019, 8, 3, 11, 0, 0), new DateTime(2019, 1, 1, 13, 0, 0), "9000", "Aalborg", new List<ModelComponents.Event>());
            NorthShoreSurfApp.App.DataService.CreateCarpoolRequest(1, new DateTime(2019, 4, 9, 15, 0, 0), new DateTime(2019, 1, 1, 17, 0, 0), "9400", "Nørresundby", new List<ModelComponents.Event>());

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
