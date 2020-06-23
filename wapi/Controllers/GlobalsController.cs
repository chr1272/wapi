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
    [Route("api/globals")]
    [ApiController]
    public class GlobalsController : ControllerBase
    {
        private readonly WapiDBContext _context;

        public GlobalsController(WapiDBContext context)
        {
            _context = context;
        }

        // GET: api/Globals
        [HttpGet]
        public IEnumerable<Globals> GetGlobals()
        {
            return _context.Globals;
        }

        // GET: api/Globals/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGlobals([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var globals = await _context.Globals.FindAsync(id);

            if (globals == null)
            {
                return NotFound();
            }

            return Ok(globals);
        }

        // PUT: api/Globals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGlobals([FromRoute] int id, [FromBody] Globals globals)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != globals.Id)
            {
                return BadRequest();
            }

            _context.Entry(globals).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GlobalsExists(id))
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

        // POST: api/Globals
        [HttpPost]
        public async Task<IActionResult> PostGlobals([FromBody] Globals globals)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Globals.Add(globals);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGlobals", new { id = globals.Id }, globals);
        }

        // DELETE: api/Globals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGlobals([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var globals = await _context.Globals.FindAsync(id);
            if (globals == null)
            {
                return NotFound();
            }

            _context.Globals.Remove(globals);
            await _context.SaveChangesAsync();

            return Ok(globals);
        }

        private bool GlobalsExists(int id)
        {
            return _context.Globals.Any(e => e.Id == id);
        }
    }
}