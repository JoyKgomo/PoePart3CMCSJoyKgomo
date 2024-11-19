using Microsoft.AspNetCore.Mvc;
using POEPART3CMCS.Models;
using System.Diagnostics;

namespace POEPART3CMCS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogUserIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("Login");
            }

            string username = model.username;
            string password = model.password;

            int id;
            string role;

            (id, role) = DBContext.Login(username, password);
            if (id == 0)
            {
                return View("Login");
            }

            HttpContext.Session.SetInt32("user_id", id);

            // Redirect based on role
            if (role == "Lecturer")
            {
                AppStateModel.State = "Lecturer";
                return RedirectToAction("Dashboard", "Lecturer");
            }
            else
            if (role =="HR")
            {
                AppStateModel.State = "HR"; // Consistent use of a string role
                return RedirectToAction("Index", "Users");
            }
            else
            if (role=="Academic Manger")
            { 
                AppStateModel.State = "CoordinatorManager"; // Consistent use of a string role
                return RedirectToAction("Dashboard", "AcademicManager");
            }
            else
            {
                AppStateModel.State = "CoordinatorManager"; // Consistent use of a string role
                return RedirectToAction("Dashboard", "ProgrammeCoordinator");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
