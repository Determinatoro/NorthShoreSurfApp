using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public partial class CarpoolRequest
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public int PassengerId { get; set; }
        [ForeignKey(nameof(PassengerId))]
        public User Passenger { get; set; }
        [Required]
        public DateTime FromTime { get; set; }
        [Required]
        public DateTime ToTime { get; set; }
        [Required]
        [StringLength(4)]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(255)]
        public string City { get; set; }

        [NotMapped]
        public string TimeInterval { get => String.Format("{0} - {1}", FromTime.ToString("HH:mm"), ToTime.ToString("HH:mm")); }
        [NotMapped]
        public string DepartureTimeDayString { get => FromTime.ToCarpoolingFormat(); }
    }
}
