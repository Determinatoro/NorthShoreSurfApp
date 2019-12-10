using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public partial class CarpoolConfirmation
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [NotMapped]
        public bool IsActive { get => IsActiveColumn != 0; set => IsActiveColumn = value ? 1 : 0; }
        [Column(nameof(IsActive))]
        public int IsActiveColumn { get; set; }
        [Required]
        public bool HasPassengerAccepted { get; set; }
        [Required]
        public bool HasDriverAccepted { get; set; }
        [Required]
        public int PassengerId { get; set; }
        [ForeignKey(nameof(PassengerId))]
        public User Passenger { get; set; }
        [Required]
        public int CarpoolRideId { get; set; }
        [ForeignKey(nameof(CarpoolRideId))]
        public CarpoolRide CarpoolRide { get; set; }

        [NotMapped]
        public bool IsConfirmed { get => HasPassengerAccepted && HasDriverAccepted; }

        [NotMapped]
        public string PricePerPassengerString { get => String.Format("{0} DKK", CarpoolRide.PricePerPassenger); }

        [NotMapped]
        public string Address { get => String.Format("{0}", CarpoolRide.Address); }

        public string Destination { get => String.Format("{0}", CarpoolRide.DestinationAddress); }

        public string PassengerName { get => String.Format("{0} has requested to join your ride", Passenger.FullName); }

        public string Date { get => String.Format("{0} ", CarpoolRide.DepartureTimeDayString); }

        public string Time { get => String.Format("{0} ", CarpoolRide.DepartureTimeHourString); }

        public string Gender { get => String.Format("{0} ", Passenger.Gender.Name); }

        public string Age { get => String.Format("{0} ", Passenger.Age); }

        public string PhoneNo { get => String.Format("{0}", Passenger.PhoneNo); }

    }
}
