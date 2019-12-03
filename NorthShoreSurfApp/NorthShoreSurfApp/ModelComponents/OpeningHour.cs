using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public class OpeningHour
    {
        [Key]
        public int Id { get; set; }
        public int Month { get; set; }
        [NotMapped]
        public bool Monday { get => MondayColumn != 0; set => MondayColumn = value ? 1 : 0; }
        [Column(nameof(Monday))]
        public int MondayColumn { get; set; }
        [NotMapped]
        public bool Tuesday { get => TuesdayColumn != 0; set => TuesdayColumn = value ? 1 : 0; }
        [Column(nameof(Tuesday))]
        public int TuesdayColumn { get; set; }
        [NotMapped]
        public bool Wednesday { get => WednesdayColumn != 0; set => WednesdayColumn = value ? 1 : 0; }
        [Column(nameof(Wednesday))]
        public int WednesdayColumn { get; set; }
        [NotMapped]
        public bool Thursday { get => ThursdayColumn != 0; set => ThursdayColumn = value ? 1 : 0; }
        [Column(nameof(Thursday))]
        public int ThursdayColumn { get; set; }
        [NotMapped]
        public bool Friday { get => FridayColumn != 0; set => FridayColumn = value ? 1 : 0; }
        [Column(nameof(Friday))]
        public int FridayColumn { get; set; }
        [NotMapped]
        public bool Saturday { get => SaturdayColumn != 0; set => SaturdayColumn = value ? 1 : 0; }
        [Column(nameof(Saturday))]
        public int SaturdayColumn { get; set; }
        [NotMapped]
        public bool Sunday { get => SundayColumn != 0; set => SundayColumn = value ? 1 : 0; }
        [Column(nameof(Sunday))]
        public int SundayColumn { get; set; }
        [NotMapped]
        public bool InHolidays { get => InHolidaysColumn != 0; set => InHolidaysColumn = value ? 1 : 0; }
        [Column(nameof(InHolidays))]
        public int InHolidaysColumn { get; set; }

        public DateTime? OpenFrom { get; set; }
        public DateTime? OpenTo { get; set; }

        [NotMapped]
        public bool IsClosed { get => !Monday && !Tuesday && !Wednesday && !Thursday && !Friday && !Saturday && !Sunday && !InHolidays; }
        [NotMapped]
        public bool IsOpenEveryday { get => Monday && Tuesday && Wednesday && Thursday && Friday && Saturday && Sunday; }
    }
}
