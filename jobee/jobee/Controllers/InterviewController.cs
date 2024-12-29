using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using jobee.Models;
using jobee.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace jobee.Controllers
{
    public class InterviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InterviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Interview
        [Authorize(Roles = "Admin,HR, Applicant, Interviewer")]
        public async Task<IActionResult> Index()
        {
            var interviews = _context.Interviews
                .Include(i => i.Vacancy)
                .Include(i => i.Applicant)
                .Include(i => i.Interviewer);
            return View(await interviews.ToListAsync());
        }

        // GET: Interview/Create
        [Authorize(Roles = "Admin,HR")]
        public IActionResult Create()
        {
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "VacancyId", "Title");
            ViewData["ApplicantId"] = new SelectList(_context.Applicants, "ApplicantId", "Name");
            ViewData["InterviewerId"] = new SelectList(_context.Users.Where(u => u.Role == "Interviewer"), "Userid", "Username");
            return View();
        }

        [Authorize(Roles = "Admin,HR")]
        // POST: Interview/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VacancyId,ApplicantId,InterviewerId,Date,Time,Result")] Interview interview)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(interview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "VacancyId", "Title", interview.VacancyId);
            ViewData["ApplicantId"] = new SelectList(_context.Applicants, "ApplicantId", "Name", interview.ApplicantId);
            ViewData["InterviewerId"] = new SelectList(_context.Users.Where(u => u.Role == "Interviewer"), "Userid", "Username", interview.InterviewerId);
            return View(interview);
        }

        // GET: Interview/Edit/5
        [Authorize(Roles = "Admin,HR, Interviewer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var interview = await _context.Interviews.FindAsync(id);
            if (interview == null) return NotFound();

            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "VacancyId", "Title", interview.VacancyId);
            ViewData["ApplicantId"] = new SelectList(_context.Applicants, "ApplicantId", "Name", interview.ApplicantId);
            ViewData["InterviewerId"] = new SelectList(_context.Users.Where(u => u.Role == "Interviewer"), "Userid", "Username", interview.InterviewerId);
            return View(interview);
        }

        // POST: Interview/Edit/5
        [Authorize(Roles = "Admin,HR, Interviewer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InterviewId,VacancyId,ApplicantId,InterviewerId,Date,Time,Result")] Interview interview)
        {
            if (id != interview.InterviewId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Interviews.Any(e => e.InterviewId == interview.InterviewId))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "VacancyId", "Title", interview.VacancyId);
            ViewData["ApplicantId"] = new SelectList(_context.Applicants, "ApplicantId", "Name", interview.ApplicantId);
            ViewData["InterviewerId"] = new SelectList(_context.Users.Where(u => u.Role == "Interviewer"), "Userid", "Username", interview.InterviewerId);
            return View(interview);
        }

        // GET: Interview/Delete/5
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var interview = await _context.Interviews
                .Include(i => i.Vacancy)
                .Include(i => i.Applicant)
                .Include(i => i.Interviewer)
                .FirstOrDefaultAsync(m => m.InterviewId == id);
            if (interview == null) return NotFound();

            return View(interview);
        }

        // POST: Interview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interview = await _context.Interviews.FindAsync(id);
            _context.Interviews.Remove(interview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Interview/GenerateReport/5
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GenerateReport(int id)
        {
            var applicant = await _context.Applicants
                .FirstOrDefaultAsync(a => a.ApplicantId == id);

            if (applicant == null)
            {
                return NotFound();
            }

            // Fetch all interviews for the applicant
            var interviews = await _context.Interviews
                .Include(i => i.VacancyId)
                .Include(i => i.ApplicantId)
                .Include(i => i.InterviewerId)
                .Where(i => i.ApplicantId == id)
                .ToListAsync();

            if (interviews == null || !interviews.Any())
            {
                ViewBag.Message = "No interviews found for this applicant.";
                return View();
            }

            // Pass the interviews and applicant to the report view
            ViewBag.ApplicantName = applicant.Name;
            ViewBag.ApplicantId = applicant.ApplicantId;

            return View(interviews);
        }

    }
}
