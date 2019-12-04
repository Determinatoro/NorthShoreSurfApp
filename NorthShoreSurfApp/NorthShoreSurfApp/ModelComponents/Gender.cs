using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public partial class Gender
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        [NotMapped]
        public string LocalizedName { get => (Id > 0 && Id <= Genders.Length) ? Genders[Id - 1].Name : null; }

        [NotMapped]
        public static Gender[] Genders
        {
            get => new Gender[]{
                new Gender() { Id = 1, Name = Resources.AppResources.male },
                new Gender() { Id = 2, Name = Resources.AppResources.female },
                new Gender() { Id = 3, Name = Resources.AppResources.other }
            };
        }
    }
}
