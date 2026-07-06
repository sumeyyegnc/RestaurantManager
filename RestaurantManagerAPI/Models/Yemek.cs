namespace RestaurantManagerAPI.Models
{
    public class Yemek
    {
        public int Id { get; set; }
        public int RestoranId { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string? Kategori { get; set; }
        public decimal Fiyat { get; set; }

        public string? RestoranAdi { get; set; }
    }
}
