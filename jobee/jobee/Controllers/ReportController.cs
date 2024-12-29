using Microsoft.AspNetCore.Mvc;

namespace jobee.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult ApplicantReport()
        {
            return View();
        }
    }
}
