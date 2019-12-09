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

    public class ConfirmationsResult
    {
        public List<CarpoolRide> OwnRides { get; set; }
        public List<CarpoolRide> OtherRides { get; set; }
    }

    public interface IDataService
    {
        void Initialize();
        void GetData<T>(string progressMessage, bool showDialog, Func<Task<T>> task, Action<T> resultCallback, int showDialogDelay = 500);

        Task<DataResponse<List<User>>> GetUsers();
        Task<DataResponse<List<Gender>>> GetGenders();
        Task<DataResponse<List<CarpoolRide>>> GetCarpoolRides();
        Task<DataResponse<List<CarpoolRequest>>> GetCarpoolRequests();
        Task<DataResponse<List<CarpoolConfirmation>>> GetCarpoolConfirmations();
        Task<DataResponse<List<Event>>> GetEvents();
        Task<DataResponse<List<Car>>> GetCars();
        Task<DataResponse<List<OpeningHour>>> GetOpeningHours();
        Task<DataResponse<ContactInfo>> GetContactInfo();

        Task<DataResponse<ConfirmationsResult>> GetConfirmations(int userId);
        Task<DataResponse<List<Car>>> GetCars(int userId);
        Task<DataResponse<string>> GetOpeningHoursInformation();
        Task<DataResponse<string>> GetTodaysOpeningHours();
        Task<DataResponse<CarpoolRide>> GetNextCarpoolRide();
        Task<DataResponse<List<CarpoolConfirmation>>> GetCarpoolConfirmations(int userId);
        Task<DataResponse> CheckIfPhoneIsNotUsedAlready(string phoneNo);
        Task<DataResponse<User>> CheckLogin(string phoneNo);
        Task<DataResponse<User>> GetUser(int userId);
        Task<DataResponse> UpdateUser(int userId, string firstName, string lastName, string phoneNo, int age, int genderId);
        Task<DataResponse<User>> SignUpUser(string firstName, string lastName, string phoneNo, int age, int genderId);
        Task<DataResponse> DeleteUser(int userId);
        Task<DataResponse<Car>> CreateCar(int userId, string licensePlate, string color);
        Task<DataResponse> DeleteCar(int carId);
        Task<DataResponse<CarpoolRide>> CreateCarpoolRide(int userId, DateTime departureTime, string address, string zipCode, string city, string destinationAddress, string destinationZipCode, string destinationCity, int carId, int numberOfSeats, int pricePerPassenger, List<Event> events, string comment = null);
        Task<DataResponse<CarpoolRequest>> CreateCarpoolRequest(int userId, DateTime fromTime, DateTime toTime, string zipCode, string city, List<Event> events);
        Task<DataResponse> InvitePassenger(int carpoolRequestId, int carpoolRideId);
        Task<DataResponse> UninvitePassenger(int carpoolConfirmationId);
        Task<DataResponse> SignUpToCarpoolRide(int carpoolRideId, int userId);
        Task<DataResponse> UnsignFromCarpoolRide(int carpoolConfirmationId);
        Task<DataResponse> AnswerCarpoolConfirmation(int userId, int carpoolConfirmationId, bool accept);
    }
}
