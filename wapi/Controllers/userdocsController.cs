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
    [Route("api/userdoc")]
    [ApiController]
    public class userdocsController : ControllerBase
    {
        private readonly WapiDBContext _context;

        public userdocsController(WapiDBContext context)
        {
            _context = context;
        }

        // GET: api/userdocs
        [HttpGet]
        public IEnumerable<userdoc> Getuserdoc()
        {
            return _context.userdoc;
        }

        // GET: api/userdocs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Getuserdoc([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userdoc = await _context.userdoc.FindAsync(id);

            if (userdoc == null)
            {
                return NotFound();
            }

            return Ok(userdoc);
        }

        // PUT: api/userdocs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putuserdoc([FromRoute] int id, [FromBody] userdoc userdoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userdoc.Id)
            {
                return BadRequest();
            }

            _context.Entry(userdoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userdocExists(id))
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

        // POST: api/userdocs
        [HttpPost]
        public async Task<IActionResult> Postuserdoc([FromBody] userdoc userdoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.userdoc.Add(userdoc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getuserdoc", new { id = userdoc.Id }, userdoc);
        }

        // DELETE: api/userdocs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteuserdoc([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userdoc = await _context.userdoc.FindAsync(id);
            if (userdoc == null)
            {
                return NotFound();
            }

            _context.userdoc.Remove(userdoc);
            await _context.SaveChangesAsync();

            return Ok(userdoc);
        }

        private bool userdocExists(int id)
        {
            return _context.userdoc.Any(e => e.Id == id);
        }
    }
}