using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace jobee.Models
{
    public class User
    {
        [Key]
        public int Userid { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string Role { get; set; } = "Applicant"; // Default role is set to Applicant
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Set default value
    }
}
