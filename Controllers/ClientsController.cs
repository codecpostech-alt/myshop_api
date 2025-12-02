using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using System.Linq;

namespace MyShop.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: عرض كل الزبائن
        public IActionResult Index()
        {
            var clients = _context.Clients.ToList();
            return View(clients);
        }

        // GET: Create / Edit
        [HttpGet]
        public IActionResult Save(int? id)
        {
            if (id == null)
                return View(new Client());

            var client = _context.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null) return NotFound();
            return View(client);
        }

        // POST: Create / Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Client client)
        {
            if (!ModelState.IsValid) return View(client);

            if (client.Id == 0)
                _context.Clients.Add(client);
            else
            {
                var clientInDb = _context.Clients.FirstOrDefault(c => c.Id == client.Id);
                if (clientInDb == null) return NotFound();

                clientInDb.Nom = client.Nom;
                clientInDb.Mobile = client.Mobile;
                clientInDb.Email = client.Email;
                clientInDb.Adresse = client.Adresse;
                clientInDb.Activite = client.Activite;
                clientInDb.Rc = client.Rc;
                clientInDb.Ai = client.Ai;
                clientInDb.Nif = client.Nif;
                clientInDb.Nis = client.Nis;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Details
        public IActionResult Details(int id)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null) return NotFound();

            return View(client);
        }
    }
}