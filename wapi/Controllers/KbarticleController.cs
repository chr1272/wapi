using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wapi.Models;



namespace wapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KbarticleController : ControllerBase
    {
        private readonly WapiDBContext _context;

        public KbarticleController(WapiDBContext context)
        {
            _context = context;

            if (_context.Kbarticle.Count() == 0)
            {
                _context.Kbarticle.Add(new Kbarticle { Name = "New Article", Content = "" });
                _context.SaveChanges();
            }
        }

        // GET: api/Kbarticle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kbarticle>>> GetKbarticles()
        {
            return await _context.Kbarticle.ToListAsync();
        }

        // GET: api/Kbarticle/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kbarticle>> GetKbarticle(long id)
        {
            var kbarticle = await _context.Kbarticle.FindAsync(id);

            if (kbarticle == null)
            {
                return NotFound();
            }

            return kbarticle;
        }

        // POST: api/Kbarticle
        [HttpPost]
        public async Task<ActionResult<Kbarticle>> PostTodoItem(Kbarticle item)
        {
            _context.Kbarticle.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetKbarticle), new { id = item.Id }, item);
        }

        // PUT: api/Kbarticle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKbarticle(long id, Kbarticle item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKbarticle(long id)
        {
            var kbitem = await _context.Kbarticle.FindAsync(id);

            if (kbitem == null)
            {
                return NotFound();
            }

            _context.Kbarticle.Remove(kbitem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
