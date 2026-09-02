using System.ComponentModel.DataAnnotations;

namespace ReadersBookBank.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string BookName { get; set; }

        [Required]
        [StringLength(20)]
        public required string Genre { get; set; }

        public bool AvailabilityStatus { get; set; }
    }
}