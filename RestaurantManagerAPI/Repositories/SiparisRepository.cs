using Dapper;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;
using System.Data;

namespace RestaurantManagerAPI.Repositories
{
    public class SiparisRepository : ISiparisRepository
    {
        private readonly DapperContext _context;

        public SiparisRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Siparis>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Siparis>(
                "sp_Siparis_GetAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<Siparis?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Siparis>(
                "sp_Siparis_GetById", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Siparis>> SearchAsync(string term)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Siparis>(
                "sp_Siparis_Search", new { Term = term }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(Siparis siparis)
        {
            using var connection = _context.CreateConnection();
            var newId = await connection.ExecuteScalarAsync<int>(
                "sp_Siparis_Insert",
                new { siparis.RestoranId, siparis.YemekId, siparis.MusteriAdi, siparis.Adet, siparis.Durum },
                commandType: CommandType.StoredProcedure);
            return newId;
        }

        public async Task UpdateAsync(Siparis siparis)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_Siparis_Update",
                new { siparis.Id, siparis.RestoranId, siparis.YemekId, siparis.MusteriAdi, siparis.Adet, siparis.Durum },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_Siparis_Delete", new { Id = id }, commandType: CommandType.StoredProcedure);
        }
    }
}
