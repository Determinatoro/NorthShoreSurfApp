using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public partial class CarpoolRequests_Events_Relations
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int CarpoolRequestId { get; set; }
        [ForeignKey(nameof(CarpoolRequestId))]
        public CarpoolRequest CarpoolRequest { get; set; }
        [Required]
        public int EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; }
    }
}
