using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using System.Linq;

namespace MyShop.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SettingsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var settings = _db.StoreSettings.FirstOrDefault();

            if (settings == null)
            {
                settings = new StoreSettings();
            }

            return View(settings);
        }

        [HttpPost]
        public IActionResult Save(StoreSettings model)
        {
            var exists = _db.StoreSettings.FirstOrDefault();

            if (exists == null)
            {
                _db.StoreSettings.Add(model);
            }
            else
            {
                exists.StoreName = model.StoreName;
                exists.BusinessType = model.BusinessType;
                exists.CommercialRegister = model.CommercialRegister;
                exists.TaxNumber = model.TaxNumber;
                exists.BankAccount = model.BankAccount;
                exists.Address = model.Address;
                exists.Phone = model.Phone;
                exists.DefaultTax = model.DefaultTax;
            }

            _db.SaveChanges();

            TempData["Message"] = "✔ Les paramètres ont été enregistrés avec succès";
            return RedirectToAction("Dashboard", "Account");
        }
    }
}