using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public partial class CarpoolRide
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [NotMapped]
        public bool IsActive { get => IsActiveColumn != 0; set => IsActiveColumn = value ? 1 : 0; }
        [Column(nameof(IsActive))]
        public int IsActiveColumn { get; set; }
        [Required]
        public int DriverId { get; set; }
        [ForeignKey(nameof(DriverId))]
        public User Driver { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        [StringLength(255)]
        public string Address { get; set; }
        [Required]
        [StringLength(4)]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(255)]
        public string City { get; set; }
        [Required]
        [StringLength(255)]
        public string DestinationAddress { get; set; }
        [Required]
        [StringLength(4)]
        public string DestinationZipCode { get; set; }
        [Required]
        [StringLength(255)]
        public string DestinationCity { get; set; }
        [Required]
        public int CarId { get; set; }
        [ForeignKey(nameof(CarId))]
        public Car Car { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }
        [Required]
        public int PricePerPassenger { get; set; }
        [StringLength(255)]
        public string Comment { get; set; }

        public List<CarpoolRides_Events_Relation> CarpoolRides_Events_Relations { get; set; }
        public List<CarpoolConfirmation> CarpoolConfirmations { get; set; }

        [NotMapped]
        public string DepartureTimeString { get => DepartureTime.ToCarpoolingFormat(); }
    }
}
