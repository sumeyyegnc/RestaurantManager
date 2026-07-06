using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagerMVC.Helpers;
using RestaurantManagerMVC.Models;
using RestaurantManagerMVC.Services;

namespace RestaurantManagerMVC.Controllers
{
    public class YemekController : Controller
    {
        private readonly ExportService _exportService;

        public YemekController(ExportService exportService)
        {
            _exportService = exportService;
        }

        // GET: Yemek
        public IActionResult Index(string? term)
        {
            List<Yemek>? liste;

            if (!string.IsNullOrWhiteSpace(term))
            {
                liste = ApiContext.Get<List<Yemek>>($"Yemek/search?term={Uri.EscapeDataString(term)}");
                ViewBag.Term = term;
            }
            else
            {
                liste = ApiContext.Get<List<Yemek>>("Yemek");
            }

            return View(liste ?? new List<Yemek>());
        }

        public IActionResult Details(int id)
        {
            var yemek = ApiContext.Get<Yemek>($"Yemek/{id}");
            if (yemek == null) return NotFound();
            return View(yemek);
        }

        public IActionResult Create()
        {
            SetRestoranListesi();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Yemek yemek)
        {
            if (!ModelState.IsValid)
            {
                SetRestoranListesi();
                return View(yemek);
            }

            var (success, error) = ApiContext.Post("Yemek", yemek);
            if (!success)
            {
                ModelState.AddModelError("", "Kayit eklenemedi: " + error);
                SetRestoranListesi();
                return View(yemek);
            }

            TempData["Basari"] = "Yemek basariyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var yemek = ApiContext.Get<Yemek>($"Yemek/{id}");
            if (yemek == null) return NotFound();
            SetRestoranListesi();
            return View(yemek);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Yemek yemek)
        {
            if (id != yemek.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                SetRestoranListesi();
                return View(yemek);
            }

            var (success, error) = ApiContext.Put($"Yemek/{id}", yemek);
            if (!success)
            {
                ModelState.AddModelError("", "Guncelleme basarisiz: " + error);
                SetRestoranListesi();
                return View(yemek);
            }

            TempData["Basari"] = "Yemek basariyla guncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var yemek = ApiContext.Get<Yemek>($"Yemek/{id}");
            if (yemek == null) return NotFound();
            return View(yemek);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ApiContext.Delete($"Yemek/{id}");
            TempData["Basari"] = "Yemek silindi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ExportPdf(string? term)
        {
            var liste = string.IsNullOrWhiteSpace(term)
                ? ApiContext.Get<List<Yemek>>("Yemek")
                : ApiContext.Get<List<Yemek>>($"Yemek/search?term={Uri.EscapeDataString(term)}");

            var pdf = _exportService.YemeklerPdf(liste ?? new List<Yemek>());
            return File(pdf, "application/pdf", "Yemekler.pdf");
        }

        public IActionResult ExportExcel(string? term)
        {
            var liste = string.IsNullOrWhiteSpace(term)
                ? ApiContext.Get<List<Yemek>>("Yemek")
                : ApiContext.Get<List<Yemek>>($"Yemek/search?term={Uri.EscapeDataString(term)}");

            var excel = _exportService.YemeklerExcel(liste ?? new List<Yemek>());
            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Yemekler.xlsx");
        }

        private void SetRestoranListesi()
        {
            var restoranlar = ApiContext.Get<List<Restoran>>("Restoran") ?? new List<Restoran>();
            ViewBag.Restoranlar = new SelectList(restoranlar, "Id", "Ad");
        }
    }
}
