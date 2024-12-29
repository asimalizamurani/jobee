using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for Authorize attribute
using jobee.Data; // Replace with your actual namespace for DbContext
using jobee.Models; // Replace with your actual namespace for models
using System.Security.Claims;
using Microsoft.EntityFrameworkCore; // Required for accessing user claims;

namespace jobee.Controllers
{
    [Authorize] // Ensure only authenticated users can access these actions
    public class VacancyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacancyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vacancy/Index
        public IActionResult Index()
        {
            var vacancies = _context.Vacancies.ToList();
            return View(vacancies);
        }

        // GET: Vacancy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vacancy/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vacancy vacancy)
        {
            // Check user role
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != "HR" && role != "Admin")
            {
                TempData["ErrorMessage"] = "Access Denied: You do not have permission to create a vacancy.";
                return RedirectToAction("AccessDenied", "User"); // Redirect unauthorized users
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    // Get the logged-in user's ID from claims
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (string.IsNullOrEmpty(userId))
                    {
                        TempData["ErrorMessage"] = "Unable to identify the logged-in user.";
                        return RedirectToAction("Login", "Account"); // Redirect to login if user is not authenticated
                    }

                    // Set dynamically fetched user ID and other properties
                    vacancy.CreatedBy = int.Parse(userId); // Assuming user IDs are integers
                    vacancy.CreatedAt = DateTime.UtcNow;
                    vacancy.Status = "Open"; // Default status

                    // Save to the database
                    _context.Vacancies.Add(vacancy);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Vacancy created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the error (if you have a logging mechanism)
                    TempData["ErrorMessage"] = $"Error: {ex.Message}";
                    // Add logging here
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                // Log model state errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }

            // If the model state is invalid, return to the Create view
            return View(vacancy);
        }

        // GET: Vacancy/Edit/{id}
        public IActionResult Edit(int id)
        {
            // Check user role
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != "HR" && role != "Admin")
            {
                TempData["ErrorMessage"] = "Access Denied: You do not have permission to edit a vacancy.";
                return RedirectToAction("AccessDenied", "User"); // Redirect unauthorized users
            }

            var vacancy = _context.Vacancies.Find(id);
            if (vacancy == null)
            {
                TempData["ErrorMessage"] = "Vacancy not found.";
                return RedirectToAction("Index");
            }

            return View(vacancy);
        }

        // POST: Vacancy/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("VacancyId,Title,Description,Department,PositionsAvailable,Status,CreatedBy,CreatedAt")] Vacancy vacancy)
        {
            // Check user role
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != "HR" && role != "Admin")
            {
                TempData["ErrorMessage"] = "Access Denied: You do not have permission to edit a vacancy.";
                return RedirectToAction("AccessDenied", "User"); // Redirect unauthorized users
            }

            if (id != vacancy.VacancyId)
            {
                TempData["ErrorMessage"] = "Invalid vacancy ID.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Attach the vacancy to the context and mark it as modified
                    _context.Attach(vacancy).State = EntityState.Modified;

                    // Save the changes to the database
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Vacancy updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the error (if you have a logging mechanism)
                    TempData["ErrorMessage"] = $"Error: {ex.Message}";
                }
            }

            // If the model state is invalid, return to the Edit view
            return View(vacancy);
        }
    }
}