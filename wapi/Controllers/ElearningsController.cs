using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wapi.Models;
using System.Web.Http.Cors;

namespace wapi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [Route("api/elearning")]
    [ApiController]
    public class ElearningsController : ControllerBase
    {
        private readonly WapiDBContext _context;

        public ElearningsController(WapiDBContext context)
        {
            _context = context;
        }

        // GET: api/Elearnings
        [HttpGet]
        public IEnumerable<Elearning> GetElearning()
        {
            return _context.Elearning;
        }

        // GET: api/Elearnings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetElearning([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var elearning = await _context.Elearning.FindAsync(id);

            if (elearning == null)
            {
                return NotFound();
            }

            return Ok(elearning);
        }

        // PUT: api/Elearnings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElearning([FromRoute] int id, [FromBody] Elearning elearning)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != elearning.Id)
            {
                return BadRequest();
            }

            _context.Entry(elearning).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElearningExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Elearnings
        [HttpPost]
        public async Task<IActionResult> PostElearning([FromBody] Elearning elearning)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Elearning.Add(elearning);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetElearning", new { id = elearning.Id }, elearning);
        }

        // DELETE: api/Elearnings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElearning([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var elearning = await _context.Elearning.FindAsync(id);
            if (elearning == null)
            {
                return NotFound();
            }

            _context.Elearning.Remove(elearning);
            await _context.SaveChangesAsync();

            return Ok(elearning);
        }

        private bool ElearningExists(int id)
        {
            return _context.Elearning.Any(e => e.Id == id);
        }
    }
}