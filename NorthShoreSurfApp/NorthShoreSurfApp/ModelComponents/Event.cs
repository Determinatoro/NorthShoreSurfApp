using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace NorthShoreSurfApp.ModelComponents
{
    public class Event : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isSelected;

        /// <summary>
        /// Property changed default method
        /// </summary>
        /// <param name="propertyName">Name for the property that will be notified about</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Key]
        [Required]
        public int Id { get; set; }
        [NotMapped]
        public bool IsActive { get => IsActiveColumn != 0; set => IsActiveColumn = value ? 1 : 0; }
        [Column(nameof(IsActive))]
        public int IsActiveColumn { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [NotMapped]
        public bool IsSelected { get => isSelected; set { isSelected = value; OnPropertyChanged(); } }

        public override bool Equals(object obj)
        {
            if (!(obj is Event))
                return false;

            return Id == ((Event)obj).Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
