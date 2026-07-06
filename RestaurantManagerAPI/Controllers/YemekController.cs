using Microsoft.AspNetCore.Mvc;
using RestaurantManagerAPI.Models;
using RestaurantManagerAPI.Repositories;

namespace RestaurantManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YemekController : ControllerBase
    {
        private readonly IYemekRepository _repository;

        public YemekController(IYemekRepository repository)
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
            var yemek = await _repository.GetByIdAsync(id);
            if (yemek == null) return NotFound();
            return Ok(yemek);
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
        public async Task<IActionResult> Create([FromBody] Yemek yemek)
        {
            var newId = await _repository.InsertAsync(yemek);
            yemek.Id = newId;
            return CreatedAtAction(nameof(GetById), new { id = newId }, yemek);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Yemek yemek)
        {
            if (id != yemek.Id) return BadRequest("Id uyusmuyor.");
            var mevcut = await _repository.GetByIdAsync(id);
            if (mevcut == null) return NotFound();

            await _repository.UpdateAsync(yemek);
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
