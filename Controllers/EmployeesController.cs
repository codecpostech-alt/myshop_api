using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using System;
using System.Linq;

namespace MyShop.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ عرض جميع الموظفين
        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }

        // ✅ إنشاء أو تعديل موظف (GET)
        [HttpGet]
        public IActionResult Save(int? id)
        {
            if (id == null)
            {
                ViewBag.Action = "Créer un employé";
                return View("Create", new Employee());
            }

            var emp = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null) return NotFound();

            ViewBag.Action = "Modifier l'employé";
            return View("Create", emp);
        }

        // ✅ إنشاء أو تعديل موظف (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Employee emp)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = emp.Id == 0 ? "Créer un employé" : "Modifier l'employé";
                return View("Create", emp);
            }

            // ✅ تحويل كل التواريخ إلى UTC لتجنب الخطأ مع PostgreSQL
            if (emp.DateNaissance.HasValue)
            {
                emp.DateNaissance = DateTime.SpecifyKind(emp.DateNaissance.Value, DateTimeKind.Utc);
            }

            if (emp.DateRecrutement != default)
            {
                emp.DateRecrutement = DateTime.SpecifyKind(emp.DateRecrutement, DateTimeKind.Utc);
            }

            if (emp.Id == 0)
            {
                _context.Employees.Add(emp);
                TempData["SuccessMessage"] = "✅ Employé ajouté avec succès !";
            }
            else
            {
                var empInDb = _context.Employees.FirstOrDefault(e => e.Id == emp.Id);
                if (empInDb == null) return NotFound();

                empInDb.Nom = emp.Nom;
                empInDb.Prenom = emp.Prenom;
                empInDb.DateNaissance = emp.DateNaissance;
                empInDb.Adresse = emp.Adresse;
                empInDb.Poste = emp.Poste;
                empInDb.NSS = emp.NSS;
                empInDb.DateRecrutement = emp.DateRecrutement;
                empInDb.SalaireBase = emp.SalaireBase;
                empInDb.Email = emp.Email;
                empInDb.Telephone = emp.Telephone;

                TempData["SuccessMessage"] = "✅ Employé modifié avec succès !";
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // ✅ حذف موظف
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var emp = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null) return NotFound();

            _context.Employees.Remove(emp);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "❌ Employé supprimé avec succès !";
            return RedirectToAction("Index");
        }
    }
}