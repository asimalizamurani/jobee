using System;
using System.ComponentModel.DataAnnotations;

namespace jobee.Models
{
    public class Applicant
    {
        public int ApplicantId { get; set; } // Renamed for consistency with conventions

        [Required] // Ensures that Name is required
        [MaxLength(100)] // Sets a maximum length for Name
        public string Name { get; set; }

        [Required] // Ensures that Email is required
        [EmailAddress] // Ensures proper email format
        public string Email { get; set; }

        [Required] // Ensures that Role is required
        [MaxLength(50)] // Sets a maximum length for Role
        public string Role { get; set; }

        public int AttachedVacancyId { get; set; }

        [MaxLength(255)] // Ensures that the resume path is not too long
        public string ResumePath { get; set; }

        public DateTime AppliedAt { get; set; }

        // Navigation Property
        public Vacancy AttachedVacancy { get; set; } // Ensures we can navigate to the related Vacancy
    }
}
