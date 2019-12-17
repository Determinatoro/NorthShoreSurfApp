using NorthShoreSurfApp.ModelComponents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NorthShoreSurfApp
{
    public class DataResponse
    {
        // 0 = No error
        // 1 = Exception
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }

        public DataResponse(int errorCode, string errorMessage)
        {
            Success = false;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public DataResponse(bool success)
        {
            Success = success;
            ErrorCode = 0;
            ErrorMessage = null;
        }
        public DataResponse()
        {
            
        }
    }

    public class DataResponse<T> : DataResponse
    {
        public T Result { get; set; }

        public DataResponse(bool success, T result)
        {
            Success = success;
            ErrorCode = 0;
            ErrorMessage = null;
            Result = result;
        }

        public DataResponse(int errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
            Result = default(T);
        }
    }

    public class CarpoolResult
    {
        public CarpoolResult()
        {
            OwnRides = new List<CarpoolRide>();
            OtherRides = new List<CarpoolRide>();
        }

        public List<CarpoolRide> OwnRides { get; set; }
        public List<CarpoolRide> OtherRides { get; set; }
    }

    public class RequestResult
    {
        public RequestResult()
        {
            OwnRequests = new List<CarpoolRequest>();
            OtherRequests = new List<CarpoolRequest>();
        }

        public List<CarpoolRequest> OwnRequests { get; set; }
        public List<CarpoolRequest> OtherRequests { get; set; }
    }

    public interface IDataService
    {
        /// <summary>
        /// Initialize the service
        /// </summary>
        void Initialize();
        /// <summary>
        /// Get data
        /// </summary>
        /// <typeparam name="T">The object response expected</typeparam>
        /// <param name="progressMessage">A message to show when getting data</param>
        /// <param name="showDialog">Flag for showing a dialog while getting data</param>
        /// <param name="task">The task to be carried out</param>
        /// <param name="resultCallback">Callback for the result object</param>
        /// <param name="showDialogDelay">A delay for when to show the dialog</param>
        /// <param name="errorCallback">Callback if any unhandled errors happen in the task</param>
        void GetData<T>(string progressMessage, bool showDialog, Func<Task<T>> task, Action<T> resultCallback, int showDialogDelay = 500, Action<DataResponse> errorCallback = null);

        /// <summary>
        /// Get all the users in the database
        /// </summary>
        /// <returns>A list of User objects</returns>
        Task<DataResponse<List<User>>> GetUsers();
        /// <summary>
        /// Get the events in the database
        /// </summary>
        /// <returns>A list of Event objects</returns>
        Task<DataResponse<List<Event>>> GetEvents();
        /// <summary>
        /// Get all the objects containing information about the opening hours at the company
        /// </summary>
        /// <returns>A list of OpeningHour objects</returns>
        Task<DataResponse<List<OpeningHour>>> GetOpeningHours();
        /// <summary>
        /// Get a single object containing the contact info for the company
        /// </summary>
        /// <returns>A ContactInfo object</returns>
        Task<DataResponse<ContactInfo>> GetContactInfo();
        /// <summary>
        /// Get all the carpool rides related to the user split into own and others
        /// </summary>
        /// <param name="userId">the user's id</param>
        /// <returns>A list of CarpoolRide objects</returns>
        Task<DataResponse<CarpoolResult>> GetUsersCarpoolRides(int userId);
        /// <summary>
        /// Get all the carpool requests related to the user split into own and others
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns>A result containing two lists own and others</returns>
        Task<DataResponse<RequestResult>> GetUsersCarpoolRequests(int userId);
        /// <summary>
        /// Get all the user own carpool rides
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns>A result containing two lists own and others</returns>
        Task<DataResponse<List<CarpoolRide>>> GetOwnCarpoolRides(int userId);
        /// <summary>
        /// Get the carpool rides that are related to the user. All the carpool rides will contain information about the pending confirmations
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns>An object containing a list with the user's own carpool rides and a list with the carpool rides that are related to the user</returns>
        Task<DataResponse<CarpoolResult>> GetConfirmations(int userId);
        /// <summary>
        /// Get all the user's cars
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns>A list of car objects</returns>
        Task<DataResponse<List<Car>>> GetCars(int userId);
        /// <summary>
        /// Get a describing text about the openings hours at the company
        /// </summary>
        /// <returns></returns>
        Task<DataResponse<string>> GetOpeningHoursInformation();
        /// <summary>
        /// Get todays openings hours as a single line ex. "10.00-16.00" or "Closed"
        /// </summary>
        /// <returns>A string</returns>
        Task<DataResponse<string>> GetTodaysOpeningHours();
        /// <summary>
        /// Get next carpool ride that is not one of the user's
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns>A carpool ride if one is found else null</returns>
        Task<DataResponse<CarpoolRide>> GetNextCarpoolRide(int? userId);
        /// <summary>
        /// Get all the carpool confirmations that are related to the user
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns>A list of CarpoolConfirmation objects</returns>
        Task<DataResponse<List<CarpoolConfirmation>>> GetCarpoolConfirmations(int userId);
        /// <summary>
        /// Get all the carpool confirmations that are related to the user and not confirmed
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        Task<DataResponse<List<CarpoolConfirmation>>> GetPendingCarpoolConfirmations(int userId);
        /// <summary>
        /// Check if a phone no is already used by a user in the database
        /// </summary>
        /// <param name="phoneNo">Given phone no.</param>
        /// <returns>True if phone no. was not found else false</returns>
        Task<DataResponse> CheckIfPhoneIsNotUsedAlready(string phoneNo);
        /// <summary>
        /// Check if the phone exist in the database
        /// </summary>
        /// <param name="phoneNo">The given phone no.</param>
        /// <returns>True if the phone no. is in the database else false</returns>
        Task<DataResponse<User>> CheckLogin(string phoneNo);
        /// <summary>
        /// Get the user's information
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns>A User object</returns>
        Task<DataResponse<User>> GetUser(int userId);
        /// <summary>
        /// Update the user's information
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <param name="firstName">The user's first name</param>
        /// <param name="lastName">The user's last name</param>
        /// <param name="phoneNo">The user's phone no.</param>
        /// <param name="age">The user's age</param>
        /// <param name="genderId">Id for the user's gender</param>
        /// <returns>True if succeeded else false</returns>
        Task<DataResponse> UpdateUser(int userId, string firstName, string lastName, string phoneNo, int age, int genderId);
        /// <summary>
        /// Sign up a new user
        /// </summary>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="phoneNo">Phone no.</param>
        /// <param name="age">Age</param>
        /// <param name="genderId">Id for the gender</param>
        /// <returns>A User object</returns>
        Task<DataResponse<User>> SignUpUser(string firstName, string lastName, string phoneNo, int age, int genderId);
        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns>Tru if succeeded else false</returns>
        Task<DataResponse> DeleteUser(int userId);
        /// <summary>
        /// Update car values
        /// </summary>
        /// <param name="carId">Car's id</param>
        /// <param name="licensePlate">Car's licenseplate</param>
        /// <param name="color">Car's color</param>
        /// <returns></returns>
        Task<DataResponse<Car>> UpdateCar(int carId, string licensePlate, string color);
        /// <summary>
        /// Create a new car for a user
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <param name="licensePlate">The car's license plate</param>
        /// <param name="color">The car's color as a string</param>
        /// <returns>A Car object</returns>
        Task<DataResponse<Car>> CreateCar(int userId, string licensePlate, string color);
        /// <summary>
        /// Delete a car
        /// </summary>
        /// <param name="carId">The car's id</param>
        /// <returns>True if succeeded else false</returns>
        Task<DataResponse> DeleteCar(int carId);
        /// <summary>
        /// Create a new carpool ride for a user
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <param name="departureTime">Departure time for the carpool ride</param>
        /// <param name="address">The name of the road and road no. where the departure will be</param>
        /// <param name="zipCode">Zipcode from where the departure will be</param>
        /// <param name="city">City from where the departure will be</param>
        /// <param name="destinationAddress">The name of the road and road no. of the destination</param>
        /// <param name="destinationZipCode">The zipcode of the destination</param>
        /// <param name="destinationCity">The city of the destination</param>
        /// <param name="carId">Id for the car that will be used</param>
        /// <param name="numberOfSeats">Number of total available seats that the user would like to have occupied</param>
        /// <param name="pricePerPassenger">Price per passenger attending the carpool ride</param>
        /// <param name="events">A list of events that the driver would like to attend</param>
        /// <param name="comment">An optional comment for the carpool ride</param>
        /// <returns>A CarpoolRide object</returns>
        Task<DataResponse<CarpoolRide>> CreateCarpoolRide(int userId, DateTime departureTime, string address, string zipCode, string city, string destinationAddress, string destinationZipCode, string destinationCity, int carId, int numberOfSeats, int pricePerPassenger, List<Event> events, string comment = null);
        /// <summary>
        /// Delete carpool ride
        /// </summary>
        /// <param name="carpoolRideId">The carpool ride's id</param>
        /// <returns>True if succeeded else false</returns>
        Task<DataResponse> DeleteCarpoolRide(int carpoolRideId);
        /// <summary>
        /// Delete carpool request
        /// </summary>
        /// <param name="carpoolRequestId">The carpool request's id</param>
        /// <returns>True if succeeded else false</returns>
        Task<DataResponse> DeleteCarpoolRequest(int carpoolRequestId);
        /// <summary>
        /// Create a new carpool request for a user
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <param name="fromTime">From time on the day for the request</param>
        /// <param name="toTime">To time on the day for the request</param>
        /// <param name="zipCode">Zipcode for the area the user would want to attend a carpool ride</param>
        /// <param name="city">City for the area the user would want to attend a carpool ride</param>
        /// <param name="destinationAddress">Destination road name and road no. for the request</param>
        /// <param name="destinationZipCode">Destiation zipcode for the request</param>
        /// <param name="destinationCity">Destination city for the request</param>
        /// <param name="events">A list of events that the user would like to attend</param>
        /// <param name="comment">An optional comment for the request</param>
        /// <returns>A CarpoolRequest object</returns>
        Task<DataResponse<CarpoolRequest>> CreateCarpoolRequest(int userId, DateTime fromTime, DateTime toTime, string zipCode, string city, string destinationAddress, string destinationZipCode, string destinationCity, List<Event> events, string comment = null);
        /// <summary>
        /// Invite a passenger to a carpool ride
        /// </summary>
        /// <param name="carpoolRequestId">The carpool request's id</param>
        /// <param name="carpoolRideId">The carpool ride's id</param>
        /// <returns>A CarpoolConfirmation object</returns>
        Task<DataResponse<CarpoolConfirmation>> InvitePassenger(int carpoolRequestId, int carpoolRideId);
        /// <summary>
        /// Sign up to a carpool ride for a user
        /// </summary>
        /// <param name="carpoolRideId">The carpool ride's id</param>
        /// <param name="userId">The user's id</param>
        /// <returns>A CarpoolConfirmation object</returns>
        Task<DataResponse<CarpoolConfirmation>> SignUpToCarpoolRide(int carpoolRideId, int userId);
        /// <summary>
        /// Unsign a user from a carpool ride
        /// </summary>
        /// <param name="carpoolRideId">The carpool ride's id</param>
        /// <param name="userId">The user's id</param>
        /// <returns>A CarpoolConfirmation object</returns>
        Task<DataResponse<List<CarpoolConfirmation>>> UnsignFromCarpoolRide(int carpoolRideId, int userId);
        /// <summary>
        /// Answer a carpool confirmation for a user
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <param name="carpoolConfirmationId">The carpool confirmation's id</param>
        /// <param name="accept">True if accepted, false if denied</param>
        /// <returns></returns>
        Task<DataResponse> AnswerCarpoolConfirmation(int userId, int carpoolConfirmationId, bool accept);
        /// <summary>
        /// Update information in a carpool request
        /// </summary>
        /// <param name="carpoolRequestId">The carpool request's id</param>
        /// <param name="fromTime">From time on the day for the request</param>
        /// <param name="toTime">To time on the day for the request</param>
        /// <param name="zipCode">Zipcode for the area the user would want to attend a carpool ride</param>
        /// <param name="city">City for the area the user would want to attend a carpool ride</param>
        /// <param name="destinationAddress">Destination road name and road no. for the request</param>
        /// <param name="destinationZipCode">Destiation zipcode for the request</param>
        /// <param name="destinationCity">Destination city for the request</param>
        /// <param name="events">A list of events that the user would like to attend</param>
        /// <param name="comment">An optional comment for the request</param>
        /// <returns></returns>
        Task<DataResponse<CarpoolRequest>> UpdateCarpoolRequest(int carpoolRequestId, DateTime fromTime, DateTime toTime, string zipCode, string city, string destinationAddress, string destinationZipCode, string destinationCity, List<Event> events, string comment = null);
        /// <summary>
        /// Update information in a carpool ride. Number of seats cannot be updated
        /// </summary>
        /// <param name="carpoolRideId">The carpool ride's id</param>
        /// <param name="departureTime">Departure time for the carpool ride</param>
        /// <param name="address">The name of the road and road no. where the departure will be</param>
        /// <param name="zipCode">Zipcode from where the departure will be</param>
        /// <param name="city">City from where the departure will be</param>
        /// <param name="destinationAddress">The name of the road and road no. of the destination</param>
        /// <param name="destinationZipCode">The zipcode of the destination</param>
        /// <param name="destinationCity">The city of the destination</param>
        /// <param name="carId">Id for the car that will be used</param>
        /// <param name="pricePerPassenger">Price per passenger attending the carpool ride</param>
        /// <param name="events">A list of events that the driver would like to attend</param>
        /// <param name="comment">An optional comment for the carpool ride</param>
        /// <returns></returns>
        Task<DataResponse<CarpoolRide>> UpdateCarpoolRide(int carpoolRideId, DateTime departureTime, string address, string zipCode, string city, string destinationAddress, string destinationZipCode, string destinationCity, int carId, int pricePerPassenger, List<Event> events, string comment = null);
    }
}
