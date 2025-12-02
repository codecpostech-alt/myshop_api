using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using System.Linq;

namespace MyShop.Controllers
{
    public class ProduitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProduitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // عرض كل المنتجات
        public IActionResult Index()
        {
            var produits = _context.Produits.ToList();
            return View(produits);
        }

        // GET: إنشاء أو تعديل
        [HttpGet]
        public IActionResult Save(int? id)
        {
            if (id == null)
            {
                ViewBag.Action = "Créer";
                return View("Create", new Produit());
            }

            var produit = _context.Produits.FirstOrDefault(p => p.Id == id);
            if (produit == null) return NotFound();

            ViewBag.Action = "Modifier";
            return View("Create", produit); // استخدم نفس صفحة Create
        }

        // POST: إنشاء أو تعديل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Produit produit)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = produit.Id > 0 ? "Modifier" : "Créer";
                return View("Create", produit);
            }

            if (produit.Id == 0)
            {
                _context.Produits.Add(produit);
                TempData["SuccessMessage"] = "✅ Produit ajouté avec succès !";
            }
            else
            {
                var produitInDb = _context.Produits.FirstOrDefault(p => p.Id == produit.Id);
                if (produitInDb == null) return NotFound();

                produitInDb.Nom = produit.Nom;
                produitInDb.Categorie = produit.Categorie;
                produitInDb.Quantite = produit.Quantite;
                produitInDb.PrixAchat = produit.PrixAchat;
                produitInDb.PrixVente = produit.PrixVente;

                // حفظ الباركود بدل الوصف
                produitInDb.Barcode = produit.Barcode;

                TempData["SuccessMessage"] = "✅ Produit modifié avec succès !";
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // حذف المنتج
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var produit = _context.Produits.FirstOrDefault(p => p.Id == id);
            if (produit == null) return NotFound();

            _context.Produits.Remove(produit);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "✅ Produit supprimé avec succès !";
            return RedirectToAction("Index");
        }

        // طباعة المنتج
        public IActionResult Print(int? id)
        {
            if (id == null) return NotFound();

            var produit = _context.Produits.FirstOrDefault(p => p.Id == id);
            if (produit == null) return NotFound();

            return View(produit);
        }
    }
}