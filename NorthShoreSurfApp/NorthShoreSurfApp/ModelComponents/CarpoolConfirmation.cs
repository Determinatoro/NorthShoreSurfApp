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
        public int? CarpoolRequestId { get; set; }
        [ForeignKey(nameof(CarpoolRequestId))]
        public CarpoolRequest CarpoolRequest { get; set; }

        [NotMapped]
        public int? CurrentUserId { get; set; }
        [NotMapped]
        public bool IsOwnRide
        {
            get
            {
                if (CurrentUserId == null)
                    return false;
                
                return CurrentUserId == CarpoolRide.DriverId;
            }
        }
        [NotMapped]
        public bool IsOtherRide
        {
            get
            {
                if (CurrentUserId == null)
                    return false;

                return CurrentUserId == PassengerId;
            }
        }
        [NotMapped]
        public string InformationString
        {
            get
            {
                if (IsInvite)
                    return string.Format(Resources.AppResources.you_have_been_invited_by, CarpoolRide.Driver.FullName);
                else if (IsRequest)
                    return string.Format(Resources.AppResources.you_have_requested_to_join, CarpoolRide.Driver.FullName);

                return string.Empty;
            }
        }
        [NotMapped]
        public bool IsConfirmed { get => HasPassengerAccepted && HasDriverAccepted; }
        [NotMapped]
        public bool IsInvite { get => HasDriverAccepted && !HasPassengerAccepted; }
        [NotMapped]
        public bool IsRequest { get => !HasDriverAccepted && HasPassengerAccepted; }
        [NotMapped]
        public bool ShowAcceptButton
        {
            get
            {
                if (CurrentUserId == null)
                    return false;

                if (CurrentUserId == PassengerId)
                {
                    return IsInvite;
                }
                else
                {
                    return IsRequest;
                }

            }
        }
        [NotMapped]
        public bool ShowDenyButton
        {
            get
            {
                if (CurrentUserId == null)
                    return false;

                if (CurrentUserId.Value == PassengerId)
                {
                    return IsInvite || IsRequest;
                }
                else
                {
                    return IsInvite || IsRequest;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CarpoolConfirmation))
                return false;

            return this.Id == ((CarpoolConfirmation)obj).Id;
        }
    }
}
