using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerMVC.Models
{
    public class Yemek
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Restoran secimi zorunludur.")]
        [Display(Name = "Restoran")]
        public int RestoranId { get; set; }

        [Required(ErrorMessage = "Yemek adi zorunludur.")]
        [Display(Name = "Yemek Adi")]
        public string Ad { get; set; } = string.Empty;

        [Display(Name = "Kategori")]
        public string? Kategori { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, 100000, ErrorMessage = "Gecerli bir fiyat giriniz.")]
        [Display(Name = "Fiyat")]
        public decimal Fiyat { get; set; }

        public string? RestoranAdi { get; set; }
    }
}
