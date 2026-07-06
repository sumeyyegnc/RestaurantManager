using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagerMVC.Helpers;
using RestaurantManagerMVC.Models;
using RestaurantManagerMVC.Services;

namespace RestaurantManagerMVC.Controllers
{
    public class SiparisController : Controller
    {
        private readonly ExportService _exportService;

        public SiparisController(ExportService exportService)
        {
            _exportService = exportService;
        }

        // GET: Siparis
        public IActionResult Index(string? term)
        {
            List<Siparis>? liste;

            if (!string.IsNullOrWhiteSpace(term))
            {
                liste = ApiContext.Get<List<Siparis>>($"Siparis/search?term={Uri.EscapeDataString(term)}");
                ViewBag.Term = term;
            }
            else
            {
                liste = ApiContext.Get<List<Siparis>>("Siparis");
            }

            return View(liste ?? new List<Siparis>());
        }

        public IActionResult Details(int id)
        {
            var siparis = ApiContext.Get<Siparis>($"Siparis/{id}");
            if (siparis == null) return NotFound();
            return View(siparis);
        }

        public IActionResult Create()
        {
            SetListeler();
            return View(new Siparis { Durum = "Beklemede", Adet = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Siparis siparis)
        {
            if (!ModelState.IsValid)
            {
                SetListeler();
                return View(siparis);
            }

            var (success, error) = ApiContext.Post("Siparis", siparis);
            if (!success)
            {
                ModelState.AddModelError("", "Kayit eklenemedi: " + error);
                SetListeler();
                return View(siparis);
            }

            TempData["Basari"] = "Siparis basariyla olusturuldu.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var siparis = ApiContext.Get<Siparis>($"Siparis/{id}");
            if (siparis == null) return NotFound();
            SetListeler();
            return View(siparis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Siparis siparis)
        {
            if (id != siparis.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                SetListeler();
                return View(siparis);
            }

            var (success, error) = ApiContext.Put($"Siparis/{id}", siparis);
            if (!success)
            {
                ModelState.AddModelError("", "Guncelleme basarisiz: " + error);
                SetListeler();
                return View(siparis);
            }

            TempData["Basari"] = "Siparis guncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var siparis = ApiContext.Get<Siparis>($"Siparis/{id}");
            if (siparis == null) return NotFound();
            return View(siparis);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ApiContext.Delete($"Siparis/{id}");
            TempData["Basari"] = "Siparis silindi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ExportPdf(string? term)
        {
            var liste = string.IsNullOrWhiteSpace(term)
                ? ApiContext.Get<List<Siparis>>("Siparis")
                : ApiContext.Get<List<Siparis>>($"Siparis/search?term={Uri.EscapeDataString(term)}");

            var pdf = _exportService.SiparislerPdf(liste ?? new List<Siparis>());
            return File(pdf, "application/pdf", "Siparisler.pdf");
        }

        public IActionResult ExportExcel(string? term)
        {
            var liste = string.IsNullOrWhiteSpace(term)
                ? ApiContext.Get<List<Siparis>>("Siparis")
                : ApiContext.Get<List<Siparis>>($"Siparis/search?term={Uri.EscapeDataString(term)}");

            var excel = _exportService.SiparislerExcel(liste ?? new List<Siparis>());
            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Siparisler.xlsx");
        }

        private void SetListeler()
        {
            var restoranlar = ApiContext.Get<List<Restoran>>("Restoran") ?? new List<Restoran>();
            var yemekler = ApiContext.Get<List<Yemek>>("Yemek") ?? new List<Yemek>();

            ViewBag.Restoranlar = new SelectList(restoranlar, "Id", "Ad");
            ViewBag.Yemekler = new SelectList(yemekler, "Id", "Ad");
            ViewBag.Durumlar = new SelectList(new[] { "Beklemede", "Hazirlaniyor", "Tamamlandi", "Iptal" });
        }
    }
}
