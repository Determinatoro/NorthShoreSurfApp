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
        public Nullable<int> CarpoolRequestId { get; set; }
        [ForeignKey(nameof(CarpoolRequestId))]
        public CarpoolRequest CarpoolRequest { get; set; }

        [NotMapped]
        public int CurrentUserId { get; set; }
        [NotMapped]
        public bool IsConfirmed { get => HasPassengerAccepted && HasDriverAccepted; }
        [NotMapped]
        public bool IsInvite { get => HasDriverAccepted && !HasPassengerAccepted; }
        [NotMapped]
        public bool IsRequest { get => !HasDriverAccepted && HasPassengerAccepted; }
        [NotMapped]
        public bool ShowAcceptButton { get => CurrentUserId != PassengerId && IsRequest; }
        [NotMapped]
        public bool ShowDenyButton { get => !(CurrentUserId != PassengerId && IsConfirmed); }
    }
}
