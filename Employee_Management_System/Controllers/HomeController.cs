using System.Diagnostics;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Employee_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Redirection si l'utilisateur n'est pas HRManager
        private bool IsHRManager()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "HRManager";
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;
            return View(); // Page d'accueil publique
        }

        public IActionResult Privacy()
        {
            return View(); // Page d'infos publique
        }

        

        public IActionResult Dashboard()
        {
            if (!IsHRManager())
            {
                TempData["ErrorMessage"] = "Accès refusé. Réservé au HRManager.";
                return RedirectToAction("Index", "Compte");
            }

            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View(); // Page privée HRManager
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
