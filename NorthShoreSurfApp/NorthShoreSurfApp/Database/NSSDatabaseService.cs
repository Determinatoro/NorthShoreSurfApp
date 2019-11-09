﻿using Microsoft.EntityFrameworkCore;
using NorthShoreSurfApp.ModelComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<CarpoolConfirmation>> GetCarpoolConfirmations()
        {
            using (var context = CreateContext())
            {
                return await context.CarpoolConfirmations
                                    .Include(x => x.Passenger)
                                    .Include(x => x.CarpoolEvent)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<List<CarpoolEvent>> GetCarpoolEvents()
        {
            using (var context = CreateContext())
            {
                return await context.CarpoolEvents
                                    .Include(x => x.State)
                                    .Include(x => x.Driver)
                                    .Include(x => x.Car)
                                    .Include(x => x.CarpoolConfirmations)
                                    .Include(x => x.CarpoolEvents_Events_Relations).ThenInclude(x => x.Event)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<List<CarpoolRequest>> GetCarpoolRequests()
        {
            using (var context = CreateContext())
            {
                return await context.CarpoolRequests
                                    .Include(x => x.State)
                                    .Include(x => x.Passenger)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<List<Car>> GetCars()
        {
            using (var context = CreateContext())
            {
                return await context.Cars
                                    .Include(x => x.State)
                                    .Include(x => x.User)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<List<Event>> GetEvents()
        {
            using (var context = CreateContext())
            {
                return await context.Events
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<List<Gender>> GetGenders()
        {
            using (var context = CreateContext())
            {
                return await context.Genders
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<List<State>> GetStates()
        {
            using (var context = CreateContext())
            {
                return await context.States
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<List<User>> GetUsers()
        {
            using (var context = CreateContext())
            {
                return await context.Users
                                    .Include(x => x.State)
                                    .Include(x => x.Gender)
                                    .Include(x => x.Cars)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<DataResponse> CheckPhoneNo(string phoneNo)
        {
            using (var context = CreateContext())
            {
                bool success = await context.Users.AnyAsync(x => x.PhoneNo == phoneNo);
                return new DataResponse(success);
            }
        }

        public async Task<DataResponse<User>> GetUser(string phoneNo)
        {
            using (var context = CreateContext())
            {
                // Get user from phone no.
                User user = await context.Users.FirstOrDefaultAsync(x => x.PhoneNo == phoneNo);
                // Return response
                return new DataResponse<User>(user != null, user);
            }
        }

        public async Task<DataResponse> UpdateUser(int userId, string firstName, string lastName, string phoneNo, int age, int genderId)
        {
            using (var context = CreateContext())
            {
                try
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
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse<User>> SignUpUser(string firstName, string lastName, string phoneNo, int age, int genderId)
        {
            using (var context = CreateContext())
            {
                try
                {
                    // Check phone no.
                    DataResponse response = await CheckPhoneNo(phoneNo);
                    // User already exist
                    if (response.Success)
                        return new DataResponse<User>(101, Resources.AppResources.user_already_exist);
                    // Create user
                    User user = new User()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNo = phoneNo,
                        Age = age,
                        GenderId = genderId,
                        StateId = 1
                    };
                    // Add new user
                    var entry = await context.Users.AddAsync(user);
                    // Save changes
                    await context.SaveChangesAsync();
                    // Return response
                    return new DataResponse<User>(true, entry.Entity);
                }
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse<User>(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse> DeleteUser(string phoneNo)
        {
            using (var context = CreateContext())
            {
                try
                {
                    // Get user from phone no.
                    User user = await context.Users.FirstOrDefaultAsync(x => x.PhoneNo == phoneNo);
                    // User not found
                    if (user == null)
                        return new DataResponse<User>(100, Resources.AppResources.user_not_found);
                    // Set user to inactive
                    user.StateId = 2;
                    // Save changes
                    await context.SaveChangesAsync();
                    // Return response
                    return new DataResponse(true);
                }
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse<User>(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse<Car>> CreateCar(int userId, string licensePlate, string color)
        {
            using (var context = CreateContext())
            {
                try
                {
                    // Create car
                    Car car = new Car()
                    {
                        UserId = userId,
                        StateId = 1,
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
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse<Car>(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse<CarpoolEvent>> CreateCarpoolEvent(int userId, DateTime departureTime, string address, string zipCode, string city, int carId, int numberOfSeats, int pricePerPassenger, List<Event> events, string comment = null)
        {
            using (var context = CreateContext())
            {
                try
                {
                    // Create carpool event
                    CarpoolEvent carpoolEvent = new CarpoolEvent()
                    {
                        DriverId = userId,
                        DepartureTime = departureTime,
                        Address = address,
                        ZipCode = zipCode,
                        CarId = carId,
                        NumberOfSeats = numberOfSeats,
                        PricePerPassenger = pricePerPassenger,
                        Comment = comment,
                        StateId = 1
                    };
                    // Add new carpool event
                    var entry = await context.CarpoolEvents.AddAsync(carpoolEvent);
                    // Save changes
                    int rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != 1)
                        return new DataResponse<CarpoolEvent>(200, Resources.AppResources.could_not_create_carpool_event);
                    // Create event relations
                    var event_relations = new List<CarpoolEvents_Events_Relation>();
                    foreach (var eve in events)
                    {
                        event_relations.Add(new CarpoolEvents_Events_Relation()
                        {
                            CarpoolEventId = entry.Entity.Id,
                            EventId = eve.Id
                        });
                    }
                    // Add new relations
                    await context.CarpoolEvents_Events_Relations.AddRangeAsync(event_relations);
                    // Save changes
                    rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != event_relations.Count)
                        return new DataResponse<CarpoolEvent>(201, Resources.AppResources.could_not_add_events_to_carpool_event);
                    // Return response
                    return new DataResponse<CarpoolEvent>(true, entry.Entity);
                }
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse<CarpoolEvent>(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse<CarpoolRequest>> CreateCarpoolRequest(int userId, DateTime fromTime, DateTime toTime, string zipCode, string city, List<Event> events)
        {
            using (var context = CreateContext())
            {
                try
                {
                    // Create carpool request
                    CarpoolRequest carpoolRequest = new CarpoolRequest()
                    {
                        PassengerId = userId,
                        FromTime = fromTime,
                        ToTime = toTime,
                        ZipCode = zipCode,
                        City = city,
                        StateId = 1
                    };
                    // Add new carpool request
                    var entry = await context.CarpoolRequests.AddAsync(carpoolRequest);
                    // Save changes
                    int rowsChanged = await context.SaveChangesAsync();
                    // Error when saving
                    if (rowsChanged != 1)
                        return new DataResponse<CarpoolRequest>(300, Resources.AppResources.could_not_create_carpool_request);
                    // Create event relations
                    var event_relations = new List<CarpoolRequests_Events_Relations>();
                    foreach (var eve in events)
                    {
                        event_relations.Add(new CarpoolRequests_Events_Relations()
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
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse<CarpoolRequest>(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse> InvitePassenger(int carpoolRequestId, int carpoolEventId)
        {
            using (var context = CreateContext())
            {
                try
                {
                    // Get carpool request
                    CarpoolRequest carpoolRequest = await context.CarpoolRequests.FirstOrDefaultAsync(x => x.Id == carpoolRequestId);
                    // Set carpool request to inactive
                    carpoolRequest.StateId = 2;
                    // Create carpool confirmation
                    CarpoolConfirmation carpoolConfirmation = new CarpoolConfirmation()
                    {
                        CarpoolEventId = carpoolEventId,
                        HasDriverAccepted = true,
                        HasPassengerAccepted = false,
                        StateId = 1,
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
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse> UninvitePassenger(int carpoolConfirmationId)
        {
            using (var context = CreateContext())
            {
                try
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
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse> SignUpToCarpoolEvent(int carpoolEventId, int userId)
        {
            using (var context = CreateContext())
            {
                try
                {
                    // Create carpool confirmation
                    CarpoolConfirmation carpoolConfirmation = new CarpoolConfirmation()
                    {
                        CarpoolEventId = carpoolEventId,
                        HasDriverAccepted = false,
                        HasPassengerAccepted = true,
                        StateId = 1,
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
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse> UnsignFromCarpoolEvent(int carpoolConfirmationId)
        {
            using (var context = CreateContext())
            {
                try
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
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse(1, mes.Message);
                }
            }
        }

        public async Task<DataResponse> AnswerCarpoolConfirmation(int userId, int carpoolConfirmationId, bool accept)
        {
            using (var context = CreateContext())
            {
                try
                {
                    // Get carpool confirmation
                    CarpoolConfirmation carpoolConfirmation = await context.CarpoolConfirmations
                        .Include(x => x.CarpoolEvent)
                        .FirstOrDefaultAsync(x => x.Id == carpoolConfirmationId);

                    int passengerId = carpoolConfirmation.PassengerId;
                    int driverId = carpoolConfirmation.CarpoolEvent.DriverId;
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
                catch (Exception mes)
                {
                    // Return exception
                    return new DataResponse(1, mes.Message);
                }
            }
        }
    }
}
