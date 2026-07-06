using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerMVC.Models
{
    public class Siparis
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Restoran secimi zorunludur.")]
        [Display(Name = "Restoran")]
        public int RestoranId { get; set; }

        [Required(ErrorMessage = "Yemek secimi zorunludur.")]
        [Display(Name = "Yemek")]
        public int YemekId { get; set; }

        [Required(ErrorMessage = "Musteri adi zorunludur.")]
        [Display(Name = "Musteri Adi")]
        public string MusteriAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adet zorunludur.")]
        [Range(1, 1000, ErrorMessage = "Adet en az 1 olmalidir.")]
        [Display(Name = "Adet")]
        public int Adet { get; set; } = 1;

        [Display(Name = "Tarih")]
        public DateTime Tarih { get; set; }

        [Display(Name = "Durum")]
        public string Durum { get; set; } = "Beklemede";

        public string? RestoranAdi { get; set; }
        public string? YemekAdi { get; set; }
        public decimal Fiyat { get; set; }
    }
}
