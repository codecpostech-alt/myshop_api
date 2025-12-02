using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class ChauffeursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChauffeursController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Chauffeurs.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Chauffeur model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 🔥 تحويل التاريخ إلى UTC لتجنب المشكل
            if (model.ExpirationPermis.HasValue)
            {
                model.ExpirationPermis = DateTime.SpecifyKind(
                    model.ExpirationPermis.Value,
                    DateTimeKind.Utc
                );
            }

            _context.Chauffeurs.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Chauffeur ajouté avec succès !";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var chauffeur = await _context.Chauffeurs.FindAsync(id);
            if (chauffeur == null) return NotFound();
            return View(chauffeur);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Chauffeur model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ExpirationPermis.HasValue)
            {
                model.ExpirationPermis = DateTime.SpecifyKind(
                    model.ExpirationPermis.Value,
                    DateTimeKind.Utc
                );
            }

            _context.Chauffeurs.Update(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Chauffeur modifié avec succès !";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var chauffeur = await _context.Chauffeurs.FindAsync(id);
            if (chauffeur == null) return NotFound();
            return View(chauffeur);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var chauffeur = await _context.Chauffeurs.FindAsync(id);
            if (chauffeur == null) return NotFound();

            _context.Chauffeurs.Remove(chauffeur);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Chauffeur supprimé avec succès !";

            return RedirectToAction(nameof(Index));
        }
    }
}