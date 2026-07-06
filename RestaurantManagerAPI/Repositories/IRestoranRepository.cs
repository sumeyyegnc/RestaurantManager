using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Repositories
{
    public interface IRestoranRepository
    {
        Task<IEnumerable<Restoran>> GetAllAsync();
        Task<Restoran?> GetByIdAsync(int id);
        Task<IEnumerable<Restoran>> SearchAsync(string term);
        Task<int> InsertAsync(Restoran restoran);
        Task UpdateAsync(Restoran restoran);
        Task DeleteAsync(int id);
    }
}
