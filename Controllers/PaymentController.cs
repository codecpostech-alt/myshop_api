using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using System;
using System.Threading.Tasks;

namespace SetShop.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ChargilyPaymentService _paymentService;
        private readonly ApplicationDbContext _context;

        public PaymentController(ChargilyPaymentService paymentService, ApplicationDbContext context)
        {
            _paymentService = paymentService;
            _context = context;
        }

        // =======================
        // 🔹 StartPayment
        // =======================
        [HttpGet]
        public IActionResult StartPayment(string plan)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                string returnUrl = Url.Action("Checkout", "Payment", new { plan });
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            return RedirectToAction("Checkout", new { plan });
        }

        // =======================
        // 🔹 Checkout الفعلية
        // =======================
        public async Task<IActionResult> Checkout(string plan)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", new { plan }) });

            decimal amount = plan switch
            {
                "monthly" => 3000,
                "yearly" => 15000,
                _ => 0
            };

            if (amount <= 0)
            {
                TempData["Message"] = "Plan non valide!";
                return RedirectToAction("Dashboard", "Account");
            }

            string successUrl = Url.Action("Success", "Payment", new { plan, amount }, Request.Scheme);
            string failureUrl = Url.Action("Failure", "Payment", null, Request.Scheme);

            // إنشاء رابط الدفع عبر خدمة Chargily
            string checkoutId = await _paymentService.CreateCheckoutAsync(amount, plan, successUrl, failureUrl);

            // ✅ تسجيل الدفع في قاعدة البيانات
            var payment = new PaymentRecord
            {
                Email = userEmail,
                Plan = plan,
                Amount = amount,
                CheckoutId = checkoutId,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };
            _context.Payments.Add(payment);
            _context.SaveChanges();

            return Redirect(checkoutId);
        }

        // =======================
        // 🔹 Success
        // =======================
        public IActionResult Success(string plan, decimal amount, string checkout_id)
        {
            // تحديث حالة الدفع
            var record = _context.Payments.FirstOrDefault(p => p.CheckoutId == checkout_id);
            if (record != null)
            {
                record.Status = "success";
                _context.SaveChanges();
            }

            TempData["Message"] = $"تم الدفع بنجاح للباقة {plan} ({amount} DA)";
            return RedirectToAction("Dashboard", "Account");
        }

        // =======================
        // 🔹 Failure
        // =======================
        public IActionResult Failure(string checkout_id)
        {
            var record = _context.Payments.FirstOrDefault(p => p.CheckoutId == checkout_id);
            if (record != null)
            {
                record.Status = "failed";
                _context.SaveChanges();
            }

            TempData["Message"] = "فشل الدفع. حاول مرة أخرى.";
            return RedirectToAction("Dashboard", "Account");
        }
    }
}