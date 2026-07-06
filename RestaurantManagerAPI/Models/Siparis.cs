namespace RestaurantManagerAPI.Models
{
    public class Siparis
    {
        public int Id { get; set; }
        public int RestoranId { get; set; }
        public int YemekId { get; set; }
        public string MusteriAdi { get; set; } = string.Empty;
        public int Adet { get; set; } = 1;
        public DateTime Tarih { get; set; }
        public string Durum { get; set; } = "Beklemede";

        public string? RestoranAdi { get; set; }
        public string? YemekAdi { get; set; }
        public decimal Fiyat { get; set; }
    }
}
