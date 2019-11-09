﻿using Microsoft.EntityFrameworkCore;
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
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", LocalDataFiles.Database); ;
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
        public DbSet<State> States { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarpoolEvent> CarpoolEvents { get; set; }
        public DbSet<CarpoolRequest> CarpoolRequests { get; set; }
        public DbSet<CarpoolConfirmation> CarpoolConfirmations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<CarpoolEvents_Events_Relation> CarpoolEvents_Events_Relations { get; set; }
        public DbSet<CarpoolRequests_Events_Relations> CarpoolRequests_Events_Relations { get; set; }
    }
}
