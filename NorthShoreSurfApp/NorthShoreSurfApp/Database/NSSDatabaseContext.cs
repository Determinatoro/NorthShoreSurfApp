using Microsoft.EntityFrameworkCore;
using NorthShoreSurfApp.Database;
using NorthShoreSurfApp.ModelComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp
{
    public class NSSDatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = "";
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library", "DataFiles", LocalDataFiles.Database);
                    break;
                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), LocalDataFiles.Database);
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
            }
            // Specify that we will use sqlite and the path of the database here
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarpoolRide> CarpoolRides { get; set; }
        public DbSet<CarpoolRequest> CarpoolRequests { get; set; }
        public DbSet<CarpoolConfirmation> CarpoolConfirmations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<CarpoolRides_Events_Relation> CarpoolRides_Events_Relations { get; set; }
        public DbSet<CarpoolRequests_Events_Relation> CarpoolRequests_Events_Relations { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<OpeningHour> OpeningHours { get; set; }
    }
}
