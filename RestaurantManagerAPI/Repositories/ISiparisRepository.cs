using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Repositories
{
    public interface ISiparisRepository
    {
        Task<IEnumerable<Siparis>> GetAllAsync();
        Task<Siparis?> GetByIdAsync(int id);
        Task<IEnumerable<Siparis>> SearchAsync(string term);
        Task<int> InsertAsync(Siparis siparis);
        Task UpdateAsync(Siparis siparis);
        Task DeleteAsync(int id);
    }
}
