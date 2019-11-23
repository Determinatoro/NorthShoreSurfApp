using Microsoft.EntityFrameworkCore;
using NorthShoreSurfApp.Database;
using System;
using System.IO;
using System.Threading.Tasks;
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

            MainPage = new NavigationPage (new CarpoolingPage());

            LocalDataService = DependencyService.Get<ILocalDataService>();
            OrientationService = DependencyService.Get<IOrientationService>();

            LocalDataService.InitializeFiles(true);

            DataService = new NSSDatabaseService<NSSDatabaseContext>();

            // Initialize
            Task.Run(() =>
            {
                DataService.Initialize();
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
