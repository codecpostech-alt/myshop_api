using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using System.Linq;

namespace MyShop.Controllers
{
    public class FournisseursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FournisseursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // عرض كل الموردين
        public IActionResult Index()
        {
            var fournisseurs = _context.Fournisseurs.ToList();
            return View(fournisseurs);
        }

        // GET: إنشاء أو تعديل
        [HttpGet]
        public IActionResult Save(int? id)
        {
            if (id == null)
            {
                ViewBag.Action = "Créer";
                return View("Create", new Fournisseur());
            }

            var fournisseur = _context.Fournisseurs.FirstOrDefault(f => f.Id == id);
            if (fournisseur == null) return NotFound();

            ViewBag.Action = "Modifier";
            return View("Create", fournisseur); // استخدم نفس صفحة Create
        }

        // POST: إنشاء أو تعديل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Fournisseur fournisseur)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = fournisseur.Id > 0 ? "Modifier" : "Créer";
                return View("Create", fournisseur);
            }

            if (fournisseur.Id == 0)
            {
                _context.Fournisseurs.Add(fournisseur);
                TempData["SuccessMessage"] = "✅ Fournisseur ajouté avec succès !";
            }
            else
            {
                var fournisseurInDb = _context.Fournisseurs.FirstOrDefault(f => f.Id == fournisseur.Id);
                if (fournisseurInDb == null) return NotFound();

                fournisseurInDb.Nom = fournisseur.Nom;
                fournisseurInDb.Mobile = fournisseur.Mobile;
                fournisseurInDb.Email = fournisseur.Email;
                fournisseurInDb.Adresse = fournisseur.Adresse;
                fournisseurInDb.Activite = fournisseur.Activite;
                fournisseurInDb.Rc = fournisseur.Rc;
                fournisseurInDb.Ai = fournisseur.Ai;
                fournisseurInDb.Nif = fournisseur.Nif;
                fournisseurInDb.Nis = fournisseur.Nis;
                fournisseurInDb.Solde = fournisseur.Solde;

                TempData["SuccessMessage"] = "✅ Fournisseur modifié avec succès !";
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // حذف المورد
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var fournisseur = _context.Fournisseurs.FirstOrDefault(f => f.Id == id);
            if (fournisseur == null) return NotFound();

            _context.Fournisseurs.Remove(fournisseur);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "✅ Fournisseur supprimé avec succès !";
            return RedirectToAction("Index");
        }
    }
}