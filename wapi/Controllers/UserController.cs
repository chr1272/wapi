using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wapi.Models;
using System.Web.Http.Cors;



namespace wapi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly WapiDBContext _context;


        public UserController(WapiDBContext context)
        {
            _context = context;

            if (_context.User.Count() == 0)
            {
                _context.User.Add(new User { Fname = "Christophe", Pname = "Maille" });
                _context.SaveChanges();
            }
        }

        // GET: api/Kbarticle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetComments()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Kbarticle/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Kbarticle
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User item)
        {
            _context.User.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = item.Id }, item);
        }

        // PUT: api/Kbarticle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User item)
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
        public async Task<IActionResult> DeleteUser(int id)
        {
            var useritem = await _context.User.FindAsync(id);

            if (useritem == null)
            {
                return NotFound();
            }

            _context.User.Remove(useritem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
