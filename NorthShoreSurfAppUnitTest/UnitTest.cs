using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthShoreSurfApp;
using NorthShoreSurfApp.Database;
using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewModels;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace NorthShoreSurfAppUnitTest
{
    [TestClass]
    public class UnitTest
    {
        /*****************************************************************/
        // VARIABLES
        /*****************************************************************/
        #region Variables

        private static ResourceManager ResourceManager { get; set; }

        #endregion

        /*****************************************************************/
        // SETUP
        /*****************************************************************/
        #region Setup

        [AssemblyInitialize]
        public static void TestSetup(TestContext testContext)
        {
            string languageCode = "en-US";
            Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
            ResourceManager = new ResourceManager("NorthShoreSurfApp.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
        }

        #endregion

        /*****************************************************************/
        // METHODS
        /*****************************************************************/
        #region Methods

        /// <summary>
        /// Get localized string from key
        /// </summary>
        /// <param name="key">Key for a localized string</param>
        /// <returns>A localized string</returns>
        private string GetLocalizedString(string key)
        {
            return ResourceManager.GetString(key, CultureInfo.CurrentCulture);
        }

        #endregion

        #region Services 

        [TestMethod]
        public async Task DatabaseService_GetUsers_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetUsers();
                // Assert
                Assert.IsTrue(response.Result.Count > 0);
            }
        }

        #endregion

        /*****************************************************************/
        // MODEL COMPONENTS
        /*****************************************************************/
        #region Model Components

        #endregion

        /*****************************************************************/
        // VIEW MODELS
        /*****************************************************************/
        #region ViewModels

        /****************************************************/
        // HOME VIEW MODEL
        /****************************************************/
        #region HomeViewModel

        [TestMethod]
        public void HomeViewModel_SetNextCarpoolRideDepartureTimeSeconds_CheckNextCarpoolRideContent()
        {
            // Arrange
            HomeViewModel homeViewModel = new HomeViewModel();
            CarpoolRide carpoolRide = new CarpoolRide() { DepartureTime = DateTime.Now };
            // Act
            homeViewModel.NextCarpoolRide = carpoolRide;
            // Assert
            Assert.IsTrue(homeViewModel.NextRideInContent != null);
        }
        [TestMethod]
        public void HomeViewModel_SetNextCarpoolRideDeaprtureTimeMinutes_CheckNextCarpoolRideContent()
        {
            // Arrange
            HomeViewModel homeViewModel = new HomeViewModel();
            CarpoolRide carpoolRide = new CarpoolRide() { DepartureTime = DateTime.Now.AddMinutes(5).AddSeconds(30) };
            // Act
            homeViewModel.NextCarpoolRide = carpoolRide;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("minutes"), "5"), homeViewModel.NextRideInContent);
        }
        [TestMethod]
        public void HomeViewModel_SetNextCarpoolRideDeaprtureTimeHours_CheckNextCarpoolRideContent()
        {
            // Arrange
            HomeViewModel homeViewModel = new HomeViewModel();
            CarpoolRide carpoolRide = new CarpoolRide() { DepartureTime = DateTime.Now.AddHours(5).AddSeconds(30) };
            // Act
            homeViewModel.NextCarpoolRide = carpoolRide;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("hours"), "5"), homeViewModel.NextRideInContent);
        }
        [TestMethod]
        public void HomeViewModel_SetNextCarpoolRideDeaprtureTimeDays_CheckNextCarpoolRideContent()
        {
            // Arrange
            HomeViewModel homeViewModel = new HomeViewModel();
            CarpoolRide carpoolRide = new CarpoolRide() { DepartureTime = DateTime.Now.AddDays(5).AddSeconds(30) };
            // Act
            homeViewModel.NextCarpoolRide = carpoolRide;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("days"), "5"), homeViewModel.NextRideInContent);
        }
        [TestMethod]
        public void HomeViewModel_SetNextCarpoolRideToNull_CheckNextCarpoolRideContent()
        {
            // Arrange
            HomeViewModel homeViewModel = new HomeViewModel();
            // Act
            homeViewModel.NextCarpoolRide = null;
            // Assert
            Assert.AreEqual(GetLocalizedString("none"), homeViewModel.NextRideInContent);
        }
        [TestMethod]
        public void HomeViewModel_SetOpeningHoursContent_CheckIfValueIsSet()
        {
            // Arrange
            HomeViewModel homeViewModel = new HomeViewModel();
            // Act
            homeViewModel.OpeningHoursContent = "test";
            // Assert
            Assert.AreEqual("test", homeViewModel.OpeningHoursContent);
        }

        #endregion

        #endregion
    }
}
