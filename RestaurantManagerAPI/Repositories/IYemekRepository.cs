using RestaurantManagerAPI.Models;

namespace RestaurantManagerAPI.Repositories
{
    public interface IYemekRepository
    {
        Task<IEnumerable<Yemek>> GetAllAsync();
        Task<Yemek?> GetByIdAsync(int id);
        Task<IEnumerable<Yemek>> SearchAsync(string term);
        Task<int> InsertAsync(Yemek yemek);
        Task UpdateAsync(Yemek yemek);
        Task DeleteAsync(int id);
    }
}
