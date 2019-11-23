using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public class CarpoolRides_Events_Relation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CarpoolRideId { get; set; }
        [ForeignKey(nameof(CarpoolRideId))]
        public CarpoolRide CarpoolRide { get; set; }
        [Required]
        public int EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; }
    }
}
