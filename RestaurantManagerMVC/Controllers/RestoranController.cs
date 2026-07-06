using Microsoft.AspNetCore.Mvc;
using RestaurantManagerMVC.Helpers;
using RestaurantManagerMVC.Models;
using RestaurantManagerMVC.Services;

namespace RestaurantManagerMVC.Controllers
{
    public class RestoranController : Controller
    {
        private readonly ExportService _exportService;

        public RestoranController(ExportService exportService)
        {
            _exportService = exportService;
        }

        // GET: Restoran
        public IActionResult Index(string? term)
        {
            List<Restoran>? liste;

            if (!string.IsNullOrWhiteSpace(term))
            {
                liste = ApiContext.Get<List<Restoran>>($"Restoran/search?term={Uri.EscapeDataString(term)}");
                ViewBag.Term = term;
            }
            else
            {
                liste = ApiContext.Get<List<Restoran>>("Restoran");
            }

            return View(liste ?? new List<Restoran>());
        }

        // GET: Restoran/Details/5
        public IActionResult Details(int id)
        {
            var restoran = ApiContext.Get<Restoran>($"Restoran/{id}");
            if (restoran == null) return NotFound();
            return View(restoran);
        }

        // GET: Restoran/Create
        public IActionResult Create() => View();

        // POST: Restoran/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Restoran restoran)
        {
            if (!ModelState.IsValid) return View(restoran);

            var (success, error) = ApiContext.Post("Restoran", restoran);
            if (!success)
            {
                ModelState.AddModelError("", "Kayit eklenemedi: " + error);
                return View(restoran);
            }

            TempData["Basari"] = "Restoran basariyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Restoran/Edit/5
        public IActionResult Edit(int id)
        {
            var restoran = ApiContext.Get<Restoran>($"Restoran/{id}");
            if (restoran == null) return NotFound();
            return View(restoran);
        }

        // POST: Restoran/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Restoran restoran)
        {
            if (id != restoran.Id) return BadRequest();
            if (!ModelState.IsValid) return View(restoran);

            var (success, error) = ApiContext.Put($"Restoran/{id}", restoran);
            if (!success)
            {
                ModelState.AddModelError("", "Guncelleme basarisiz: " + error);
                return View(restoran);
            }

            TempData["Basari"] = "Restoran basariyla guncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Restoran/Delete/5
        public IActionResult Delete(int id)
        {
            var restoran = ApiContext.Get<Restoran>($"Restoran/{id}");
            if (restoran == null) return NotFound();
            return View(restoran);
        }

        // POST: Restoran/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ApiContext.Delete($"Restoran/{id}");
            TempData["Basari"] = "Restoran silindi.";
            return RedirectToAction(nameof(Index));
        }

        // PDF export
        public IActionResult ExportPdf(string? term)
        {
            var liste = string.IsNullOrWhiteSpace(term)
                ? ApiContext.Get<List<Restoran>>("Restoran")
                : ApiContext.Get<List<Restoran>>($"Restoran/search?term={Uri.EscapeDataString(term)}");

            var pdf = _exportService.RestoranlarPdf(liste ?? new List<Restoran>());
            return File(pdf, "application/pdf", "Restoranlar.pdf");
        }

        // Excel export
        public IActionResult ExportExcel(string? term)
        {
            var liste = string.IsNullOrWhiteSpace(term)
                ? ApiContext.Get<List<Restoran>>("Restoran")
                : ApiContext.Get<List<Restoran>>($"Restoran/search?term={Uri.EscapeDataString(term)}");

            var excel = _exportService.RestoranlarExcel(liste ?? new List<Restoran>());
            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Restoranlar.xlsx");
        }
    }
}
