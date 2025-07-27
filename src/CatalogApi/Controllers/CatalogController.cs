using AspireLearning.Contracts.Models;
using AspireLearning.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspireLearning.CatalogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogRepository _repo;

        public CatalogController(IOptions<MongoDbSettings> settings)
        {
            _repo = new CatalogRepository(settings.Value);
        }

        [HttpGet]
        public async Task<IEnumerable<CatalogItem>> Get() => await _repo.GetAllAsync();

        [HttpGet("random/{count}")]
        public async Task<IEnumerable<CatalogItem>> GetRandom(int count) => await _repo.GetRandomAsync(count);

        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogItem>> GetById(string id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CatalogItem item)
        {
            await _repo.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, CatalogItem item)
        {
            await _repo.UpdateAsync(id, item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
} 