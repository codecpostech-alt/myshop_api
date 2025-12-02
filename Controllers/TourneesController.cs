using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class TourneesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TourneesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.Tournees
                .Include(c => c.Chauffeur)
                .Include(p => p.PreVendeur)
                .ToListAsync();

            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Chauffeurs = _context.Chauffeurs.ToList();
            ViewBag.PreVendeurs = _context.PreVendeurs.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tournee model)
        {
            model.DateTournee = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Tournees.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
