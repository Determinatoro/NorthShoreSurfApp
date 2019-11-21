using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public class CarpoolEvents_Events_Relation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CarpoolEventId { get; set; }
        [ForeignKey(nameof(CarpoolEventId))]
        public CarpoolEvent CarpoolEvent { get; set; }
        [Required]
        public int EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; }
    }
}
