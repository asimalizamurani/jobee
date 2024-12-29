using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace jobee.Models
{
    public class Vacancy
    {
        [Key]
        public int VacancyId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } // Limit title length for better UX and DB storage

        [Required]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } // Limit description length

        [Required]
        [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters.")]
        public string Department { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "At least one position must be available.")]
        public int PositionsAvailable { get; set; } // Ensure at least one position

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Open"; // Default status is "Open"

        [Required]
        [ForeignKey("CreatedByUser")]
        public int CreatedBy { get; set; } // Associate with User entity

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to UTC for consistency

        // Navigation Property
        public User CreatedByUser { get; set; } // Navigation to user who created the vacancy

    }
}
