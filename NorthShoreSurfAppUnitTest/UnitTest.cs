using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthShoreSurfApp;
using NorthShoreSurfApp.Database;
using NorthShoreSurfApp.ModelComponents;
using NorthShoreSurfApp.ViewModels;
using System;
using System.Collections.Generic;
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

        private IDataService getDataService(NSSDatabaseContextFactory factory)
        {
            return new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
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
                Assert.AreEqual(false, response.Success);
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
                Assert.AreEqual(true, response.Success);
            }
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [TestCategory("DatabaseService")]
        public async Task DatabaseService_GetUser_Success(int userId)
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                NSSDatabaseService<NSSDatabaseContext> service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                var response = await service.GetUser(userId);
                // Assert
                Assert.IsTrue(response.Success && response.Result.Id == userId);
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
                CarpoolRide carpoolRide = resp.Result.OwnRides.Where(x => x.CarpoolConfirmations.Any(x2 => x2.IsConfirmed && x2.PassengerId != userId)).FirstOrDefault();
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

        /****************************************************/
        // CAR
        /****************************************************/
        #region Car

        [TestMethod]
        [TestCategory("Car")]
        public void Car_IsTitle_IsTrue()
        {
            // Arrange
            Car car = new Car();
            // Act
            car.Title = "test";
            // Assert
            Assert.IsTrue(car.IsTitle);
        }
        [TestMethod]
        [TestCategory("Car")]
        public void Car_IsTitle_IsFalse()
        {
            // Arrange
            Car car = new Car();
            // Act
            car.Title = null;
            // Assert
            Assert.IsFalse(car.IsTitle);
        }
        [TestMethod]
        [TestCategory("Car")]
        public void Car_CarInfo_IsNotNull()
        {
            // Arrange
            Car car = new Car();
            // Act
            car.LicensePlate = "test";
            car.Color = "test";
            // Assert
            Assert.IsNotNull(car.CarInfo);
        }

        #endregion

        /****************************************************/
        // CARPOOL CONFIRMATION
        /****************************************************/
        #region CarpoolConfirmation

        [TestMethod]
        [TestCategory("CarpoolConfirmation")]
        public async Task CarpoolConfirmation_IsOwnRide_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                var response = await service.GetCarpoolConfirmations(userId);
                var carpoolConfirmation = response.Result.FirstOrDefault(x => x.CarpoolRide.DriverId == userId);
                carpoolConfirmation.CurrentUserId = userId;
                // Assert
                Assert.IsTrue(carpoolConfirmation.IsOwnRide);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolConfirmation")]
        public async Task CarpoolConfirmation_IsOtherRide_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                var response = await service.GetCarpoolConfirmations(userId);
                var carpoolConfirmation = response.Result.FirstOrDefault(x => x.PassengerId == userId);
                carpoolConfirmation.CurrentUserId = userId;
                // Assert
                Assert.IsTrue(carpoolConfirmation.IsOtherRide);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolConfirmation")]
        public async Task CarpoolConfirmation_InformationString_NotEmpty()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                var response = await service.GetCarpoolConfirmations(userId);
                var carpoolConfirmation = response.Result.FirstOrDefault(x => x.IsRequest);
                // Assert
                Assert.IsTrue(carpoolConfirmation.InformationString != string.Empty);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolConfirmation")]
        public async Task CarpoolConfirmation_ShowAcceptButton_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                var response = await service.GetCarpoolConfirmations(userId);
                var carpoolConfirmation = response.Result.FirstOrDefault(x => x.IsInvite && x.PassengerId == userId);
                carpoolConfirmation.CurrentUserId = userId;
                // Assert
                Assert.IsTrue(carpoolConfirmation.ShowAcceptButton);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolConfirmation")]
        public async Task CarpoolConfirmation_ShowDenyButton_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                // Act
                int userId = 1;
                var response = await service.GetCarpoolConfirmations(userId);
                var carpoolConfirmation = response.Result.FirstOrDefault(x => x.IsInvite && x.PassengerId == userId);
                carpoolConfirmation.CurrentUserId = userId;
                // Assert
                Assert.IsTrue(carpoolConfirmation.ShowDenyButton);
            }
        }

        #endregion

        /****************************************************/
        // CARPOOL REQUEST
        /****************************************************/
        #region CarpoolRequest

        [TestMethod]
        [TestCategory("CarpoolRequest")]
        public void CarpoolRequest_ZipcodeCityString_GivesRightOutput()
        {
            // Arrange
            CarpoolRequest carpoolRequest = new CarpoolRequest();
            // Act
            carpoolRequest.ZipCode = "1234";
            carpoolRequest.City = "test";
            // Assert
            Assert.AreEqual("1234 test", carpoolRequest.ZipcodeCityString);
        }
        [TestMethod]
        [TestCategory("CarpoolRequest")]
        public void CarpoolRequest_DestinationZipcodeCityString_GivesRightOutput()
        {
            // Arrange
            CarpoolRequest carpoolRequest = new CarpoolRequest();
            // Act
            carpoolRequest.DestinationZipCode = "1234";
            carpoolRequest.DestinationCity = "test";
            // Assert
            Assert.AreEqual("1234 test", carpoolRequest.DestinationZipcodeCityString);
        }
        [TestMethod]
        [TestCategory("CarpoolRequest")]
        public void CarpoolRequest_TimeInterval_GivesRightOutput()
        {
            // Arrange
            CarpoolRequest carpoolRequest = new CarpoolRequest();
            // Act
            carpoolRequest.FromTime = new DateTime(2019, 1, 1, 8, 0, 0);
            carpoolRequest.ToTime = new DateTime(2019, 1, 1, 16, 0, 0);
            // Assert
            Assert.AreEqual("08:00-16:00", carpoolRequest.TimeInterval);
        }
        [TestMethod]
        [TestCategory("CarpoolRequest")]
        public void CarpoolRequest_DepartureTimeDayString_GivesRightOutput()
        {
            // Arrange
            CarpoolRequest carpoolRequest = new CarpoolRequest();
            // Act
            carpoolRequest.FromTime = DateTime.Now;
            // Assert
            Assert.AreEqual(DateTime.Now.ToString("dddd"), carpoolRequest.DepartureTimeDayString);
        }

        #endregion

        /****************************************************/
        // CARPOOL RIDE
        /****************************************************/
        #region CarpoolRide

        [TestMethod]
        [TestCategory("CarpoolRide")]
        public void CarpoolRide_DepartureTimeDayString_GivesRightOutput()
        {
            // Arrange
            CarpoolRide carpoolRide = new CarpoolRide();
            // Act
            carpoolRide.DepartureTime = DateTime.Now;
            // Assert
            Assert.AreEqual(DateTime.Now.ToString("dddd"), carpoolRide.DepartureTimeDayString);
        }
        [TestMethod]
        [TestCategory("CarpoolRide")]
        public void CarpoolRide_DepartureTimeHourString_GivesRightOutput()
        {
            // Arrange
            CarpoolRide carpoolRide = new CarpoolRide();
            // Act
            carpoolRide.DepartureTime = new DateTime(2019, 1, 1, 8, 0, 0);
            // Assert
            Assert.AreEqual("08:00", carpoolRide.DepartureTimeHourString);
        }
        [TestMethod]
        [TestCategory("CarpoolRide")]
        public void CarpoolRide_PricePerPassengerString_GivesFreeOutput()
        {
            // Arrange
            CarpoolRide carpoolRide = new CarpoolRide();
            // Act
            carpoolRide.PricePerPassenger = 0;
            // Assert
            Assert.AreEqual(GetLocalizedString("free"), carpoolRide.PricePerPassengerString);
        }
        [TestMethod]
        [TestCategory("CarpoolRide")]
        public void CarpoolRide_PricePerPassengerString_GivesNumberOutput()
        {
            // Arrange
            CarpoolRide carpoolRide = new CarpoolRide();
            // Act
            carpoolRide.PricePerPassenger = 10;
            // Assert
            Assert.IsTrue(carpoolRide.PricePerPassengerString.Contains(carpoolRide.PricePerPassenger.ToString()));
        }
        [TestMethod]
        [TestCategory("CarpoolRide")]
        public void CarpoolRide_ZipcodeCityString_GivesRightOutput()
        {
            // Arrange
            CarpoolRide carpoolRide = new CarpoolRide();
            // Act
            carpoolRide.ZipCode = "1234";
            carpoolRide.City = "test";
            // Assert
            Assert.AreEqual("1234 test", carpoolRide.ZipcodeCityString);
        }
        [TestMethod]
        [TestCategory("CarpoolRide")]
        public void CarpoolRide_DestinationZipcodeCityString_GivesRightOutput()
        {
            // Arrange
            CarpoolRide carpoolRide = new CarpoolRide();
            // Act
            carpoolRide.DestinationZipCode = "1234";
            carpoolRide.DestinationCity = "test";
            // Assert
            Assert.AreEqual("1234 test", carpoolRide.DestinationZipcodeCityString);
        }
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [TestCategory("CarpoolRide")]
        public void CarpoolRide_AvailableSeats_GivesRightOutput(int numberOfSeats)
        {
            // Arrange
            CarpoolRide carpoolRide = new CarpoolRide() { NumberOfSeats = numberOfSeats };
            // Act
            carpoolRide.CarpoolConfirmations = new List<CarpoolConfirmation>();
            carpoolRide.CarpoolConfirmations.Add(new CarpoolConfirmation() { HasDriverAccepted = true, HasPassengerAccepted = true });
            // Assert
            Assert.AreEqual(numberOfSeats - 1, carpoolRide.AvailableSeats);
        }

        #endregion

        /****************************************************/
        // GENDER
        /****************************************************/
        #region Gender

        [TestMethod]
        [TestCategory("Gender")]
        public void Gender_LocalizedName_GivesRightOutput()
        {
            // Arrange
            Gender gender = new Gender() { Id = 1 };
            // Assert
            Assert.AreEqual(GetLocalizedString("male"), gender.LocalizedName);
        }

        #endregion

        /****************************************************/
        // OPENING HOUR
        /****************************************************/
        #region OpeningHour

        [TestMethod]
        [TestCategory("OpeningHour")]
        public void OpeningHour_IsClosed_IsTrue()
        {
            // Arrange
            OpeningHour openingHour = new OpeningHour();
            // Assert
            Assert.IsTrue(openingHour.IsClosed);
        }
        [TestMethod]
        [TestCategory("OpeningHour")]
        public void OpeningHour_IsClosed_IsFalse()
        {
            // Arrange
            OpeningHour openingHour = new OpeningHour();
            // Act
            openingHour.InHolidays = true;
            // Assert
            Assert.IsFalse(openingHour.IsClosed);
        }
        [TestMethod]
        [TestCategory("OpeningHour")]
        public void OpeningHour_IsOpenEveryday_IsTrue()
        {
            // Arrange
            OpeningHour openingHour = new OpeningHour();
            // Act
            openingHour.Monday = true;
            openingHour.Tuesday = true;
            openingHour.Wednesday = true;
            openingHour.Thursday = true;
            openingHour.Friday = true;
            openingHour.Saturday = true;
            openingHour.Sunday = true;
            // Assert
            Assert.IsTrue(openingHour.IsOpenEveryday);
        }
        [TestMethod]
        [TestCategory("OpeningHour")]
        public void OpeningHour_IsOpenEveryday_IsFalse()
        {
            // Arrange
            OpeningHour openingHour = new OpeningHour();
            // Act
            openingHour.Monday = true;
            openingHour.Tuesday = true;
            openingHour.Wednesday = true;
            openingHour.Thursday = true;
            openingHour.Friday = true;
            openingHour.Saturday = true;
            openingHour.Sunday = false;
            // Assert
            Assert.IsFalse(openingHour.IsOpenEveryday);
        }

        #endregion

        /****************************************************/
        // USER
        /****************************************************/
        #region User

        [TestMethod]
        [TestCategory("User")]
        public void User_FullName_GivesRightOutput()
        {
            // Arrange
            User user = new User();
            // Act
            user.FirstName = "test";
            user.LastName = "test1";
            // Assert
            Assert.AreEqual("test test1", user.FullName);
        }
        [TestMethod]
        [TestCategory("User")]
        public void User_LocalizedGender_GivesRightOutput()
        {
            // Arrange
            User user = new User();
            // Act
            user.Gender = new Gender() { Id = 2 };
            // Assert
            Assert.AreEqual(GetLocalizedString("female"), user.LocalizedGender);
        }

        #endregion

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
        public void NewCarViewModel_SetPageType_ButtonTitleIsEdit()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.Car = new Car() { LicensePlate = "TE12345", Color = "Green" };
            // Assert
            Assert.AreEqual(GetLocalizedString("edit_car"), newCarViewModel.ButtonTitle);
        }
        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_SetPageType_ButtonTitleIsAdd()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.Car = null;
            // Assert
            Assert.AreEqual(GetLocalizedString("add_new_car"), newCarViewModel.ButtonTitle);
        }
        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_SetPageType_NavigationBarTitleIsEdit()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.Car = new Car() { LicensePlate = "TE12345", Color = "Green" };
            // Assert
            Assert.AreEqual(GetLocalizedString("edit_car"), newCarViewModel.NavigationBarTitle);
        }
        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_SetPageType_NavigationBarTitleIsAdd()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.Car = null;
            // Assert
            Assert.AreEqual(GetLocalizedString("add_new_car"), newCarViewModel.NavigationBarTitle);
        }
        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_ButtonIcon_IsNotNull()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();           
            // Assert
            Assert.IsNotNull(newCarViewModel.ButtonIcon);
        }
        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_ButtonCommand_CommandIsExecuted()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.ButtonCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            newCarViewModel.ButtonCommand.Execute(null);
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
        [TestMethod]
        [TestCategory("NewCarViewModel")]
        public void NewCarViewModel_SetCar_ValuesAreSet()
        {
            // Arrange
            NewCarViewModel newCarViewModel = new NewCarViewModel();
            // Act
            newCarViewModel.Car = new Car() { LicensePlate = "TE12345", Color = "Green" };
            // Assert
            Assert.IsTrue(
                newCarViewModel.LicensePlate != null &&
                newCarViewModel.Color != null
            );
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

        /****************************************************/
        // SURFING CONDITIONS FULLSCREEN VIEW MODEL
        /****************************************************/
        #region SurfingConditionsFullscreenViewModel

        [TestMethod]
        [TestCategory("SurfingConditionsFullscreenViewModel")]
        public void SurfingConditionsFullscreenViewModel_SetPageTypeWebView_ShowWebViewPageIsTrue()
        {
            // Arrange
            SurfingConditionsFullscreenViewModel surfingConditionsFullscreenViewModel = new SurfingConditionsFullscreenViewModel();
            // Act
            surfingConditionsFullscreenViewModel.PageType = SurfingConditionsFullscreenPageType.WebView;
            // Assert
            Assert.AreEqual(true, surfingConditionsFullscreenViewModel.ShowWebViewPage);
        }
        [TestMethod]
        [TestCategory("SurfingConditionsFullscreenViewModel")]
        public void SurfingConditionsFullscreenViewModel_SetPageTypeVideo_ShowVideoPageIsTrue()
        {
            // Arrange
            SurfingConditionsFullscreenViewModel surfingConditionsFullscreenViewModel = new SurfingConditionsFullscreenViewModel();
            // Act
            surfingConditionsFullscreenViewModel.PageType = SurfingConditionsFullscreenPageType.Video;
            // Assert
            Assert.AreEqual(true, surfingConditionsFullscreenViewModel.ShowVideoPage);
        }
        [TestMethod]
        [TestCategory("SurfingConditionsFullscreenViewModel")]
        public void SurfingConditionsFullscreenViewModel_SetWebViewSource_NotNull()
        {
            // Arrange
            SurfingConditionsFullscreenViewModel surfingConditionsFullscreenViewModel = new SurfingConditionsFullscreenViewModel();
            // Act
            surfingConditionsFullscreenViewModel.WebViewSource = "test";
            // Assert
            Assert.IsNotNull(surfingConditionsFullscreenViewModel.WebViewSource);
        }
        [TestMethod]
        [TestCategory("SurfingConditionsFullscreenViewModel")]
        public void SurfingConditionsFullscreenViewModel_SetVideoURL_NotNull()
        {
            // Arrange
            SurfingConditionsFullscreenViewModel surfingConditionsFullscreenViewModel = new SurfingConditionsFullscreenViewModel();
            // Act
            surfingConditionsFullscreenViewModel.VideoURL = "test";
            // Assert
            Assert.IsNotNull(surfingConditionsFullscreenViewModel.VideoURL);
        }

        #endregion

        /****************************************************/
        // SURFING CONDITIONS VIEW MODEL
        /****************************************************/
        #region SurfingConditionsViewModel

        [TestMethod]
        [TestCategory("SurfingConditionsViewModel")]
        public void SurfingConditionsViewModel_VideoUrl_NotNull()
        {
            // Assert
            Assert.IsNotNull(SurfingConditionsViewModel.VideoUrl);
        }
        [TestMethod]
        [TestCategory("SurfingConditionsViewModel")]
        public void SurfingConditionsViewModel_OceanInfoUrl_NotNull()
        {
            // Arrange
            SurfingConditionsViewModel surfingConditionsViewModel = new SurfingConditionsViewModel();
            // Assert
            Assert.IsNotNull(surfingConditionsViewModel.OceanInfoUrl);
        }
        [TestMethod]
        [TestCategory("SurfingConditionsViewModel")]
        public void SurfingConditionsViewModel_WeatherInfoUrl_NotNull()
        {
            // Arrange
            SurfingConditionsViewModel surfingConditionsViewModel = new SurfingConditionsViewModel();
            // Assert
            Assert.IsNotNull(surfingConditionsViewModel.WeatherInfoUrl);
        }
        [TestMethod]
        [TestCategory("SurfingConditionsViewModel")]
        public void SurfingConditionsViewModel_SeeWebcamCommand_CommandIsExecuted()
        {
            // Arrange
            SurfingConditionsViewModel surfingConditionsViewModel = new SurfingConditionsViewModel();
            // Act
            surfingConditionsViewModel.SeeWebcamCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            surfingConditionsViewModel.SeeWebcamCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }

        #endregion

        /****************************************************/
        // USER VIEW MODEL
        /****************************************************/
        #region UserViewModel

        [TestMethod]
        [TestCategory("UserViewModel")]
        public async Task UserViewModel_SetExistingUser_ValuesAreSet()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = getDataService(factory);
                UserViewModel userViewModel = new UserViewModel();
                // Act
                var response = await service.GetUser(1);
                var user = response.Result;
                // Act
                userViewModel.ExistingUser = user;
                // Assert
                Assert.IsTrue(
                    userViewModel.FullName == user.FullName &&
                    userViewModel.PhoneNo == user.PhoneNo &&
                    userViewModel.Age == user.Age.ToString() &&
                    userViewModel.Gender == user.Gender.LocalizedName
                );
            }
        }
        [TestMethod]
        [TestCategory("UserViewModel")]
        public void UserViewModel_EditCommand_CommandIsExecuted()
        {
            // Arrange
            UserViewModel userViewModel = new UserViewModel();
            // Act
            userViewModel.EditCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            userViewModel.EditCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("UserViewModel")]
        public void UserViewModel_LogOutCommand_CommandIsExecuted()
        {
            // Arrange
            UserViewModel userViewModel = new UserViewModel();
            // Act
            userViewModel.LogOutCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            userViewModel.LogOutCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("UserViewModel")]
        public void UserViewModel_DeleteAccountCommand_CommandIsExecuted()
        {
            // Arrange
            UserViewModel userViewModel = new UserViewModel();
            // Act
            userViewModel.DeleteAccountCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            userViewModel.DeleteAccountCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }

        #endregion

        /****************************************************/
        // WELCOME VIEW MODEL
        /****************************************************/
        #region WelcomeViewModel

        [TestMethod]
        [TestCategory("WelcomeViewModel")]
        public void WelcomeViewModel_SetContentSiteToWelcome_ShowWelcomeIsTrue()
        {
            // Arrange
            WelcomeViewModel welcomeViewModel = new WelcomeViewModel();
            // Act
            welcomeViewModel.CurrentContentSite = WelcomePageContentSite.Welcome;
            // Assert
            Assert.IsTrue(welcomeViewModel.ShowWelcome);
        }
        [TestMethod]
        [TestCategory("WelcomeViewModel")]
        public void WelcomeViewModel_SetContentSiteToUserNotLoggedIn_ShowUserNotLoggedInIsTrue()
        {
            // Arrange
            WelcomeViewModel welcomeViewModel = new WelcomeViewModel();
            // Act
            welcomeViewModel.CurrentContentSite = WelcomePageContentSite.UserNotLoggedIn;
            // Assert
            Assert.IsTrue(welcomeViewModel.ShowUserNotLoggedIn);
        }
        [TestMethod]
        [TestCategory("WelcomeViewModel")]
        public void WelcomeViewModel_SignUpCommand_CommandIsExecuted()
        {
            // Arrange
            WelcomeViewModel welcomeViewModel = new WelcomeViewModel();
            // Act
            welcomeViewModel.SignUpCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            welcomeViewModel.SignUpCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("WelcomeViewModel")]
        public void WelcomeViewModel_LogInCommand_CommandIsExecuted()
        {
            // Arrange
            WelcomeViewModel welcomeViewModel = new WelcomeViewModel();
            // Act
            welcomeViewModel.LogInCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            welcomeViewModel.LogInCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("WelcomeViewModel")]
        public void WelcomeViewModel_ContinueAsGuestCommand_CommandIsExecuted()
        {
            // Arrange
            WelcomeViewModel welcomeViewModel = new WelcomeViewModel();
            // Act
            welcomeViewModel.ContinueAsGuestCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            welcomeViewModel.ContinueAsGuestCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }

        #endregion

        /****************************************************/
        // CARPOOL CONFIRMATION VIEW MODEL
        /****************************************************/
        #region CarpoolConfirmationViewModel

        [TestMethod]
        [TestCategory("CarpoolConfirmationViewModel")]
        public void CarpoolConfirmationViewModel_SetItemsSource_IsNotNull()
        {
            // Arrange
            CarpoolConfirmationViewModel carpoolConfirmationViewModel = new CarpoolConfirmationViewModel();
            // Act
            carpoolConfirmationViewModel.ItemsSource = new System.Collections.Generic.List<NorthShoreSurfApp.UIComponents.CarpoolConfirmationItem>();
            // Assert
            Assert.IsNotNull(carpoolConfirmationViewModel.ItemsSource);
        }
        [TestMethod]
        [TestCategory("CarpoolConfirmationViewModel")]
        public void CarpoolConfirmationViewModel_CarpoolConfirmationItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolConfirmationViewModel carpoolConfirmationViewModel = new CarpoolConfirmationViewModel();
            // Assert
            Assert.IsNotNull(carpoolConfirmationViewModel.CarpoolConfirmationItemItemTemplate);
        }

        #endregion

        /****************************************************/
        // CARPOOL DETALS VIEW MODEL
        /****************************************************/
        #region CarpoolDetailsViewModel

        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_PageTypeIsCarpoolRideOwn()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId);
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.AreEqual(CarpoolDetailsPageType.CarpoolRide_Own, carpoolDetailsViewModel.PageType);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_PageTypeIsCarpoolRideLeave()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId != userId);
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.AreEqual(CarpoolDetailsPageType.CarpoolRide_Leave, carpoolDetailsViewModel.PageType);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_PageTypeIsCarpoolRideJoin()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OtherRides.FirstOrDefault(x => !x.CarpoolConfirmations.Any(x2 => x2.PassengerId == userId));
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.AreEqual(CarpoolDetailsPageType.CarpoolRide_Join, carpoolDetailsViewModel.PageType);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_PageTypeIsCarpoolRequestOwn()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OwnRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.AreEqual(CarpoolDetailsPageType.CarpoolRequest_Own, carpoolDetailsViewModel.PageType);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_PageTypeIsCarpoolRequestOther()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OtherRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.AreEqual(CarpoolDetailsPageType.CarpoolRequest_Other, carpoolDetailsViewModel.PageType);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_PageObjectIsCarpoolRide()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.AreEqual(CarpoolDetailsPageObject.CarpoolRide, carpoolDetailsViewModel.PageObject);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_PageObjectIsCarpoolRequest()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OtherRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.AreEqual(CarpoolDetailsPageObject.CarpoolRequest, carpoolDetailsViewModel.PageObject);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_BackCommand_CommandIsExecuted()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Act
            carpoolDetailsViewModel.BackCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            carpoolDetailsViewModel.BackCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_NavigationBarButtonOneIsVisible_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId);
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.NavigationBarButtonOneIsVisible);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_NavigationBarButtonOneIsVisible_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OwnRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.NavigationBarButtonOneIsVisible);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_NavigationBarButtonTwoIsVisible_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId && !x.IsLocked);
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.NavigationBarButtonOneIsVisible);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_NavigationBarButtonTwoIsVisible_IsFalse()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId);
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsFalse(carpoolDetailsViewModel.NavigationBarButtonTwoIsVisible);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_NavigationBarButtonOneIcon_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Assert
            Assert.IsNotNull(carpoolDetailsViewModel.NavigationBarButtonOneIcon);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_NavigationBarButtonTwoIcon_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Assert
            Assert.IsNotNull(carpoolDetailsViewModel.NavigationBarButtonTwoIcon);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_SetCarpoolConfirmations_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Act
            var list = new List<CarpoolConfirmation>();
            carpoolDetailsViewModel.CarpoolConfirmations = list;
            // Assert
            Assert.AreEqual(list,  carpoolDetailsViewModel.CarpoolConfirmations);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_GetCar_IsNotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsNotNull(carpoolDetailsViewModel.Car);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_ShowCar_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.ShowCar);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_ButtonCommand_CommandIsExecuted()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Act
            carpoolDetailsViewModel.ButtonCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            carpoolDetailsViewModel.ButtonCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_NavigationBarTitle_IsRide()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault();
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.AreEqual(GetLocalizedString("ride"), carpoolDetailsViewModel.NavigationBarTitle);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_NavigationBarTitle_IsRequest()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OwnRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.AreEqual(GetLocalizedString("request"), carpoolDetailsViewModel.NavigationBarTitle);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_UserInformationTitle_IsDriver()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault();
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.AreEqual(GetLocalizedString("driver"), carpoolDetailsViewModel.UserInformationTitle);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_UserInformationTitle_IsPassenger()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OwnRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.AreEqual(GetLocalizedString("request"), carpoolDetailsViewModel.NavigationBarTitle);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_ButtonTitle_IsLeaveRide()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId != userId);
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.AreEqual(GetLocalizedString("leave_ride"), carpoolDetailsViewModel.ButtonTitle);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_ButtonTitle_IsJoinRide()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OtherRides.FirstOrDefault(x => !x.CarpoolConfirmations.Any(x2 => x2.PassengerId == userId));
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.AreEqual(GetLocalizedString("join_ride"), carpoolDetailsViewModel.ButtonTitle);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_ButtonTitle_IsInviteToRide()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OwnRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.AreEqual(GetLocalizedString("invite_to_ride"), carpoolDetailsViewModel.ButtonTitle);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_ButtonIcon_IsNotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId != userId);
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsNotNull(carpoolDetailsViewModel.ButtonIcon);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_ButtonIcon_IsNotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OwnRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.IsNotNull(carpoolDetailsViewModel.ButtonIcon);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_ShowButton_IsFalse()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId);
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsFalse(carpoolDetailsViewModel.ShowButton);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_ShowButton_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId != userId);
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.ShowButton);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_ShowButton_IsFalse()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OwnRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.IsFalse(carpoolDetailsViewModel.ShowButton);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_ShowButton_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OtherRequests.FirstOrDefault();
                // Get carpool rides
                var response2 = await service.GetOwnCarpoolRides(userId);
                // Set values
                carpoolDetailsViewModel.CarpoolRides = response2.Result;
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.ShowButton);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_GetDetailsObject_IsNotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OtherRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.IsNotNull(carpoolDetailsViewModel.Details);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_GetUserInformation_IsNotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OtherRequests.FirstOrDefault();
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.IsNotNull(carpoolDetailsViewModel.UserInformation);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_Passenger_IsNotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId && x.CarpoolConfirmations.Any(x2 => x2.IsConfirmed));
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.Passengers.Count > 0);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_ShowPassengers_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId && x.CarpoolConfirmations.Any(x2 => x2.IsConfirmed));
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.ShowPassengers);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRequest_Events_NotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OtherRequests.FirstOrDefault(x => x.CarpoolRequests_Events_Relations.Count > 0);
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.Events.Count > 0);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_Events_IsNotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId && x.CarpoolRides_Events_Relations.Count > 0);
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.Events.Count > 0);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_ShowEvents_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId && x.CarpoolRides_Events_Relations.Count > 0);
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.ShowEvents);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_Comment_IsNotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId && x.Comment != null);
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsNotNull(carpoolDetailsViewModel.Comment);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_SetCarpoolRide_ShowComment_IsTrue()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId && x.Comment != null);
                carpoolRide.IsLocked = true;
                // Set values
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.ShowComment);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public async Task CarpoolDetailsViewModel_CarpoolRidesList_IsNotNull()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OtherRequests.FirstOrDefault(x => x.CarpoolRequests_Events_Relations.Count > 0);
                // Get carpool rides
                var response2 = await service.GetOwnCarpoolRides(userId);
                // Set values
                carpoolDetailsViewModel.CarpoolRides = response2.Result;
                carpoolDetailsViewModel.User = user;
                carpoolDetailsViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.IsTrue(carpoolDetailsViewModel.CarpoolRidesList.Count > 0);
            }
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_CarpoolRideItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Assert
            Assert.IsNotNull(carpoolDetailsViewModel.CarpoolRideListItemTemplate);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_CarpoolRequestItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Assert
            Assert.IsNotNull(carpoolDetailsViewModel.CarpoolRequestItemTemplate);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_UserItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Assert
            Assert.IsNotNull(carpoolDetailsViewModel.UserItemTemplate);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_EventItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Assert
            Assert.IsNotNull(carpoolDetailsViewModel.EventItemTemplate);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_PassengerItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Assert
            Assert.IsNotNull(carpoolDetailsViewModel.PassengerItemTemplate);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_DetailsDataTemplateSelector_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Assert
            Assert.IsNotNull(carpoolDetailsViewModel.DetailsDataTemplateSelector);
        }
        [TestMethod]
        [TestCategory("CarpoolDetailsViewModel")]
        public void CarpoolDetailsViewModel_CarpoolRideListItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolDetailsViewModel carpoolDetailsViewModel = new CarpoolDetailsViewModel();
            // Assert
            Assert.IsNotNull(carpoolDetailsViewModel.CarpoolRideListItemTemplate);
        }

        #endregion

        /****************************************************/
        // CARPOOLING VIEW MODEL
        /****************************************************/
        #region CarpoolingViewModel

        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_SetToggleType_ShowRides_IsTrue()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Act
            carpoolingViewModel.ToggleType = ToggleType.Left;
            // Assert
            Assert.IsTrue(carpoolingViewModel.ShowRides);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_SetToggleType_ShowRequests_IsTrue()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Act
            carpoolingViewModel.ToggleType = ToggleType.Right;
            // Assert
            Assert.IsTrue(carpoolingViewModel.ShowRequests);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_SetRides_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Act
            carpoolingViewModel.Rides = new List<CarpoolRide>();
            // Assert
            Assert.IsNotNull(carpoolingViewModel.Rides);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_SetRequests_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Act
            carpoolingViewModel.Requests = new List<CarpoolRequest>();
            // Assert
            Assert.IsNotNull(carpoolingViewModel.Requests);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_SetCarpoolConfirmations_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Act
            carpoolingViewModel.CarpoolConfirmations = new List<CarpoolConfirmation>();
            // Assert
            Assert.IsNotNull(carpoolingViewModel.CarpoolConfirmations);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_SetCarpoolConfirmations_NavigationBarButtonTwoIcon_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Act
            carpoolingViewModel.CarpoolConfirmations = new List<CarpoolConfirmation>();
            carpoolingViewModel.CarpoolConfirmations.Add(new CarpoolConfirmation());
            // Assert
            Assert.IsNotNull(carpoolingViewModel.NavigationBarButtonTwoIcon);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_CarpoolRideItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Assert
            Assert.IsNotNull(carpoolingViewModel.CarpoolRideItemTemplate);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_CarpoolRequestDividerItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Assert
            Assert.IsNotNull(carpoolingViewModel.CarpoolRequestDividerItemTemplate);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_CarpoolRideDividerItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Assert
            Assert.IsNotNull(carpoolingViewModel.CarpoolRideDividerItemTemplate);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_CarpoolRequestItemTemplate_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Assert
            Assert.IsNotNull(carpoolingViewModel.CarpoolRequestItemTemplate);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_CarpoolRideDataTemplateSelector_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Assert
            Assert.IsNotNull(carpoolingViewModel.CarpoolRideDataTemplateSelector);
        }
        [TestMethod]
        [TestCategory("CarpoolingViewModel")]
        public void CarpoolingViewModel_CarpoolRequestDataTemplateSelector_IsNotNull()
        {
            // Arrange
            CarpoolingViewModel carpoolingViewModel = new CarpoolingViewModel();
            // Assert
            Assert.IsNotNull(carpoolingViewModel.CarpoolRequestDataTemplateSelector);
        }

        #endregion

        /****************************************************/
        // NEW CARPOOLING VIEW MODEL
        /****************************************************/
        #region NewCarpoolingViewModel

        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SetPageType_ButtonIcon_isNotNull()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRide;
            // Assert
            Assert.IsNotNull(newCarpoolingViewModel.ButtonIcon);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SetPageType_ButtonTitle_IsCreateRide()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRide;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("create_this"), GetLocalizedString("ride").ToLower()), newCarpoolingViewModel.ButtonTitle);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SetPageType_ButtonTitle_IsNewRequest()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRequest;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("create_this"), GetLocalizedString("request").ToLower()), newCarpoolingViewModel.ButtonTitle);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SetPageType_ButtonTitle_IsEditRide()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.EditCarpoolRide;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("edit_this"), GetLocalizedString("ride").ToLower()), newCarpoolingViewModel.ButtonTitle);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SetPageType_ButtonTitle_IsEditRequest()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.EditCarpoolRequest;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("edit_this"), GetLocalizedString("request").ToLower()), newCarpoolingViewModel.ButtonTitle);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SetPageType_NavigationBarTitle_IsNewRide()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRide;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("new_this"), GetLocalizedString("ride").ToLower()), newCarpoolingViewModel.NavigationBarTitle);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SetPageType_NavigationBarTitle_IsNewRequest()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRequest;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("new_this"), GetLocalizedString("request").ToLower()), newCarpoolingViewModel.NavigationBarTitle);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SetPageType_NavigationBarTitle_IsEditRide()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.EditCarpoolRide;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("edit_this"), GetLocalizedString("ride").ToLower()), newCarpoolingViewModel.NavigationBarTitle);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SetPageType_NavigationBarTitle_IsEditRequest()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.EditCarpoolRequest;
            // Assert
            Assert.AreEqual(string.Format(GetLocalizedString("edit_this"), GetLocalizedString("request").ToLower()), newCarpoolingViewModel.NavigationBarTitle);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public async Task NewCarpoolingViewModel_SetCarpoolRide_ValuesAreSet()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool rides
                var response = await service.GetUsersCarpoolRides(userId);
                // Get carpool ride
                var carpoolRide = response.Result.OwnRides.FirstOrDefault(x => x.DriverId == userId && x.Comment != null);
                // Set values
                newCarpoolingViewModel.CarpoolRide = carpoolRide;
                // Assert
                Assert.IsTrue(
                    newCarpoolingViewModel.Comment != null &&
                    newCarpoolingViewModel.DepartureDate != default(DateTime) && 
                    newCarpoolingViewModel.DepartureTime != default(TimeSpan) &&
                    newCarpoolingViewModel.SelectedEvents != null &&
                    newCarpoolingViewModel.Car != null &&
                    newCarpoolingViewModel.PricePerPassenger == carpoolRide.PricePerPassenger &&
                    newCarpoolingViewModel.PricePerPassengerString != null &&
                    newCarpoolingViewModel.NumberOfSeats != 0 &&
                    newCarpoolingViewModel.NumberOfSeatsString != null &&
                    newCarpoolingViewModel.PickupPlace != null &&
                    newCarpoolingViewModel.DestinationPlace != null
                );
            }
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public async Task NewCarpoolingViewModel_SetCarpoolRequest_ValuesAreSet()
        {
            using (var factory = new NSSDatabaseContextFactory())
            {
                // Arrange
                IDataService service = new NSSDatabaseService<NSSDatabaseContext>(() => factory.CreateContext());
                NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
                // Act
                int userId = 1;
                // Get user
                var resp = await service.GetUser(userId);
                // User result
                var user = resp.Result;
                // Get user's carpool requests
                var response = await service.GetUsersCarpoolRequests(userId);
                // Get carpool request
                var carpoolRequest = response.Result.OwnRequests.FirstOrDefault();
                // Set values
                newCarpoolingViewModel.CarpoolRequest = carpoolRequest;
                // Assert
                Assert.IsTrue(
                    newCarpoolingViewModel.Comment != null &&
                    newCarpoolingViewModel.RequestDate != default(DateTime) &&
                    newCarpoolingViewModel.FromTime != default(TimeSpan) &&
                    newCarpoolingViewModel.ToTime != default(TimeSpan) &&
                    newCarpoolingViewModel.SelectedEvents != null &&
                    newCarpoolingViewModel.RequestPickupPlace != null &&
                    newCarpoolingViewModel.RequestDestinationPlace != null
                );
            }
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_IsNumberOfSeatsReadOnly_IsTrue()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.EditCarpoolRide;
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.IsNumberOfSeatsReadOnly);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_ShowToggleButton_IsTrue()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRide;
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.ShowToggleButton);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_ShowToggleButton_IsFalse()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.EditCarpoolRide;
            // Assert
            Assert.IsFalse(newCarpoolingViewModel.ShowToggleButton);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_AllDataGiven_IsTrue()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRide;
            newCarpoolingViewModel.ToggleType = ToggleType.Left;

            newCarpoolingViewModel.SetDefaultDestinationPlace();
            newCarpoolingViewModel.PickupPlace = new DurianCode.PlacesSearchBar.Place("test", "1234", "Testby");
            newCarpoolingViewModel.Car = new Car();
            newCarpoolingViewModel.NumberOfSeats = 4;
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.AllDataGiven);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_AllDataGiven_IsFalse()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRide;
            newCarpoolingViewModel.ToggleType = ToggleType.Left;

            newCarpoolingViewModel.SetDefaultDestinationPlace();
            newCarpoolingViewModel.PickupPlace = new DurianCode.PlacesSearchBar.Place("test", "1234", "Testby");
            newCarpoolingViewModel.Car = new Car();
            newCarpoolingViewModel.NumberOfSeats = 0;
            // Assert
            Assert.IsFalse(newCarpoolingViewModel.AllDataGiven);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_ShowRide_IsTrue()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRide;
            newCarpoolingViewModel.ToggleType = ToggleType.Left;
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.ShowRide);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_ShowRequest_IsTrue()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.PageType = NewCarpoolingPageType.NewCarpoolRide;
            newCarpoolingViewModel.ToggleType = ToggleType.Right;
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.ShowRequest);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_Events_IsNotNull()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.Events = new System.Collections.ObjectModel.ObservableCollection<Event>();
            // Assert
            Assert.IsNotNull(newCarpoolingViewModel.Events);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_EventListItemTemplate_IsNotNull()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Assert
            Assert.IsNotNull(newCarpoolingViewModel.EventListItemTemplate);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SelectedEvents_IsNotNull()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.SelectedEvents = new System.Collections.ObjectModel.ObservableCollection<Event>();
            // Assert
            Assert.IsNotNull(newCarpoolingViewModel.SelectedEvents);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_EventsSelected_IsTrue()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.SelectedEvents = new System.Collections.ObjectModel.ObservableCollection<Event>();
            newCarpoolingViewModel.SelectedEvents.Add(new Event() { Id = 1, Name = "test" });
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.EventsSelected);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_EventSelectedItemTemplate_IsNotNull()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Assert
            Assert.IsNotNull(newCarpoolingViewModel.EventSelectedItemTemplate);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_DateToday_IsNotNull()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.DateToday != default);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_SelectEventsCommand_CommandIsExecuted()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.SelectEventsCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            newCarpoolingViewModel.SelectEventsCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_EventDeleteCommand_CommandIsExecuted()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.EventDeleteCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            newCarpoolingViewModel.EventDeleteCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_CarInfo_IsNotNull()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.Car = new Car() { Id = 1, Color = "grøn", LicensePlate = "1234567" };
            // Assert
            Assert.IsNotNull(newCarpoolingViewModel.CarInfo);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_CarDeleteCommand_CommandIsExecuted()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.CarDeleteCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            newCarpoolingViewModel.CarDeleteCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_CarEditCommand_CommandIsExecuted()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.CarEditCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            newCarpoolingViewModel.CarEditCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_Cars_IsNotNull()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.Cars = new System.Collections.ObjectModel.ObservableCollection<Car>();
            // Assert
            Assert.IsNotNull(newCarpoolingViewModel.Cars);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_CarItemTemplate_IsNotNull()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Assert
            Assert.IsNotNull(newCarpoolingViewModel.CarItemTemplate);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_ButtonCommand_CommandIsExecuted()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            // Act
            newCarpoolingViewModel.ButtonCommand = new Command(() =>
            {
                // Assert
                Assert.IsTrue(true);
            });
            newCarpoolingViewModel.ButtonCommand.Execute(null);
            // Assert
            Assert.IsFalse(false);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_CompleteDepartureTime_IsTheSame()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            var dateTime = DateTime.Now;
            // Act
            newCarpoolingViewModel.DepartureDate = dateTime;
            newCarpoolingViewModel.DepartureTime = dateTime.TimeOfDay;
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.CompleteDepartureTime.Value.CompareTo(dateTime) == 0);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_CompleteFromTime_IsTheSame()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            var dateTime = DateTime.Now;
            // Act
            newCarpoolingViewModel.RequestDate = dateTime;
            newCarpoolingViewModel.FromTime = dateTime.TimeOfDay;
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.CompleteFromTime.Value.CompareTo(dateTime) == 0);
        }
        [TestMethod]
        [TestCategory("NewCarpoolingViewModel")]
        public void NewCarpoolingViewModel_CompleteToTime_IsTheSame()
        {
            // Arrange
            NewCarpoolingViewModel newCarpoolingViewModel = new NewCarpoolingViewModel();
            var dateTime = DateTime.Now;
            // Act
            newCarpoolingViewModel.RequestDate = dateTime;
            newCarpoolingViewModel.ToTime = dateTime.TimeOfDay;
            // Assert
            Assert.IsTrue(newCarpoolingViewModel.CompleteToTime.Value.CompareTo(dateTime) == 0);
        }

        #endregion

        #endregion

        /*****************************************************************/
        // EXTENSIONS
        /*****************************************************************/
        #region Extensions

        /****************************************************/
        // DATETIME EXTENSIONS
        /****************************************************/
        #region DateTime Extensions

        [TestMethod]
        [TestCategory("DateTimeExtensions")]
        public void DateTimeExtensions_ToCarpoolingFormat_FirstFormat()
        {
            // Arrange
            var dateTime = DateTime.Now;
            // Act
            var str = dateTime.ToCarpoolingFormat();
            // Assert
            Assert.AreEqual(dateTime.ToString("dddd"), str);
        }
        [TestMethod]
        [TestCategory("DateTimeExtensions")]
        public void DateTimeExtensions_ToCarpoolingFormat_SecondFormat()
        {
            // Arrange
            var dateTime = DateTime.Now.AddDays(10);
            // Act
            var str = dateTime.ToCarpoolingFormat();
            // Assert
            Assert.AreEqual(dateTime.ToString("dd-MM-yy"), str);
        }

        #endregion

        /****************************************************/
        // STRING EXTENSIONS
        /****************************************************/
        #region String Extensions

        [TestMethod]
        [TestCategory("DateTimeExtensions")]
        public void StringExtensions_FirstCharToUpper_Successful()
        {
            // Arrange
            var str = "hello";
            // Act
            var str2 = str.FirstCharToUpper();
            // Assert
            Assert.AreEqual("Hello", str2);
        }

        #endregion

        #endregion
    }
}
