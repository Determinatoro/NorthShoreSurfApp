using Microsoft.EntityFrameworkCore;
using NorthShoreSurfApp.ModelComponents;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NorthShoreSurfApp.Database
{
    public class NSSDatabaseService<T> : IDataService where T : NSSDatabaseContext
    {
        protected NSSDatabaseContext CreateContext()
        {
            NSSDatabaseContext context = (T)Activator.CreateInstance(typeof(T));
            context.Database.EnsureCreated();
            context.Database.Migrate();
            return context;
        }

        /// <summary>
        /// Initialize Entity Framework context
        /// </summary>
        public void Initialize()
        {
            using (var context = CreateContext())
            {
                var model = context.Model;
            }
        }

        public void GetData<T1>(string progressMessage, bool showDialog, Func<Task<T1>> task, Action<T1> resultCallback)
        {
            // Create dialog
            CustomDialog customDialog = new CustomDialog(CustomDialogType.Progress, progressMessage);
            Timer timer = null;
            CancellationTokenSource cancellationToken = new CancellationTokenSource();

            // Create new thread
            Task.Run(async () =>
            {
                // Show dialog?
                if (showDialog)
                {
                    // Timer thats waits before showing progress dialog
                    timer = new Timer((state) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            // Set cancel event
                            customDialog.Canceled += (sender, args) =>
                            {
                                cancellationToken.Cancel();
                            };
                            // Show dialog
                            PopupNavigation.Instance.PushAsync(customDialog, true);
                        });
                    },
                    null,
                    // Wait time before timer runs
                    500,
                    // No interval
                    Timeout.Infinite);
                }

                try
                {
                    // Get response
                    var response = await task();
                    // Cancel requested
                    if (cancellationToken.IsCancellationRequested)
                        cancellationToken.Token.ThrowIfCancellationRequested();
                    // Run on UI thread
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // Return result to caller
                        resultCallback(response);
                    });
                }
                catch
                {
                    // Task cancelled
                }

                // Is a timer created
                if (timer != null)
                {
                    // Stop timer
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                    timer.Dispose();
                }

                // Run on UI thread
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Close dialog
                    if (PopupNavigation.Instance.PopupStack.Contains(customDialog))
                        await PopupNavigation.Instance.RemovePageAsync(customDialog);
                });
            }, cancellationToken.Token);
        }
        public async Task<DataResponse<List<CarpoolConfirmation>>> GetCarpoolConfirmations()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get all carpool confirmations
                    var carpoolConfirmations = await context.CarpoolConfirmations
                                        .Include(x => x.Passenger)
                                        .Include(x => x.CarpoolRide)
                                        .AsNoTracking()
                                        .ToListAsync();
                    // Return response
                    return new DataResponse<List<CarpoolConfirmation>>(true, carpoolConfirmations);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<List<CarpoolConfirmation>>(1, mes.Message);
            }
        }
        public async Task<DataResponse<List<CarpoolRide>>> GetCarpoolRides()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get all carpool events
                    var events = await context.CarpoolRides
                                        .Include(x => x.Driver)
                                        .Include(x => x.Car)
                                        .Include(x => x.CarpoolConfirmations)
                                        .Include(x => x.CarpoolRides_Events_Relations).ThenInclude(x => x.Event)
                                        .AsNoTracking()
                                        .ToListAsync();
                    // Return response
                    return new DataResponse<List<CarpoolRide>>(true, events);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<List<CarpoolRide>>(1, mes.Message);
            }
        }
        public async Task<DataResponse<List<CarpoolRequest>>> GetCarpoolRequests()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get all requests
                    var requests = await context.CarpoolRequests
                                        .Include(x => x.Passenger)
                                        .AsNoTracking()
                                        .ToListAsync();
                    // Return response
                    return new DataResponse<List<CarpoolRequest>>(true, requests);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<List<CarpoolRequest>>(1, mes.Message);
            }
        }
        public async Task<DataResponse<List<Car>>> GetCars()
        {
            try
            {
                using (var context = CreateContext())
                {
                    Thread.Sleep(5000);
                    // Get all cars
                    var cars = await context.Cars
                                        .Include(x => x.User)
                                        .AsNoTracking()
                                        .ToListAsync();
                    // Return response
                    return new DataResponse<List<Car>>(true, cars);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<List<Car>>(1, mes.Message);
            }
        }
        public async Task<DataResponse<List<Event>>> GetEvents()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get all events
                    var events = await context.Events
                                        .AsNoTracking()
                                        .ToListAsync();
                    // Return response
                    return new DataResponse<List<Event>>(true, events);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<List<Event>>(1, mes.Message);
            }
        }
        public async Task<DataResponse<List<Gender>>> GetGenders()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get all genders
                    var genders = await context.Genders
                                        .AsNoTracking()
                                        .ToListAsync();
                    // Return response
                    return new DataResponse<List<Gender>>(true, genders);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<List<Gender>>(1, mes.Message);
            }
        }
        public async Task<DataResponse<List<User>>> GetUsers()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get all users
                    var users = await context.Users
                                        .Include(x => x.Gender)
                                        .Include(x => x.Cars)
                                        .AsNoTracking()
                                        .ToListAsync();
                    // Return response
                    return new DataResponse<List<User>>(true, users);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<List<User>>(1, mes.Message);
            }
        }
        private async Task<DataResponse<User>> CheckUser(int userId)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get user from user id
                    User user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    // User does not exist
                    if (user == null)
                        return new DataResponse<User>(100, Resources.AppResources.user_not_found);
                    // User not active
                    if (!user.IsActive)
                        return new DataResponse<User>(100, Resources.AppResources.user_is_not_active);
                    // Return response
                    return new DataResponse<User>(true, user);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<User>(1, mes.Message);
            }
        }
        public async Task<DataResponse<List<CarpoolConfirmation>>> GetCarpoolConfirmations(int userId)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Check user
                    var response = await CheckUser(userId);
                    // Something wrong with user
                    if (!response.Success)
                        return new DataResponse<List<CarpoolConfirmation>>(response.ErrorCode, response.ErrorMessage);

                    // Get all carpool confirmations
                    var carpoolConfirmations = await context.CarpoolConfirmations
                                        .Include(x => x.Passenger)
                                        .Include(x => x.CarpoolRide).ThenInclude(x => x.Driver)
                                        .Where(x => x.Passenger.Id == userId || x.CarpoolRide.Driver.Id == userId)
                                        .AsNoTracking()
                                        .ToListAsync();
                    // Return response
                    return new DataResponse<List<CarpoolConfirmation>>(true, carpoolConfirmations);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<List<CarpoolConfirmation>>(1, mes.Message);
            }
        }
        public async Task<DataResponse> CheckIfPhoneIsNotUsedAlready(string phoneNo)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Check if phone no exist already
                    var phoneNoAlreadyInDatabase = await context.Users.AnyAsync(x => x.PhoneNo == phoneNo);
                    // Return response
                    if (phoneNoAlreadyInDatabase)
                        return new DataResponse(101, Resources.AppResources.user_already_exist);
                    // Return response
                    return new DataResponse(true);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<User>(1, mes.Message);
            }
        }
        public async Task<DataResponse<User>> GetUser(int userId)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get user from user id
                    User user = await context.Users
                        .Include(x => x.Gender)
                        .FirstOrDefaultAsync(x => x.Id == userId);
                    // Return response
                    return new DataResponse<User>(user != null, user);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<User>(1, mes.Message);
            }
        }
        public async Task<DataResponse> UpdateUser(int userId, string firstName, string lastName, string phoneNo, int age, int genderId)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get user from Id
                    User user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    // If user not found return error
                    if (user == null)
                        return new DataResponse(100, Resources.AppResources.user_not_found);

                    // Update data
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.PhoneNo = phoneNo;
                    user.Age = age;
                    user.GenderId = genderId;

                    // Save changes
                    await context.SaveChangesAsync();
                    // Return response
                    return new DataResponse(true);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse(1, mes.Message);
            }
        }
        public async Task<DataResponse<User>> SignUpUser(string firstName, string lastName, string phoneNo, int age, int genderId)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Check phone no.
                    DataResponse response = await CheckIfPhoneIsNotUsedAlready(phoneNo);
                    // User already exist
                    if (!response.Success)
                        return new DataResponse<User>(response.ErrorCode, response.ErrorMessage);
                    // Create user
                    User user = new User()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNo = phoneNo,
                        Age = age,
                        GenderId = genderId,
                        IsActive = true
                    };
                    // Add new user
                    var entry = await context.Users.AddAsync(user);
                    // Save changes
                    await context.SaveChangesAsync();
                    // Return response
                    return new DataResponse<User>(true, entry.Entity);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<User>(1, mes.Message);
            }
        }
        public async Task<DataResponse> DeleteUser(string phoneNo)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get user from phone no.
                    User user = await context.Users.FirstOrDefaultAsync(x => x.PhoneNo == phoneNo);
                    // User not found
                    if (user == null)
                        return new DataResponse<User>(100, Resources.AppResources.user_not_found);
                    // Set user to inactive
                    user.IsActive = false;
                    // Save changes
                    await context.SaveChangesAsync();
                    // Return response
                    return new DataResponse(true);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<User>(1, mes.Message);
            }
        }
        public async Task<DataResponse<Car>> CreateCar(int userId, string licensePlate, string color)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Create car
                    Car car = new Car()
                    {
                        UserId = userId,
                        IsActive = true,
                        LicensePlate = licensePlate,
                        Color = color
                    };
                    // Add new car
                    var entry = await context.Cars.AddAsync(car);
                    // Save changes
                    await context.SaveChangesAsync();
                    // Return response
                    return new DataResponse<Car>(true, entry.Entity);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<Car>(1, mes.Message);
            }
        }
        public async Task<DataResponse<CarpoolRide>> CreateCarpoolRide(int userId, DateTime departureTime, string address, string zipCode, string city, string destinationAddress, string destinationZipCode, string destinationCity, int carId, int numberOfSeats, int pricePerPassenger, List<Event> events, string comment = null)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Create carpool event
                    CarpoolRide carpoolRide = new CarpoolRide()
                    {
                        DriverId = userId,
                        DepartureTime = departureTime,
                        Address = address,
                        ZipCode = zipCode,
                        City = city,
                        CarId = carId,
                        NumberOfSeats = numberOfSeats,
                        PricePerPassenger = pricePerPassenger,
                        Comment = comment,
                        IsActive = true,
                        DestinationAddress = destinationAddress,
                        DestinationCity = destinationCity,
                        DestinationZipCode = destinationZipCode,
                        IsLocked = false
                    };
                    // Add new carpool event
                    var entry = await context.CarpoolRides.AddAsync(carpoolRide);
                    // Save changes
                    int rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != 1)
                        return new DataResponse<CarpoolRide>(200, Resources.AppResources.could_not_create_carpool_event);
                    // Create event relations
                    var event_relations = new List<CarpoolRides_Events_Relation>();
                    foreach (var eve in events)
                    {
                        event_relations.Add(new CarpoolRides_Events_Relation()
                        {
                            CarpoolRideId = entry.Entity.Id,
                            EventId = eve.Id
                        });
                    }
                    // Add new relations
                    await context.CarpoolRides_Events_Relations.AddRangeAsync(event_relations);
                    // Save changes
                    rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != event_relations.Count)
                        return new DataResponse<CarpoolRide>(201, Resources.AppResources.could_not_add_events_to_carpool_event);
                    // Return response
                    return new DataResponse<CarpoolRide>(true, entry.Entity);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<CarpoolRide>(1, mes.Message);
            }
        }
        public async Task<DataResponse<CarpoolRequest>> CreateCarpoolRequest(int userId, DateTime fromTime, DateTime toTime, string zipCode, string city, List<Event> events)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Create carpool request
                    CarpoolRequest carpoolRequest = new CarpoolRequest()
                    {
                        PassengerId = userId,
                        FromTime = fromTime,
                        ToTime = toTime,
                        ZipCode = zipCode,
                        City = city,
                        IsActive = true
                    };
                    // Add new carpool request
                    var entry = await context.CarpoolRequests.AddAsync(carpoolRequest);
                    // Save changes
                    int rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != 1)
                        return new DataResponse<CarpoolRequest>(300, Resources.AppResources.could_not_create_carpool_request);
                    // Create event relations
                    var event_relations = new List<CarpoolRequests_Events_Relation>();
                    foreach (var eve in events)
                    {
                        event_relations.Add(new CarpoolRequests_Events_Relation()
                        {
                            CarpoolRequestId = entry.Entity.Id,
                            EventId = eve.Id
                        });
                    }
                    // Add new relations
                    await context.CarpoolRequests_Events_Relations.AddRangeAsync(event_relations);
                    // Save changes
                    rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != event_relations.Count)
                        return new DataResponse<CarpoolRequest>(301, Resources.AppResources.could_not_add_events_to_carpool_request);
                    // Return response
                    return new DataResponse<CarpoolRequest>(true, entry.Entity);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<CarpoolRequest>(1, mes.Message);
            }
        }
        public async Task<DataResponse> InvitePassenger(int carpoolRequestId, int carpoolRideId)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get carpool request
                    CarpoolRequest carpoolRequest = await context.CarpoolRequests.FirstOrDefaultAsync(x => x.Id == carpoolRequestId);
                    // Set carpool request to inactive
                    carpoolRequest.IsActive = false;
                    // Create carpool confirmation
                    CarpoolConfirmation carpoolConfirmation = new CarpoolConfirmation()
                    {
                        CarpoolRideId = carpoolRideId,
                        HasDriverAccepted = true,
                        HasPassengerAccepted = false,
                        IsActive = true,
                        PassengerId = carpoolRequest.PassengerId
                    };
                    // Add new carpool confirmation
                    await context.CarpoolConfirmations.AddAsync(carpoolConfirmation);
                    // Save changes
                    int rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged < 1)
                        return new DataResponse(401, Resources.AppResources.could_not_invite_passenger);
                    // Return response
                    return new DataResponse(true);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse(1, mes.Message);
            }
        }
        public async Task<DataResponse> UninvitePassenger(int carpoolConfirmationId)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get carpool confirmation
                    CarpoolConfirmation carpoolConfirmation = await context.CarpoolConfirmations.FirstOrDefaultAsync(x => x.Id == carpoolConfirmationId);
                    // Remove carpool confirmation
                    context.CarpoolConfirmations.Remove(carpoolConfirmation);
                    // Save changes
                    int rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != 1)
                        return new DataResponse(400, Resources.AppResources.could_not_delete_invite);
                    // Return response
                    return new DataResponse(true);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse(1, mes.Message);
            }
        }
        public async Task<DataResponse> SignUpToCarpoolRide(int carpoolRideId, int userId)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Create carpool confirmation
                    CarpoolConfirmation carpoolConfirmation = new CarpoolConfirmation()
                    {
                        CarpoolRideId = carpoolRideId,
                        HasDriverAccepted = false,
                        HasPassengerAccepted = true,
                        IsActive = true,
                        PassengerId = userId
                    };
                    // Add new carpool confirmation
                    await context.CarpoolConfirmations.AddAsync(carpoolConfirmation);
                    // Save changes
                    int rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != 1)
                        return new DataResponse(402, Resources.AppResources.could_not_sign_up_to_carpool);
                    // Return response
                    return new DataResponse(true);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse(1, mes.Message);
            }
        }
        public async Task<DataResponse> UnsignFromCarpoolRide(int carpoolConfirmationId)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get carpool confirmation
                    CarpoolConfirmation carpoolConfirmation = await context.CarpoolConfirmations.FirstOrDefaultAsync(x => x.Id == carpoolConfirmationId);
                    // Remove carpool confirmation
                    context.CarpoolConfirmations.Remove(carpoolConfirmation);
                    // Save changes
                    int rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != 1)
                        return new DataResponse(400, Resources.AppResources.could_not_unsign_from_carpool);
                    // Return response
                    return new DataResponse(true);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse(1, mes.Message);
            }
        }
        public async Task<DataResponse> AnswerCarpoolConfirmation(int userId, int carpoolConfirmationId, bool accept)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get carpool confirmation
                    CarpoolConfirmation carpoolConfirmation = await context.CarpoolConfirmations
                        .Include(x => x.CarpoolRide)
                        .FirstOrDefaultAsync(x => x.Id == carpoolConfirmationId);

                    int passengerId = carpoolConfirmation.PassengerId;
                    int driverId = carpoolConfirmation.CarpoolRide.DriverId;
                    // User has answered a confirmation that was not for them
                    if (userId != passengerId && userId != driverId)
                        return new DataResponse(500, Resources.AppResources.could_not_answer_confirmation);
                    // It is passenger or driver
                    bool isDriver = userId == driverId;
                    // Has accepted
                    if (accept)
                    {
                        // Set flags
                        carpoolConfirmation.HasDriverAccepted = isDriver;
                        carpoolConfirmation.HasPassengerAccepted = !isDriver;
                    }
                    // Has denied
                    else
                    {
                        // Remove carpool confirmation
                        context.Remove(carpoolConfirmation);
                    }
                    // Save changes
                    int rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != 1)
                        return new DataResponse(500, Resources.AppResources.could_not_answer_confirmation);
                    // Return response
                    return new DataResponse(true);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse(1, mes.Message);
            }
        }
        public async Task<DataResponse<List<OpeningHour>>> GetOpeningHours()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get opening hours
                    var openingHours = await context.OpeningHours.ToListAsync();
                    // Return response
                    return new DataResponse<List<OpeningHour>>(true, openingHours);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<List<OpeningHour>>(1, mes.Message);
            }
        }
        public async Task<DataResponse<ContactInfo>> GetContactInfo()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get contact info
                    var contactInfo = await context.ContactInfos.FirstOrDefaultAsync();
                    // Return response
                    return new DataResponse<ContactInfo>(contactInfo != null, contactInfo);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<ContactInfo>(1, mes.Message);
            }
        }
        public async Task<DataResponse<string>> GetOpeningHoursInformation()
        {
            try
            {
                CultureInfo ciCurr = CultureInfo.CurrentCulture;

                using (var context = CreateContext())
                {
                    // Get opening hours information from database
                    List<OpeningHour> openingHours = await context.OpeningHours.ToListAsync();

                    string openingsHoursDetails = Resources.AppResources.opening_hours_details;

                    string daysString = "";

                    Action<string> addDayString = (str) =>
                    {
                        daysString += daysString == string.Empty ? $"{str}" : $"\n\n{str}";
                    };

                    foreach (var item in openingHours)
                    {
                        // Get month name
                        string monthName = ciCurr.DateTimeFormat.MonthNames[item.Month - 1];
                        // Open every day in the month
                        if (item.IsOpenEveryday)
                        {
                            addDayString(
                                string.Format("{0}\n{1}, {2}-{3}",
                                              monthName.FirstCharToUpper(),
                                              Resources.AppResources.every_day,
                                              item.OpenFrom.Value.ToString("HH.mm"),
                                              item.OpenTo.Value.ToString("HH.mm"))
                                );
                        }
                        // All days closed
                        else if (item.IsClosed)
                        {
                            addDayString(string.Format("{0}\n{1}",
                                                       monthName.FirstCharToUpper(),
                                                       Resources.AppResources.closed)
                                );
                        }
                        // Some days open
                        else
                        {
                            var weekDayNames = ciCurr.DateTimeFormat.DayNames;

                            List<string> openDayNames = new List<string>();
                            Action<bool, int> addDayName = (flag, dayNumber) =>
                            {
                                if (flag)
                                    openDayNames.Add(weekDayNames[dayNumber]);
                            };

                            // Add the name of days that are open
                            addDayName(item.Monday, 1);
                            addDayName(item.Tuesday, 2);
                            addDayName(item.Wednesday, 3);
                            addDayName(item.Thursday, 4);
                            addDayName(item.Friday, 5);
                            addDayName(item.Saturday, 6);
                            addDayName(item.Sunday, 0);
                            // Open in the holidays
                            if (item.InHolidays)
                                openDayNames.Add(Resources.AppResources.holidays.ToLower());

                            string dayString = string.Empty;
                            for (int i = 0; i < openDayNames.Count; i++)
                            {
                                string dayName = openDayNames[i];

                                if (dayString == string.Empty)
                                    dayString += dayName;
                                else
                                {
                                    if (i == openDayNames.Count - 1)
                                        dayString += $" & {dayName}";
                                    else
                                        dayString += $", {dayName}";
                                }
                            }

                            addDayString(
                                string.Format("{0}\n{1}, {2}-{3}",
                                              monthName.FirstCharToUpper(),
                                              dayString.FirstCharToUpper(),
                                              item.OpenFrom.Value.ToString("HH.mm"),
                                              item.OpenTo.Value.ToString("HH.mm"))
                                );
                        }
                    }

                    // Return response
                    return new DataResponse<string>(true, string.Format(Resources.AppResources.opening_hours_details, daysString));
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<string>(1, mes.Message);
            }
        }
        public async Task<DataResponse<string>> GetTodaysOpeningHours()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get opening hours
                    var openingHour = await context.OpeningHours.FirstOrDefaultAsync(x => x.Month == DateTime.Now.Month);

                    var dateTime = DateTime.Now;
                    Func<bool, DayOfWeek, bool> checkOpenDays = (flag, dayOfWeek) =>
                    {
                        return flag && dateTime.DayOfWeek == dayOfWeek;
                    };

                    string strOpeningHour = string.Empty;

                    // Check if there is open today
                    if (openingHour.IsOpenEveryday ||
                             checkOpenDays(openingHour.Monday, DayOfWeek.Monday) ||
                            checkOpenDays(openingHour.Tuesday, DayOfWeek.Tuesday) ||
                            checkOpenDays(openingHour.Wednesday, DayOfWeek.Wednesday) ||
                            checkOpenDays(openingHour.Thursday, DayOfWeek.Thursday) ||
                            checkOpenDays(openingHour.Friday, DayOfWeek.Friday) ||
                            checkOpenDays(openingHour.Saturday, DayOfWeek.Saturday) ||
                            checkOpenDays(openingHour.Sunday, DayOfWeek.Sunday))
                    {
                        strOpeningHour = string.Format("{0}-{1}",
                            openingHour.OpenFrom.Value.ToString("HH.mm"),
                            openingHour.OpenTo.Value.ToString("HH.mm")
                            );
                    }
                    // There is closed
                    else
                    {
                        strOpeningHour = Resources.AppResources.closed;
                    }

                    // Return response
                    return new DataResponse<string>(true, strOpeningHour);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<string>(1, mes.Message);
            }
        }

        public async Task<DataResponse<CarpoolRide>> GetNextCarpoolRide()
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get next carpool ride
                    var carpoolRide = await context.CarpoolRides
                        .Where(x => x.DepartureTime.CompareTo(DateTime.Now) >= 0)
                        .OrderBy(x => x.DepartureTime)
                        .FirstOrDefaultAsync();
                    // Return response
                    return new DataResponse<CarpoolRide>(true, carpoolRide);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<CarpoolRide>(1, mes.Message);
            }
        }
        public async Task<DataResponse<User>> CheckLogin(string phoneNo)
        {
            try
            {
                using (var context = CreateContext())
                {
                    // Get user from user id
                    User user = await context.Users.FirstOrDefaultAsync(x => x.PhoneNo == phoneNo);
                    // Phone no. is used on an existing account
                    if (user == null)
                        return new DataResponse<User>(110, Resources.AppResources.this_phone_no_is_not_used_on_any_existing_accounts);
                    // Return response
                    return new DataResponse<User>(true, user);
                }
            }
            catch (Exception mes)
            {
                // Return exception
                return new DataResponse<User>(1, mes.Message);
            }
        }
    }
}
