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

    public interface IDataService
    {
        void Initialize();
        void GetData<T>(string progressMessage, bool showDialog, Func<Task<T>> task, Action<T> resultCallback);

        Task<DataResponse<List<User>>> GetUsers();
        Task<DataResponse<List<Gender>>> GetGenders();
        Task<DataResponse<List<CarpoolEvent>>> GetCarpoolEvents();
        Task<DataResponse<List<CarpoolRequest>>> GetCarpoolRequests();
        Task<DataResponse<List<CarpoolConfirmation>>> GetCarpoolConfirmations();
        Task<DataResponse<List<Event>>> GetEvents();
        Task<DataResponse<List<Car>>> GetCars();
        Task<DataResponse<List<OpeningHour>>> GetOpeningHours();
        Task<DataResponse<ContactInfo>> GetContactInfo();

        Task<DataResponse> CheckPhoneNo(string phoneNo);
        Task<DataResponse<User>> GetUser(string phoneNo);
        Task<DataResponse> UpdateUser(int userId, string firstName, string lastName, string phoneNo, int age, int genderId); 
        Task<DataResponse<User>> SignUpUser(string firstName, string lastName, string phoneNo, int age, int genderId);
        Task<DataResponse> DeleteUser(string phoneNo);        
        Task<DataResponse<Car>> CreateCar(int userId, string licensePlate, string color);
        Task<DataResponse<CarpoolEvent>> CreateCarpoolEvent(int userId, DateTime departureTime, string address, string zipCode, string city, int carId, int numberOfSeats, int pricePerPassenger, List<Event> events, string comment = null);
        Task<DataResponse<CarpoolRequest>> CreateCarpoolRequest(int userId, DateTime fromTime, DateTime toTime, string zipCode, string city, List<Event> events);
        Task<DataResponse> InvitePassenger(int carpoolRequestId, int carpoolEventId);
        Task<DataResponse> UninvitePassenger(int carpoolConfirmationId);
        Task<DataResponse> SignUpToCarpoolEvent(int carpoolEventId, int userId);
        Task<DataResponse> UnsignFromCarpoolEvent(int carpoolConfirmationId);
        Task<DataResponse> AnswerCarpoolConfirmation(int userId, int carpoolConfirmationId, bool accept);
    }
}
