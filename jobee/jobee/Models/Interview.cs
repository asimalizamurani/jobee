using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jobee.Models
{
    public class Interview
    {
        [Key]
        public int InterviewId { get; set; }

        [Required]
        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; } // FK for Vacancy

        [Required]
        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; } // FK for Applicant

        [Required]
        [ForeignKey("Interviewer")]
        public int InterviewerId { get; set; } // FK for User (Interviewer)

        [Required]
        public DateTime Date { get; set; } // Interview date

        [Required]
        public TimeSpan Time { get; set; } // Interview time

        [StringLength(500)]
        public string Result { get; set; } // Result of the interview (optional)

        // Navigation Properties
        public Vacancy Vacancy { get; set; }
        public Applicant Applicant { get; set; }
        public User Interviewer { get; set; }

        // Computed Property for Combined Date and Time
        [NotMapped]
        public DateTime InterviewDateTime => Date.Add(Time);
    }
}
