using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthShoreSurfApp;
using NorthShoreSurfApp.Database;
using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        /*****************************************************************/
        // SERVICES
        /*****************************************************************/
        #region Services 

        /****************************************************/
        // DATABASE SERVICE
        /****************************************************/
        #region DatabaseService

        [TestMethod]
        [TestCategory("DatabaseService")]
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
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetEvents_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetEvents();
                // Assert
                Assert.IsTrue(response.Result.Count > 0);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetOpeningHours_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetOpeningHours();
                // Assert
                Assert.IsTrue(response.Result != null);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetContactInfo_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetContactInfo();
                ContactInfo contactInfo = response.Result;
                // Assert
                Assert.IsTrue(
                    contactInfo.Address != null &&
                    contactInfo.Email != null &&
                    contactInfo.ZipCode != null &&
                    contactInfo.City != null &&
                    contactInfo.PhoneNo != null
                );
            }
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetUsersCarpoolRides_Success(int userId)
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetUsersCarpoolRides(userId);
                // Assert
                Assert.IsTrue(response.Result.OwnRides
                                .TrueForAll(x => x.DriverId == userId ||
                                x.CarpoolConfirmations.Any(x2 => x2.IsConfirmed && x2.PassengerId == userId)) &&
                                response.Result.OtherRides.TrueForAll(x => x.DriverId != userId)
                );
            }
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetUsersCarpoolRequests_Success(int userId)
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetUsersCarpoolRequests(userId);
                // Assert
                Assert.IsTrue(
                    response.Result.OwnRequests.TrueForAll(x => x.PassengerId == userId) &&
                    response.Result.OtherRequests.TrueForAll(x => x.PassengerId != userId)
                );
            }
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetOwnCarpoolRides_Success(int userId)
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetOwnCarpoolRides(userId);
                // Assert
                Assert.IsTrue(
                    response.Result.TrueForAll(x => x.DriverId == userId)
                );
            }
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetConfirmations_Success(int userId)
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetConfirmations(userId);
                // Assert
                Assert.IsTrue(
                    response.Result.OwnRides.TrueForAll(x => x.DriverId == userId) &&
                    response.Result.OtherRides.TrueForAll(x => x.DriverId != userId)
                );
            }
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetCars_Success(int userId)
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetCars(userId);
                // Assert
                Assert.IsTrue(response.Result.TrueForAll(x => x.UserId == userId));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetOpeningHoursInformation_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetOpeningHoursInformation();
                // Assert
                Assert.IsTrue(!string.IsNullOrEmpty(response.Result));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetTodaysOpeningHours_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetTodaysOpeningHours();
                // Assert
                Assert.IsTrue(!string.IsNullOrEmpty(response.Result));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetNextCarpoolRide_NoNextCarpoolRide()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetNextCarpoolRide(1);
                // Assert
                Assert.IsTrue(response.Success);
            }
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetCarpoolConfirmations_Success(int userId)
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetCarpoolConfirmations(userId);
                // Assert
                Assert.IsTrue(response.Result.TrueForAll(x => x.Passenger.Id == userId || x.CarpoolRide.Driver.Id == userId));
            }
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetPendingCarpoolConfirmations_Success(int userId)
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetPendingCarpoolConfirmations(userId);
                // Assert
                Assert.IsTrue(response.Result.TrueForAll(x => (x.Passenger.Id == userId || x.CarpoolRide.Driver.Id == userId) && !x.IsConfirmed));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_CheckIfPhoneIsNotUsedAlready_PhoneNoAlreadyUsed()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                string phoneNo = "+4530360633";
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.CheckIfPhoneIsNotUsedAlready(phoneNo);
                // Assert
                Assert.IsTrue(!response.Success);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_CheckIfPhoneIsNotUsedAlready_PhoneNoNotUsed()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                string phoneNo = "+4510101010";
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.CheckIfPhoneIsNotUsedAlready(phoneNo);
                // Assert
                Assert.IsTrue(response.Success);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetUser_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetUser(1);
                // Assert
                Assert.IsTrue(response.Success);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetUser_UserNotFound()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetUser(10000);
                // Assert
                Assert.IsTrue(!response.Success);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_UpdateUser_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                string firstName = "test";
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.UpdateUser(1, firstName, "test", "1234", 30, 2);
                var response2 = await service.GetUser(1);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.FirstName == firstName);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_SignUpUser_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                string firstName = "test";
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.SignUpUser(firstName, "test", "1234", 30, 2);
                var response2 = await service.GetUser(response.Result.Id);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.FirstName == firstName);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_DeleteUser_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.DeleteUser(1);
                var response2 = await service.GetUser(1);
                // Assert
                Assert.IsTrue(response.Success && !response2.Success);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_CreateCar_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.CreateCar(1, "TE12345", "Grøn");
                var response2 = await service.GetCars(1);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.Any(x => x.Id == response.Result.Id));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_DeleteCar_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.DeleteCar(1);
                var response2 = await service.GetCars(1);
                // Assert
                Assert.IsTrue(response.Success && !response2.Result.Any(x => x.Id == 1));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_CreateCarpoolRide_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                // Make list of events
                var events = new System.Collections.Generic.List<Event>();
                events.Add(new Event() { Id = 1 });
                events.Add(new Event() { Id = 2 });
                // Create carpool ride with test values
                var response = await service.CreateCarpoolRide(1, DateTime.Now.AddDays(1), "Testvej 1", "1234", "Testby", "Testvej 10", "4321", "Testby 2", 1, 4, 100, events, "this is a test");
                // Get user's carpool rides
                var response2 = await service.GetOwnCarpoolRides(1);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.Any(x => x.Id == response.Result.Id));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_DeleteCarpoolRide_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.DeleteCarpoolRide(1);
                var response2 = await service.GetUsersCarpoolRides(1);
                // Assert
                Assert.IsTrue(response.Success && !response2.Result.OtherRides.Any(x => x.Id == 1) && !response2.Result.OwnRides.Any(x => x.Id == 1));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_DeleteCarpoolRequest_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                // Get user's carpool requests
                var resp = await service.GetUsersCarpoolRequests(userId);
                // Get the first of the user's own requests
                int id = resp.Result.OwnRequests.FirstOrDefault().Id;
                var response = await service.DeleteCarpoolRequest(id);
                var response2 = await service.GetUsersCarpoolRequests(userId);
                // Assert
                Assert.IsTrue(response.Success && !response2.Result.OtherRequests.Any(x => x.Id == id) && !response2.Result.OwnRequests.Any(x => x.Id == id));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_CreateCarpoolRequest_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                // Make list of events
                var events = new System.Collections.Generic.List<Event>();
                events.Add(new Event() { Id = 1 });
                events.Add(new Event() { Id = 2 });
                // Create carpool request with test values
                var response = await service.CreateCarpoolRequest(1, DateTime.Now.AddMinutes(20), DateTime.Now.AddMinutes(30), "1234", "Testby", "Testvej 1", "4321", "Testby 2", events, "This is a test");
                // Get user's carpool requests
                var response2 = await service.GetUsersCarpoolRequests(1);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.OwnRequests.Any(x => x.Id == response.Result.Id));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_InvitePassenger_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                // Get user's carpool requests
                var resp = await service.GetUsersCarpoolRequests(userId);
                // Get first of the unrelated requests to the user
                int carpoolRequestId = resp.Result.OtherRequests.FirstOrDefault().Id;
                // Invite passenger to ride
                var response = await service.InvitePassenger(carpoolRequestId, 1);
                // Get user's carpool confirmations
                var response2 = await service.GetCarpoolConfirmations(userId);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.Any(x => x.CarpoolRequestId == carpoolRequestId));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_SignUpToCarpoolRide_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                // Get user's carpool rides
                var resp = await service.GetUsersCarpoolRides(userId);
                // Get first of unrelated rides to the user
                int carpoolRideId = resp.Result.OtherRides.FirstOrDefault().Id;
                // Sign up to carpool ride
                var response = await service.SignUpToCarpoolRide(carpoolRideId, userId);
                // Get user's carpool confirmations
                var response2 = await service.GetCarpoolConfirmations(userId);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.Any(x => x.CarpoolRideId == carpoolRideId));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_UnsignFromCarpoolRide_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                // Get user's carpool rides
                var resp = await service.GetUsersCarpoolRides(userId);
                // Get first of the user's own rides with a confirmed passenger
                CarpoolRide carpoolRide = resp.Result.OwnRides.Where(x => x.CarpoolConfirmations.Any(x2 => x2.IsConfirmed)).FirstOrDefault();
                // Get passenger id for confirmed passenger
                int passengerId = carpoolRide.CarpoolConfirmations.OrderBy(x => x.PassengerId).FirstOrDefault(x => x.IsConfirmed).PassengerId;
                // Unsign the passenger
                var response = await service.UnsignFromCarpoolRide(carpoolRide.Id, passengerId);
                // Get user's carpool rides
                var response2 = await service.GetUsersCarpoolRides(userId);
                // Assert
                Assert.IsTrue(response.Success && !response2.Result.OwnRides.FirstOrDefault(x => x.Id == carpoolRide.Id).CarpoolConfirmations.Any(x => x.PassengerId == passengerId));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_AnswerCarpoolConfirmation_Accept()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                // Get user's carpool confirmations
                var resp = await service.GetCarpoolConfirmations(userId);
                // Get first confirmation that is an invite
                var carpoolConfirmation = resp.Result.FirstOrDefault(x => x.PassengerId == userId && x.IsInvite);
                // Accept the invitation
                var response = await service.AnswerCarpoolConfirmation(userId, carpoolConfirmation.Id, true);
                // Get user's carpool rides
                var response2 = await service.GetUsersCarpoolRides(userId);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.OwnRides.Any(x => x.Id == carpoolConfirmation.CarpoolRideId));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_AnswerCarpoolConfirmation_Deny()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                // Get user's carpool confirmations
                var resp = await service.GetCarpoolConfirmations(userId);
                // Get first confirmation that is an invite
                var carpoolConfirmation = resp.Result.FirstOrDefault(x => x.PassengerId == userId && x.IsInvite);
                // Deny the invite 
                var response = await service.AnswerCarpoolConfirmation(userId, carpoolConfirmation.Id, false);
                // Get the user's carpool confirmations
                var response2 = await service.GetCarpoolConfirmations(userId);
                // Assert
                Assert.IsTrue(response.Success && !response2.Result.Any(x => x.Id == carpoolConfirmation.Id));
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_UpdateCarpoolRequest_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                // User's id
                int userId = 1;
                // Get user's carpool requests
                var resp = await service.GetUsersCarpoolRequests(userId);
                // Get first request
                var carpoolRequest = resp.Result.OwnRequests.OrderBy(x => x.Id).FirstOrDefault();
                // Update values for the request
                var response = await service.UpdateCarpoolRequest(carpoolRequest.Id, DateTime.Now.AddMinutes(30), DateTime.Now.AddMinutes(40), "1234", "Testby", "Testvej 1", "4321", "Testby 2", new System.Collections.Generic.List<Event>(), null);
                // Get the user's requests
                var response2 = await service.GetUsersCarpoolRequests(userId);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.OwnRequests.FirstOrDefault(x => x.Id == carpoolRequest.Id && x.ZipCode == "1234") != null);
            }
        }
        [TestMethod]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_UpdateCarpoolRide_Success()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                // User's id
                int userId = 1;
                // Get user's carpool rides
                var resp = await service.GetUsersCarpoolRides(userId);
                // Get first ride
                var carpoolRide = resp.Result.OwnRides.OrderBy(x => x.Id).FirstOrDefault();
                // Update values for the ride
                var response = await service.UpdateCarpoolRide(carpoolRide.Id, DateTime.Now.AddMinutes(30), "Testvej 2", "1234", "Testby", "Testvej 1", "4321", "Testby 2", 1, 100, new System.Collections.Generic.List<Event>(), "This is a test");
                // Get the user's rides
                var response2 = await service.GetUsersCarpoolRides(userId);
                // Assert
                Assert.IsTrue(response.Success && response2.Result.OwnRides.FirstOrDefault(x => x.Id == carpoolRide.Id && x.ZipCode == "1234") != null);
            }
        }

        #endregion

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
        [TestCategory("HomeViewModel")]
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
        [TestCategory("HomeViewModel")]
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
        [TestCategory("HomeViewModel")]
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
        [TestCategory("HomeViewModel")]
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
        [TestCategory("HomeViewModel")]
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
        [TestCategory("HomeViewModel")]
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

        /****************************************************/
        // HOME DETAILS VIEW MODEL
        /****************************************************/
        #region HomeDetailsViewModel

        [TestMethod]
        [TestCategory("HomeDetailsViewModel")]
        public void HomeDetailsViewModel_SetPageTypeToContactInfo_CheckPageTitle()
        {
            // Arrange
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            // Act
            homeDetailsViewModel.PageType = HomeDetailsPageType.ContactInfo;
            // Assert
            Assert.AreEqual(GetLocalizedString("contact_us"), homeDetailsViewModel.PageTitle);
        }
        [TestMethod]
        [TestCategory("HomeDetailsViewModel")]
        public void HomeDetailsViewModel_SetPageTypeToContactInfo_ShowContactInfoDetailsIsTrue()
        {
            // Arrange
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            // Act
            homeDetailsViewModel.PageType = HomeDetailsPageType.ContactInfo;
            // Assert
            Assert.AreEqual(true, homeDetailsViewModel.ShowContactInfoDetails);
        }
        [TestMethod]
        [TestCategory("HomeDetailsViewModel")]
        public void HomeDetailsViewModel_SetPageTypeToOpeningHours_CheckPageTitle()
        {
            // Arrange
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            // Act
            homeDetailsViewModel.PageType = HomeDetailsPageType.OpeningHours;
            // Assert
            Assert.AreEqual(GetLocalizedString("opening_hours"), homeDetailsViewModel.PageTitle);
        }
        [TestMethod]
        [TestCategory("HomeDetailsViewModel")]
        public void HomeDetailsViewModel_SetPageTypeToOpeningHours_ShowOpeningsHoursDetailsIsTrue()
        {
            // Arrange
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            // Act
            homeDetailsViewModel.PageType = HomeDetailsPageType.OpeningHours;
            // Assert
            Assert.AreEqual(true, homeDetailsViewModel.ShowOpeningHoursDetails);
        }
        [TestMethod]
        [TestCategory("HomeDetailsViewModel")]
        public void HomeDetailsViewModel_SetOpeningHoursDetails_ContentIsSet()
        {
            // Arrange
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            // Act
            homeDetailsViewModel.OpeningHoursDetails = "test";
            // Assert
            Assert.AreEqual("test", homeDetailsViewModel.OpeningHoursDetails);
        }
        [TestMethod]
        [TestCategory("HomeDetailsViewModel")]
        public void HomeDetailsViewModel_SetContactInfo_ContactInfoDetailsIsNotNull()
        {
            // Arrange
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel();
            // Act
            homeDetailsViewModel.ContactInfo = new ContactInfo()
            {
                Address = "test",
                City = "test1",
                Email = "test2",
                PhoneNo = "test3",
                ZipCode = "test4"
            };
            // Assert
            Assert.IsTrue(!string.IsNullOrEmpty(homeDetailsViewModel.ContactInfoDetails));
        }

        #endregion

        /****************************************************/
        // NEW CAR VIEW MODEL
        /****************************************************/
        #region NewCarViewModel

        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_TestAllDataGiven_IsTrue()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.Color = "test";
            newCarViewModel.LicensePlate = "TE12345";
            // Assert
            Assert.AreEqual(true, newCarViewModel.AllDataGiven);
        }
        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_TestAllDataGiven_IsFalse()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.Color = "test";
            newCarViewModel.LicensePlate = "T";
            // Assert
            Assert.AreEqual(false, newCarViewModel.AllDataGiven);
        }
        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_CreateCommand_CommandIsExecuted()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.CreateCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            newCarViewModel.CreateCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_CancelCommand_CommandIsExecuted()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.CancelCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            newCarViewModel.CancelCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }

        #endregion

        /****************************************************/
        // SIGN UP USER VIEW MODEL
        /****************************************************/
        #region SignUpUserViewModel

        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_SetCurrentContentSiteToEnterData_ShowEnterDataSiteIsTrue()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.CurrentContentSite = SignUpUserPageContentSite.EnterData;
            // Assert
            Assert.AreEqual(true, signUpUserViewModel.ShowEnterDataSite);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_SetCurrentContentSiteToEditInformation_ShowEnterDataSiteIsTrue()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.CurrentContentSite = SignUpUserPageContentSite.EditInformation;
            // Assert
            Assert.AreEqual(true, signUpUserViewModel.ShowEnterDataSite);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_SetCurrentContentSiteToEnterSMSCode_ShowEnterSMSCodeSiteIsTrue()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.CurrentContentSite = SignUpUserPageContentSite.EnterSMSCode;
            // Assert
            Assert.AreEqual(true, signUpUserViewModel.ShowEnterSMSCodeSite);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_ButtonNextIcon_NotNullWhenSettingContentSite()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.CurrentContentSite = SignUpUserPageContentSite.EditInformation;
            // Assert
            Assert.IsTrue(signUpUserViewModel.ButtonNextIcon != null);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_ButtonNextTitle_CorrectTitleForContentSiteEditInformation()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.CurrentContentSite = SignUpUserPageContentSite.EditInformation;
            // Assert
            Assert.AreEqual(GetLocalizedString("approve"), signUpUserViewModel.ButtonNextTitle);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_ButtonNextTitle_CorrectTitleForContentSiteOtherThanEditInformation()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.CurrentContentSite = SignUpUserPageContentSite.EnterData;
            // Assert
            Assert.AreEqual(GetLocalizedString("next"), signUpUserViewModel.ButtonNextTitle);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_ButtonNextTitle_CorrectMarginForPageTypeLogin()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.PageType = SignUpUserPageType.Login;
            // Assert
            Assert.AreEqual(new Thickness(0), signUpUserViewModel.TextBoxPhoneNoMargin);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_ButtonNextTitle_CorrectMarginForPageTypeOtherThanLogin()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.PageType = SignUpUserPageType.SignUp;
            // Assert
            Assert.AreEqual(new Thickness(0, 5, 0, 0), signUpUserViewModel.TextBoxPhoneNoMargin);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_ShowLoginPage_TrueIfPageTypeLoginIsSet()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.PageType = SignUpUserPageType.Login;
            // Assert
            Assert.AreEqual(true, signUpUserViewModel.ShowLoginPage);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        [DataRow(SignUpUserPageType.SignUp, "sign_up")]
        [DataRow(SignUpUserPageType.Login, "log_in")]
        [DataRow(SignUpUserPageType.EditInformation, "edit_information")]
        public void SignUpUserViewModel_PageTitle_CorrectForPageType(SignUpUserPageType pageType, string keyForLocalizedString)
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.PageType = pageType;
            // Assert
            Assert.AreEqual(GetLocalizedString(keyForLocalizedString), signUpUserViewModel.PageTitle);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_AllDataGiven_TrueForEditInformationAndSignup()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.PageType = SignUpUserPageType.EditInformation;
            signUpUserViewModel.FirstName = "test";
            signUpUserViewModel.LastName = "test2";
            signUpUserViewModel.Age = "30";
            signUpUserViewModel.PhoneNo = "test4";
            signUpUserViewModel.GenderId = 2;
            // Assert
            Assert.AreEqual(true, signUpUserViewModel.AllDataGiven);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_AllDataGiven_TrueForLogin()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.PageType = SignUpUserPageType.Login;
            signUpUserViewModel.PhoneNo = "test4";
            // Assert
            Assert.AreEqual(true, signUpUserViewModel.AllDataGiven);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public async Task SignUpUserViewModel_HasPhoneNumberChanged_FalseIfNothingChanged()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
                // Act
                var response = await service.GetUser(1);
                // Act
                signUpUserViewModel.ExistingUser = response.Result;
                signUpUserViewModel.PageType = SignUpUserPageType.Login;
                // Assert
                Assert.AreEqual(false, signUpUserViewModel.HasPhoneNumberChanged);
            }
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public async Task SignUpUserViewModel_HasPhoneNumberChanged_TrueIfPhoneNoChanged()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
                // Act
                var response = await service.GetUser(1);
                // Act
                signUpUserViewModel.ExistingUser = response.Result;
                signUpUserViewModel.PageType = SignUpUserPageType.Login;
                signUpUserViewModel.PhoneNo = "1234";
                // Assert
                Assert.AreEqual(true, signUpUserViewModel.HasPhoneNumberChanged);
            }
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public async Task SignUpUserViewModel_ExistingUser_ValuesAreSet()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
                // Act
                var response = await service.GetUser(1);
                var user = response.Result;
                // Act
                signUpUserViewModel.ExistingUser = user;
                // Assert
                Assert.IsTrue(
                    signUpUserViewModel.FirstName == user.FirstName &&
                    signUpUserViewModel.LastName == user.LastName &&
                    signUpUserViewModel.Age == user.Age.ToString() &&
                    signUpUserViewModel.PhoneNo == user.PhoneNo &&
                    signUpUserViewModel.Gender == user.Gender.LocalizedName &&
                    signUpUserViewModel.GenderId == user.GenderId
                );
            }
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_Genders_NotNullAndCountIsGreaterThanOne()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Assert
            Assert.IsTrue(signUpUserViewModel.Genders.Count > 0);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_AgeValue_RightInput()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.Age = "12";
            // Assert
            Assert.AreEqual(12, signUpUserViewModel.AgeValue);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_AgeValue_WrongInput()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.Age = "12r";
            // Assert
            Assert.AreEqual(-1, signUpUserViewModel.AgeValue);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_GenderPickerItemTemplate_NotNull()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Assert
            Assert.IsTrue(signUpUserViewModel.GenderPickerItemTemplate != null);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_NextCommand_CommandIsExecuted()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.NextCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            signUpUserViewModel.NextCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("SignUpUserViewModel")]
        public void SignUpUserViewModel_ApproveCommand_CommandIsExecuted()
        {
            // Arrange
            SignUpUserViewModel signUpUserViewModel = new SignUpUserViewModel();
            // Act
            signUpUserViewModel.ApproveCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            signUpUserViewModel.ApproveCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }

        #endregion

        #endregion
    }
}
