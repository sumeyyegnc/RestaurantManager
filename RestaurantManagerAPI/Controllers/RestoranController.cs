using Microsoft.AspNetCore.Mvc;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Repositories;

namespace RestaurantManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestoranController : ControllerBase
    {
        private readonly IRestoranRepository _repository;

        public RestoranController(IRestoranRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var restoran = await _repository.GetByIdAsync(id);
            if (restoran == null) return NotFound();
            return Ok(restoran);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Ok(await _repository.GetAllAsync());

            var result = await _repository.SearchAsync(term);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Restoran restoran)
        {
            var newId = await _repository.InsertAsync(restoran);
            restoran.Id = newId;
            return CreatedAtAction(nameof(GetById), new { id = newId }, restoran);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Restoran restoran)
        {
            if (id != restoran.Id) return BadRequest("Id uyusmuyor.");
            var mevcut = await _repository.GetByIdAsync(id);
            if (mevcut == null) return NotFound();

            await _repository.UpdateAsync(restoran);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mevcut = await _repository.GetByIdAsync(id);
            if (mevcut == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
