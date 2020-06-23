using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wapi.Models;
using System.Web.Http.Cors;



namespace wapi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods:"*")]
    [Route("api/comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly WapiDBContext _context;

        public CommentsController(WapiDBContext context)
        {
            _context = context;

            if (_context.Comment.Count() == 0)
            {
                _context.Comment.Add(new Comment { Title = "New Comment", Content = "" });
                _context.Comment.Add(new Comment { Title = "Another New Comment", Content = "" });
                _context.SaveChanges();
            }
        }

        // GET: api/Kbarticle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comment.ToListAsync();
        }

        // GET: api/Kbarticle/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comment.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // POST: api/Kbarticle
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment item)
        {
            _context.Comment.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = item.Id }, item);
        }

        // PUT: api/Kbarticle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment item)
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
        public async Task<IActionResult> DeleteComment(int id)
        {
            var commentitem = await _context.Comment.FindAsync(id);

            if (commentitem == null)
            {
                return NotFound();
            }

            _context.Comment.Remove(commentitem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
