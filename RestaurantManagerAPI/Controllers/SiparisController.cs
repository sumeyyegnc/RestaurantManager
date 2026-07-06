using Microsoft.AspNetCore.Mvc;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Repositories;

namespace RestaurantManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiparisController : ControllerBase
    {
        private readonly ISiparisRepository _repository;

        public SiparisController(ISiparisRepository repository)
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
            var siparis = await _repository.GetByIdAsync(id);
            if (siparis == null) return NotFound();
            return Ok(siparis);
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
        public async Task<IActionResult> Create([FromBody] Siparis siparis)
        {
            var newId = await _repository.InsertAsync(siparis);
            siparis.Id = newId;
            return CreatedAtAction(nameof(GetById), new { id = newId }, siparis);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Siparis siparis)
        {
            if (id != siparis.Id) return BadRequest("Id uyusmuyor.");
            var mevcut = await _repository.GetByIdAsync(id);
            if (mevcut == null) return NotFound();

            await _repository.UpdateAsync(siparis);
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
