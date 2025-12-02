using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class PreVendeursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PreVendeursController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.PreVendeurs.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PreVendeur model)
        {
            if (ModelState.IsValid)
            {
                _context.PreVendeurs.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vendeur = await _context.PreVendeurs.FindAsync(id);
            return View(vendeur);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PreVendeur model)
        {
            if (ModelState.IsValid)
            {
                _context.PreVendeurs.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vendeur = await _context.PreVendeurs.FindAsync(id);
            _context.PreVendeurs.Remove(vendeur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
