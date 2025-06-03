using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkoutDiaryMVC.Data;
using WorkoutDiaryMVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace WorkoutDiaryMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                ModelState.AddModelError("", "User already exists.");
                return View();
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["RegistrationSuccess"] = "Registration Success.";
            return RedirectToAction("Login");
        }


        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var hash = HashPassword(password);
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);

            if (user != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Workout");
            }

            ModelState.AddModelError("", "Invalid credentials.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
