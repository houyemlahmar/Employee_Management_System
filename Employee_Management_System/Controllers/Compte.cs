using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Employee_Management_System.Data;
using Employee_Management_System.Models;

namespace Employee_Management_System.Controllers
{
    public class CompteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Utilisateur> _passwordHasher;

        public CompteController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Utilisateur>();
        }
        
        public IActionResult Index()
        {
            ViewBag.Roles = new[] { "HRManager", "Employee" };
            return View(new Utilisateur());
        }
        [HttpGet]
        public IActionResult LoginHR()
        {
            ViewBag.Roles = new[] { "HRManager" };
            return View("LoginHR");
        }
        [HttpGet]
        public IActionResult RegisterHR()
        {
            ViewBag.Roles = new[] { "HRManager" };
            return View("RegisterHR");
        }
        [HttpGet]
        public IActionResult LoginEmployee()
        {
            ViewBag.Roles = new[] { "Employee" };
            return View("LoginEmployee");
        }

        [HttpPost]
        public IActionResult Register(Utilisateur utilisateur)
        {
            ViewBag.Roles = new[] { "HRManager" };

            var currentUserRole = HttpContext.Session.GetString("UserRole");
            
            if (string.IsNullOrWhiteSpace(utilisateur.Username) ||
                string.IsNullOrWhiteSpace(utilisateur.Email) ||
                string.IsNullOrWhiteSpace(utilisateur.Password) ||
                string.IsNullOrWhiteSpace(utilisateur.Role))
            {
                TempData["ErrorMessage"] = "All fields are required.";
                return View("RegisterHR", utilisateur);
            }
            
            if (ModelState.IsValid)
            {
                var existingUser = _context.Utilisateurs.FirstOrDefault(u => u.Email == utilisateur.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exist.");
                    return View("RegisterHR", utilisateur); 
                }

                utilisateur.Password = _passwordHasher.HashPassword(utilisateur, utilisateur.Password);
                _context.Utilisateurs.Add(utilisateur);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Account created successfully.";
                return RedirectToAction("LoginHR"); 
            }

            return View("RegisterHR", utilisateur);
        }


        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            ViewBag.Roles = new[] { "HRManager" };

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Please fill all the fields!");
                return View("LoginHR");
            }

            var user = _context.Utilisateurs.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserRole", user.Role);
                    HttpContext.Session.SetString("Username", user.Username);

                    TempData["SuccessMessage"] = "Connected successfully.";
                    return RedirectToAction("Dashboard", "Home");
                }

                TempData["ErrorMessage"] = "Wrong password! Check again.";
                return View("LoginHR");
            }

            TempData["ErrorMessage"] = "No HR Manager found with this account!";
            return View("LoginHR");
        }
        [HttpPost]
        public IActionResult LoginEmployee(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Please fill all the fields!");
                return View("LoginEmployee");
            }

            var employe = _context.Employes.FirstOrDefault(e => e.Email == email);
            if (employe != null)
            {
                if (employe.Password == password)
                {
                    HttpContext.Session.SetString("UserId", employe.Id.ToString());
                    HttpContext.Session.SetString("UserRole", "Employee");
                    HttpContext.Session.SetString("Username", employe.FullName);

                    TempData["SuccessMessage"] = "Connected successfully.";
                    return RedirectToAction("DashboardEmployee", "Employes");
                }

                TempData["ErrorMessage"] = "Wrong password! Check again.";
                return View("LoginEmployee");
            }

            TempData["ErrorMessage"] = "No Employee found with this account!";
            return View("LoginEmployee");
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Deconnected successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}
