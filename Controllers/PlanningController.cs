using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class PlanningController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanningController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTE PLANNING D'UN CHAUFFEUR
        public async Task<IActionResult> Index(int chauffeurId)
        {
            var chauffeur = await _context.Chauffeurs.FindAsync(chauffeurId);
            if (chauffeur == null) return NotFound();

            var planning = await _context.Plannings
                .Where(p => p.ChauffeurId == chauffeurId)
                .OrderByDescending(p => p.DatePlanning)
                .ToListAsync();

            ViewBag.Chauffeur = chauffeur;

            return View(planning);
        }

        // CREATE
        public IActionResult Create(int chauffeurId)
        {
            ViewBag.ChauffeurId = chauffeurId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Planning model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.DatePlanning = DateTime.SpecifyKind(model.DatePlanning, DateTimeKind.Utc);

            _context.Plannings.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { chauffeurId = model.ChauffeurId });
        }
    }
}