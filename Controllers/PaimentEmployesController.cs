using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using System.Linq;

namespace MyShop.Controllers
{
    public class PaiementEmployesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaiementEmployesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ عرض جميع paiements
        public IActionResult Index()
        {
            var paiements = _context.PaiementEmployes
                .Include(p => p.Employee) // مهم جدًا حتى يمكن الوصول إلى p.Employee.Nom
                .Select(p => new
                {
                    p.Id,
                    Employe = p.Employee != null ? p.Employee.Nom + " " + p.Employee.Prenom : "(Aucun)",
                    p.DatePaiement,
                    p.Montant,
                    p.Remarque
                })
                .ToList();

            ViewBag.Paiements = paiements;
            return View(paiements);
        }

        // ✅ إنشاء paiement (GET)
        [HttpGet]
        public IActionResult Create(int? employeeId)
        {
            // عرض قائمة الموظفين مع الاسم الكامل
            var employeesList = _context.Employees
                .Select(e => new
                {
                    e.Id,
                    FullName = e.Nom + " " + e.Prenom
                })
                .ToList();

            ViewBag.Employees = new SelectList(employeesList, "Id", "FullName", employeeId);

            var model = new PaiementEmploye
            {
                EmployeeId = employeeId ?? 0,
                DatePaiement = DateTime.Now
            };

            return View(model);
        }

        // ✅ إنشاء paiement (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PaiementEmploye paiement)
        {
            if (!ModelState.IsValid)
            {
                var employeesList = _context.Employees
                    .Select(e => new
                    {
                        e.Id,
                        FullName = e.Nom + " " + e.Prenom
                    })
                    .ToList();

                ViewBag.Employees = new SelectList(employeesList, "Id", "FullName", paiement.EmployeeId);
                return View(paiement);
            }
            paiement.DatePaiement = DateTime.SpecifyKind(paiement.DatePaiement, DateTimeKind.Utc);
            _context.PaiementEmployes.Add(paiement);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "💰 Paiement enregistré avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // ✅ حذف paiement
        public IActionResult Delete(int id)
        {
            var paiement = _context.PaiementEmployes.FirstOrDefault(p => p.Id == id);
            if (paiement == null) return NotFound();

            _context.PaiementEmployes.Remove(paiement);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "🗑️ Paiement supprimé avec succès !";
            return RedirectToAction(nameof(Index));
        }
    }
}