using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using jobee.Data;
using jobee.Models;
using System.Linq;
using System.Security.Claims;

namespace jobee.Controllers
{
    [Authorize(Roles = "Applicant")]
    public class ApplicantPanelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicantPanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get the logged-in user's ID
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            // Fetch applicant details
            var applicant = _context.Applicants.FirstOrDefault(a => a.ApplicantId == userId);

            if (applicant == null)
            {
                return NotFound("Applicant details not found.");
            }

            // Fetch applied vacancies
            var appliedVacancies = _context.Vacancies
                .Where(v => v.CreatedBy == userId)
                .ToList();

            // Fetch interview schedules
            var interviews = _context.Interviews
                .Where(i => i.ApplicantId == userId)
                .ToList();

            // Pass data to the view
            ViewBag.Applicant = applicant;
            ViewBag.AppliedVacancies = appliedVacancies;
            ViewBag.Interviews = interviews;

            return View();
        }

        public IActionResult ViewApplication(int id)
        {
            // Fetch application details
            var application = _context.Vacancies.FirstOrDefault(v => v.VacancyId == id);

            if (application == null)
            {
                return NotFound("Application not found.");
            }

            return View(application);
        }

        public IActionResult ViewInterview(int id)
        {
            // Fetch interview details
            var interview = _context.Interviews.FirstOrDefault(i => i.InterviewId == id);

            if (interview == null)
            {
                return NotFound("Interview not found.");
            }

            return View(interview);
        }
    }
}
