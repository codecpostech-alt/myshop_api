using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using SetShop.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================================
        // 🔥 API LOGIN (FLUTTER)
        // ======================================
        [HttpPost("login")]
        public IActionResult ApiLogin([FromBody] LoginViewModel model)
        {
            if (model == null ||
                string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { error = "Email and password are required" });
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
                return Unauthorized(new { error = "Invalid email or password" });

            return Ok(new
            {
                message = "Login successful",
                fullName = user.FullName,
                email = user.Email,
                phone = user.PhoneNumber,
                country = user.Country
            });
        }

        // ======================================
        // 🔥 API REGISTER (FLUTTER)
        // ======================================
        [HttpPost("register")]
        public IActionResult ApiRegister([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data" });

            if (_context.Users.Any(u => u.Email == model.Email))
                return BadRequest(new { error = "Email already exists" });

            var user = new User
            {
                FullName = model.FullName,
                Country = model.Country,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Password = model.Password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Registration successful",
                fullName = user.FullName,
                email = user.Email
            });
        }



        // ===========================================================
        // 🔵 MVC ROUTES (FIXED) — مهم جداً حتى لا يخرب API في Render
        // ===========================================================

        [HttpGet]
        [Route("[action]")]
        public IActionResult Register(string plan = null, string returnUrl = null)
        {
            ViewBag.Plan = plan;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Cet e-mail est déjà utilisé ❌");
                    return View(model);
                }

                var user = new User
                {
                    FullName = model.FullName,
                    Country = model.Country,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Password = model.Password
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                HttpContext.Session.SetString("UserName", user.FullName);
                SendWelcomeEmail(user);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Dashboard");
            }

            return View(model);
        }



        // ============================
        // 🔵 MVC LOGIN (SAFE ROUTE)
        // ============================

        [HttpGet]
        [Route("[action]")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            ModelState.Remove("returnUrl");

            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users
                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserEmail", user.Email);

                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = Url.Action("Dashboard", "Account");

                return Redirect(returnUrl);
            }

            ModelState.AddModelError("", "L'adresse e-mail ou le mot de passe est incorrect ❌");
            return View(model);
        }



        // ============================
        // 🔵 DASHBOARD (MVC)
        // ============================

        [HttpGet]
        [Route("[action]")]
        public IActionResult Dashboard()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
                return RedirectToAction("Login");

            ViewBag.UserName = userName;
            return View();
        }



        // ============================
        // 🔵 EMAIL FUNCTIONS
        // ============================

        private void SendWelcomeEmail(User user)
        {
            try
            {
                var fromAddress = new MailAddress("codecpostech@gmail.com", "CODEC POS");
                var toAddress = new MailAddress(user.Email, user.FullName);
                const string fromPassword = "kcju vexy lrpv gwjl";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Bienvenue / مرحباً بك",
                    Body = $"<h2>Bienvenue {user.FullName}</h2>",
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
            catch { }
        }
    }
}
