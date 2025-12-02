using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using System;
using System.Linq;

namespace MyShop.Controllers
{
    public class VenteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VenteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // الصفحة الرئيسية لإنشاء البيع
        public IActionResult Index()
        {
            var model = new VenteViewModel
            {
                ProduitsList = _context.Produits.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddProduit(VenteViewModel model, int produitId, int quantite)
        {
            var produit = _context.Produits.FirstOrDefault(p => p.Id == produitId);
            if (produit == null)
                ModelState.AddModelError("", "Le produit n'existe pas.");
            else if (quantite <= 0)
                ModelState.AddModelError("", "La quantité doit être supérieure à zéro.");
            else if (quantite > produit.Quantite)
                ModelState.AddModelError("", $"Quantité requise ({quantite}) dépasse le stock disponible ({produit.Quantite}).");
            else
                model.Produits.Add(new VenteProduit
                {
                    ProduitId = produit.Id,
                    ProduitNom = produit.Nom,
                    Quantite = quantite,
                    PrixUnitaire = produit.PrixVente
                });

            model.ProduitsList = _context.Produits.ToList();
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Save(VenteViewModel model)
        {
            if (model.Produits == null || !model.Produits.Any())
            {
                ModelState.AddModelError("", "Vous devez ajouter au moins un produit.");
                model.ProduitsList = _context.Produits.ToList();
                return View("Index", model);
            }

            foreach (var item in model.Produits)
            {
                var produit = _context.Produits.FirstOrDefault(p => p.Id == item.ProduitId);
                if (produit == null || item.Quantite > produit.Quantite)
                {
                    ModelState.AddModelError("", $"Le produit {item.ProduitNom} n'a pas assez de stock.");
                    model.ProduitsList = _context.Produits.ToList();
                    return View("Index", model);
                }
            }

            var vente = new Vente
            {
                ClientNom = string.IsNullOrWhiteSpace(model.ClientNom) ? "CLIENT ANONYME" : model.ClientNom,
                DateVente = model.DateVente == default ? DateTime.UtcNow : model.DateVente.ToUniversalTime(),
                MontantTotal = model.MontantTotal,
                VenteDetails = model.Produits.Select(p => new VenteDetail
                {
                    ProduitId = p.ProduitId,
                    ProduitNom = p.ProduitNom,
                    Quantite = p.Quantite,
                    PrixUnitaire = p.PrixUnitaire
                }).ToList()
            };

            foreach (var item in vente.VenteDetails)
            {
                var produit = _context.Produits.FirstOrDefault(p => p.Id == item.ProduitId);
                if (produit != null)
                {
                    produit.Quantite -= item.Quantite;
                    _context.Produits.Update(produit);
                }
            }

            _context.Ventes.Add(vente);
            _context.SaveChanges();

            TempData["Success"] = "La vente a été enregistrée avec succès!";
            return RedirectToAction("Index");
        }

        // Rapport des ventes avec filtre par date
        public IActionResult Rapport(DateTime? dateStart, DateTime? dateEnd)
        {
            var ventes = _context.Ventes
                .Include(v => v.VenteDetails)
                .AsQueryable();

            if (dateStart.HasValue)
                ventes = ventes.Where(v => v.DateVente >= dateStart.Value.Date);

            if (dateEnd.HasValue)
                ventes = ventes.Where(v => v.DateVente <= dateEnd.Value.Date.AddDays(1).AddTicks(-1));

            var model = ventes
                .OrderByDescending(v => v.DateVente)
                .ToList();

            return View("RapportVentes", model);
        }

        // Détails d'une vente spécifique
        public IActionResult Details(int id)
        {
            var vente = _context.Ventes
                .Include(v => v.VenteDetails)
                .FirstOrDefault(v => v.Id == id);

            if (vente == null)
                return NotFound();

            return View(vente);
        }
    }
}