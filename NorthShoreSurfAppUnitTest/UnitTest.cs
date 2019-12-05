using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthShoreSurfApp;
using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewModels;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

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
        private string GetLocalizedString(string key) {
            return ResourceManager.GetString(key, CultureInfo.CurrentCulture);
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

        [TestMethod]
        public void HomeViewModel_SetNextCarpoolRide_CheckNextCarpoolRideContent()
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
        public void HomeViewModel_SetNextCarpoolRideToNull_CheckNextCarpoolRideContent()
        {
            // Arrange
            HomeViewModel homeViewModel = new HomeViewModel();
            // Act
            homeViewModel.NextCarpoolRide = null;
            // Assert
            Assert.AreEqual(GetLocalizedString("none"), homeViewModel.NextRideInContent);
        }

        #endregion
    }
}
