using Dapper;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Models;
using System.Data;

namespace RestaurantManagerAPI.Repositories
{
    public class RestoranRepository : IRestoranRepository
    {
        private readonly DapperContext _context;

        public RestoranRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restoran>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Restoran>(
                "sp_Restoran_GetAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<Restoran?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Restoran>(
                "sp_Restoran_GetById", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Restoran>> SearchAsync(string term)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Restoran>(
                "sp_Restoran_Search", new { Term = term }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(Restoran restoran)
        {
            using var connection = _context.CreateConnection();
            var newId = await connection.ExecuteScalarAsync<int>(
                "sp_Restoran_Insert",
                new { restoran.Ad, restoran.Adres, restoran.Telefon, restoran.Sehir },
                commandType: CommandType.StoredProcedure);
            return newId;
        }

        public async Task UpdateAsync(Restoran restoran)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_Restoran_Update",
                new { restoran.Id, restoran.Ad, restoran.Adres, restoran.Telefon, restoran.Sehir },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_Restoran_Delete", new { Id = id }, commandType: CommandType.StoredProcedure);
        }
    }
}
