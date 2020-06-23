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
    [Route("api/whatsthat")]
    [ApiController]
    public class WhatsthatsController : ControllerBase
    {
        private readonly WapiDBContext _context;

        public WhatsthatsController(WapiDBContext context)
        {
            _context = context;
        }

        // GET: api/Whatsthats
        [HttpGet]
        public IEnumerable<Whatsthat> GetWhatsthat()
        {
            return _context.Whatsthat;
        }

        // GET: api/Whatsthats/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWhatsthat([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var whatsthat = await _context.Whatsthat.FindAsync(id);

            if (whatsthat == null)
            {
                return NotFound();
            }

            return Ok(whatsthat);
        }

        // PUT: api/Whatsthats/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWhatsthat([FromRoute] int id, [FromBody] Whatsthat whatsthat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != whatsthat.Id)
            {
                return BadRequest();
            }

            _context.Entry(whatsthat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WhatsthatExists(id))
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

        // POST: api/Whatsthats
        [HttpPost]
        public async Task<IActionResult> PostWhatsthat([FromBody] Whatsthat whatsthat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Whatsthat.Add(whatsthat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWhatsthat", new { id = whatsthat.Id }, whatsthat);
        }

        // DELETE: api/Whatsthats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWhatsthat([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var whatsthat = await _context.Whatsthat.FindAsync(id);
            if (whatsthat == null)
            {
                return NotFound();
            }

            _context.Whatsthat.Remove(whatsthat);
            await _context.SaveChangesAsync();

            return Ok(whatsthat);
        }

        private bool WhatsthatExists(int id)
        {
            return _context.Whatsthat.Any(e => e.Id == id);
        }
    }
}