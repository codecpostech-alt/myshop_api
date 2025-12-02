using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace MyShop.Controllers
{
    public class FraisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FraisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ الأنواع الجاهزة للمصاريف
        private readonly string[] _typesFrais =
        {
            "Loyer",           // الإيجار
            "Électricité",     // الكهرباء
            "Eau",             // الماء
            "Internet",        // الإنترنت
            "Transport",       // النقل
            "Maintenance",     // الصيانة
            "Salaire Employé", // راتب الموظف
            "Autre"            // أخرى
        };

        // ✅ عرض جميع المصاريف
        public IActionResult Index()
        {
            var fraisList = _context.Frais
                .OrderByDescending(f => f.DateFrais)
                .ToList();

            return View(fraisList);
        }

        // ✅ صفحة الإضافة
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TypesFrais = _typesFrais;
            return View();
        }

        // ✅ حفظ المصروف الجديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Frais frais)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.TypesFrais = _typesFrais;
                    return View(frais);
                }

                // جلب المستخدم من الجلسة
                var userName = HttpContext.Session.GetString("UserName");
                frais.Utilisateur = string.IsNullOrEmpty(userName) ? "Utilisateur inconnu" : userName;

                frais.DateFrais = DateTime.UtcNow;

                _context.Frais.Add(frais);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "✅ Le frais a été ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "❌ Une erreur est survenue : " + ex.Message;
                ViewBag.TypesFrais = _typesFrais;
                return View(frais);
            }
        }

        // ✅ حذف مصروف
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var frais = _context.Frais.Find(id);
            if (frais == null)
            {
                TempData["ErrorMessage"] = "❌ Frais introuvable.";
                return RedirectToAction(nameof(Index));
            }

            _context.Frais.Remove(frais);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "🗑️ Le frais a été supprimé avec succès.";
            return RedirectToAction(nameof(Index));
        }
    }
}