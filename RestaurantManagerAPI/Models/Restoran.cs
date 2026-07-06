namespace RestaurantManagerAPI.Models
{
    public class Restoran
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string? Adres { get; set; }
        public string? Telefon { get; set; }
        public string? Sehir { get; set; }
    }
}
