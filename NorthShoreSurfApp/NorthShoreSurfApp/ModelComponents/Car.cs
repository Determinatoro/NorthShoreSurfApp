using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public partial class Car
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [NotMapped]
        public bool IsActive { get => IsActiveColumn != 0; set => IsActiveColumn = value ? 1 : 0; }
        [Column(nameof(IsActive))]
        public int IsActiveColumn { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [Required]
        [StringLength(10)]
        [MinLength(4)]
        public string LicensePlate { get; set; }
        [Required]
        [StringLength(255)]
        public string Color { get; set; }

        #region UI specific

        [NotMapped]
        public bool IsTitle { get => Title != null; }
        [NotMapped]
        public string Title { get; set; }
        [NotMapped]
        public string CarInfo { get => $"{LicensePlate}, {Color}"; }

        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is Car))
                return false;
            return this.Id == ((Car)obj).Id;
        }
    }
}
