using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Text;
using jobee.Data;
using jobee.Models;
using System.Threading.Tasks;
using System.Linq;

namespace jobee.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Register view
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register user
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = HashPassword(user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: Login view
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login user
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == loginModel.Email);

                if (user != null && VerifyPassword(loginModel.Password, user.Password))
                {
                    // Create claims for the user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.Userid.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role) // Use standard ClaimTypes.Role
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetInt32("UserId", user.Userid);

                    // Redirect based on role
                    return user.Role switch
                    {
                        "Admin" => RedirectToAction("AdminPanel"),
                        "HR" => RedirectToAction("HrPanel"),
                        _ => RedirectToAction("Index", "Home")
                    };
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginModel);
        }

        private string HashPassword(string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes("YourSecureSaltHere");

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            byte[] salt = Encoding.UTF8.GetBytes("YourSecureSaltHere");

            string enteredPasswordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return enteredPasswordHash == storedPasswordHash;
        }

        // Logout action to clear session and cookies
        public async Task<IActionResult> Logout()
        {
            // Clear the session data
            HttpContext.Session.Clear();

            // Sign out the user and remove the cookies
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Optionally, you can also clear the authentication cookies manually:
            // HttpContext.Response.Cookies.Delete("YourCookieName");

            // Redirect to the login page
            return RedirectToAction("Login");
        }

        /* USER MANAGEMENT */
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Hash the password before saving
                user.Password = HashPassword(user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("AdminPanel");
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.Find(user.Userid);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Update user details, including password if changed
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = HashPassword(user.Password);
                }

                _context.SaveChanges();
                return RedirectToAction("AdminPanel");
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("AdminPanel");
        }

        /* Display the users */
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            var users = _context.Users.ToList(); // Get all users
            return View(users);
        }



        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "HR")]
        public IActionResult HrPanel()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Interviewer")]
        public IActionResult InterviewerPanel()
        {
            return View();
        }

        /*[Authorize(Roles = "Applicant")]
        public IActionResult ApplicantPanel()
        {
            return View();
        }*/

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
