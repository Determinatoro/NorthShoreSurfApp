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
        public static IScreenService ScreenService { get; set; }

        public static event EventHandler<EventArgs> Started;
        public static event EventHandler<EventArgs> Slept;
        public static event EventHandler<EventArgs> Resumed;

        public App()
        {
            InitializeComponent();

            Core.Initialize();

            LocalDataService = DependencyService.Get<ILocalDataService>();
            ScreenService = DependencyService.Get<IScreenService>();

            LocalDataService.InitializeFiles(true);

            DataService = new NSSDatabaseService<NSSDatabaseContext>();
            DataService.Initialize();

            // TEST User logged in
            LocalDataService.SaveValue(nameof(LocalDataKeys.UserId), "1");

            // Check if a user is logged in
            var userId = LocalDataService.GetValue(nameof(LocalDataKeys.UserId));

            if (userId != null && userId != string.Empty)
            {
                // Set main page to tab
                MainPage = new RootTabbedPage();
            }
            else
            {
                // Set main page to welcome page
                MainPage = new WelcomePage();
            }
        }

        protected override void OnStart()
        {
            Started?.Invoke(this, new EventArgs());
        }

        protected override void OnSleep()
        {
            Slept?.Invoke(this, new EventArgs());
        }

        protected override void OnResume()
        {
            Resumed?.Invoke(this, new EventArgs());
        }
    }
}
