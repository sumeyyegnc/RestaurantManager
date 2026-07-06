using Dapper;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;
using System.Data;

namespace RestaurantManagerAPI.Repositories
{
    public class YemekRepository : IYemekRepository
    {
        private readonly DapperContext _context;

        public YemekRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Yemek>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Yemek>(
                "sp_Yemek_GetAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<Yemek?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Yemek>(
                "sp_Yemek_GetById", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Yemek>> SearchAsync(string term)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Yemek>(
                "sp_Yemek_Search", new { Term = term }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(Yemek yemek)
        {
            using var connection = _context.CreateConnection();
            var newId = await connection.ExecuteScalarAsync<int>(
                "sp_Yemek_Insert",
                new { yemek.RestoranId, yemek.Ad, yemek.Kategori, yemek.Fiyat },
                commandType: CommandType.StoredProcedure);
            return newId;
        }

        public async Task UpdateAsync(Yemek yemek)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_Yemek_Update",
                new { yemek.Id, yemek.RestoranId, yemek.Ad, yemek.Kategori, yemek.Fiyat },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_Yemek_Delete", new { Id = id }, commandType: CommandType.StoredProcedure);
        }
    }
}
