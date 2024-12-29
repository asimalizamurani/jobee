using System;
using System.Collections.Generic;

namespace jobee.Models
{
    public class ApplicantPanelViewModel
    {
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public List<Vacancy> AppliedVacancies { get; set; } = new List<Vacancy>();
        public DateTime AppliedAt { get; set; }
    }
}
