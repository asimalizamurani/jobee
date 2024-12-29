using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using jobee.Data;
using jobee.Models;
using System.Linq;
using System.Security.Claims;
using System;
using Microsoft.EntityFrameworkCore;

namespace jobee.Controllers
{
    [Authorize] // Ensures that only authenticated users can access these actions
    public class ApplicantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Applicant/Index
        [Authorize(Roles = "HR, Admin")] // Only HR or Admin can view applications
        public IActionResult Index()
        {
            var applicants = _context.Applicants
                .Include(a => a.AttachedVacancy) // Include the related Vacancy data
                .ToList();
            return View(applicants);
        }

        // GET: Applicant/Details/{id}
        public IActionResult Details(int id)
        {
            var applicant = _context.Applicants
                .Include(a => a.AttachedVacancy) // Include the related Vacancy data
                .FirstOrDefault(a => a.ApplicantId == id);

            if (applicant == null)
            {
                TempData["ErrorMessage"] = "Applicant not found.";
                return RedirectToAction("Index");
            }

            return View(applicant);
        }

        // GET: Applicant/Apply/{vacancyId}
        public IActionResult Apply(int vacancyId)
        {
            // Fetch the vacancy details to display on the application form
            var vacancy = _context.Vacancies.Find(vacancyId);
            if (vacancy == null)
            {
                TempData["ErrorMessage"] = "Vacancy not found.";
                return RedirectToAction("Index", "Vacancy");
            }

            // Pass the vacancies to the view
            ViewBag.Vacancies = _context.Vacancies.ToList();

            return View(new Applicant
            {
                AttachedVacancyId = vacancyId // Pre-fill the vacancy ID
            });
        }

        // POST: Applicant/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(Applicant model, IFormFile resumePath)
        {
            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Unable to identify the logged-in user.";
                return RedirectToAction("Login", "User");
            }

            // Check if the vacancyId is valid
            var vacancy = await _context.Vacancies.FindAsync(model.AttachedVacancyId);
            if (vacancy == null)
            {
                TempData["ErrorMessage"] = "Invalid vacancy ID.";
                return RedirectToAction("Index", "Vacancy");
            }

            // Check if the applicant already applied for the job
            var existingApplication = _context.Applicants
                .FirstOrDefault(a => a.ApplicantId == int.Parse(userId) && a.AttachedVacancyId == model.AttachedVacancyId);

            if (existingApplication != null)
            {
                TempData["ErrorMessage"] = "You have already applied for this job.";
                return RedirectToAction("Index", "Vacancy");
            }

            // Get the logged-in user's email
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            // Validate resumePath
            if (resumePath == null || resumePath.Length == 0)
            {
                TempData["ErrorMessage"] = "Resume path cannot be empty.";
                return RedirectToAction("Index", "Vacancy");
            }

            // Ensure the directory exists
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the file to a specific path
            var filePath = Path.Combine(directoryPath, resumePath.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await resumePath.CopyToAsync(stream);
            }

            // Create a new application record
            var applicant = new Applicant
            {
                Name = model.Name,
                Role = model.Role,
                AttachedVacancyId = model.AttachedVacancyId,
                ResumePath = filePath,
                AppliedAt = DateTime.UtcNow,
                Email = userEmail // Ensure Email is set
            };

            // Save the application to the database
            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "You have successfully applied for the job!";
            return RedirectToAction("Index", "Vacancy");
        }
    }
}
