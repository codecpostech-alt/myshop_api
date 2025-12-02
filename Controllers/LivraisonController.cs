using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using MyShop.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Controllers
{
    public class LivraisonController : Controller
    {
        private static List<Carrier> _carriers = new List<Carrier>
        {
            new Carrier { Id = 1, Name = "ECOM DELIVERY", LogoUrl = "~/images/ecom.png", IsActive = false },
            new Carrier { Id = 2, Name = "SAMEX DELIVERY", LogoUrl = "~/images/samex.png", IsActive = false },
            new Carrier { Id = 3, Name = "ZR EXPRESS", LogoUrl = "~/images/zr.png", IsActive = false },
        };

        public async Task<IActionResult> Livraison()
        {
            // 🔹 تحقق من حالة تفعيل ECOM
            var ecomService = new EcomDeliveryService(
                "8bb0d1411b85412999432401e090c7be",
                "dd557a88-82b0-4859-8328-8aeb3c887f40"
            );

            bool isEcomActive = await ecomService.IsActivatedAsync();

            // 🔹 حدث حالة الشركة في القائمة
            var ecom = _carriers.FirstOrDefault(c => c.Name == "ECOM DELIVERY");
            if (ecom != null)
                ecom.IsActive = isEcomActive;

            return View(_carriers);
        }

        [HttpPost]
        public IActionResult ToggleActive(int id)
        {
            var c = _carriers.FirstOrDefault(x => x.Id == id);
            if (c != null)
                c.IsActive = !c.IsActive;

            return Json(new { success = true, isActive = c?.IsActive });
        }
    }
}