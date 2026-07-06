using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerMVC.Models
{
    public class Restoran
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Restoran adi zorunludur.")]
        [Display(Name = "Restoran Adi")]
        public string Ad { get; set; } = string.Empty;

        [Display(Name = "Adres")]
        public string? Adres { get; set; }

        [Display(Name = "Telefon")]
        public string? Telefon { get; set; }

        [Display(Name = "Sehir")]
        public string? Sehir { get; set; }
    }
}
