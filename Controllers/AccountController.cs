using Microsoft.AspNetCore.Mvc;
using NewsApp.Data;
using NewsApp.Models;

namespace NewsApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register() {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Name,Password")] User user)
        {
            
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Name == user.Name))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(user);
                }

                user.Password = Utility.Hash(user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Articles");
            }

            return View(user);
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Name,Password")] User user)
        {
            var authenticatedUser = _context.Users.FirstOrDefault(u => u.Name == user.Name && u.Password == Utility.Hash(user.Password));

            if (authenticatedUser != null)
            {
                HttpContext.Session.SetInt32("UserId", authenticatedUser.UID);
                HttpContext.Session.SetString("Username", authenticatedUser.Name);

                return RedirectToAction("Index", "Articles"); // Redirect to the home page or any other page
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password";

                return View();
            }
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            // Destroy the session
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public int CheckSessionStatus()
        {
            Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
            return (int)HttpContext.Session.GetInt32("UserId");
           
        }
    }
}
