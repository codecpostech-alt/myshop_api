using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using SetShop.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace SetShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =======================
        // 🔹 REGISTER (التسجيل)
        // =======================
        [HttpGet]
        public IActionResult Register(string plan = null, string returnUrl = null)
        {
            ViewBag.Plan = plan;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
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
                    Password = model.Password // ⚠️ مستقبلاً استخدم التشفير
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                // ✅ حفظ اسم المستخدم في Session
                HttpContext.Session.SetString("UserName", user.FullName);

                // ✅ إرسال رسالة ترحيبية
                SendWelcomeEmail(user);

                // ✅ إعادة التوجيه بعد التسجيل
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        // =======================
        // 🔹 LOGIN (تسجيل الدخول)
        // =======================
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users
                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.FullName);

                HttpContext.Session.SetString("UserEmail", user.Email);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "L'adresse e-mail ou le mot de passe est incorrect ❌");
            return View(model);
        }

        // =======================
        // 🔹 FORGOT PASSWORD
        // =======================
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                TempData["Message"] = "❌ لم يتم العثور على مستخدم بهذا البريد الإلكتروني.";
                return View();
            }

            try
            {
                SendForgotPasswordEmail(user);
                TempData["Message"] = "✅ تم إرسال كلمة المرور إلى بريدك الإلكتروني.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "⚠️ حدث خطأ أثناء الإرسال: " + ex.Message;
            }

            return View();
        }

        // =======================
        // 🔹
        // 

        // =======================
        public IActionResult Dashboard()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
                return RedirectToAction("Login");

            ViewBag.UserName = userName;
            return View();
        }

        // =======================
        // 🔹 Page de succès
        // =======================
        public IActionResult Success()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }

        // =======================
        // 🔹 Methods for Emails
        // =======================
        private void SendWelcomeEmail(User user)
        {
            try
            {
                var fromAddress = new MailAddress("codecpostech@gmail.com", "CODEC POS");
                var toAddress = new MailAddress(user.Email, user.FullName);
                const string fromPassword = "kcju vexy lrpv gwjl";

                string subject = "🎉 Bienvenue / مرحباً بك في CODEC POS";
                string body = $"<h2>Bienvenue {user.FullName}</h2><p>مرحباً بك في CODEC POS</p>";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Erreur lors de l'envoi du mail: " + ex.Message);
            }
        }

        private void SendForgotPasswordEmail(User user)
        {
            var fromAddress = new MailAddress("codecpostech@gmail.com", "CODEC POS");
            var toAddress = new MailAddress(user.Email, user.FullName);
            const string fromPassword = "kcju vexy lrpv gwjl";

            string subject = "🔑 Réinitialisation du mot de passe / استرجاع كلمة المرور";
            string body = $"<p>Bonjour {user.FullName}</p><p>Email: {user.Email}<br>Mot de passe: {user.Password}</p>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}