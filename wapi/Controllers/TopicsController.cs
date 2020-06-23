using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wapi.Models;
using System.Web.Http.Cors;

namespace wapi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [Route("api/topic")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly WapiDBContext _context;

        public TopicsController(WapiDBContext context)
        {
            _context = context;
        }

        // GET: api/Topics
        [HttpGet]
        public IEnumerable<Topic> GetTopic()
        {
            return _context.Topic;
        }

        // GET: api/Topics/id/path/filename
        [HttpGet("{path}/{filename}")]
        public async Task<IActionResult> GetTopic([FromRoute]string path, string filename)
        {

            path = path.Replace("::", "/");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            //var topic = await _context.Topic.FindAsync(id);

            var topic = await _context.Topic.SingleOrDefaultAsync(m => m.Path == path && m.FileName == filename);


            /*
                        if (topic == null)
                        {
                            Topic t = new Topic();

                            Console.Write(t.CreateTopic(path, filename));

                            _context.Topic.Add(t.CreateTopic(path, filename));
                            await _context.SaveChangesAsync();

                            topic = t;

                            return CreatedAtAction("GetTopic", new { id = topic.Id }, topic);
                        }

                        return Ok(topic);
            */
            Topic t = new Topic();
            return Ok(t.CreateTopic(path, filename));
        }

        // PUT: api/Topics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopic([FromRoute] int id, [FromBody] Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topic.Id)
            {
                return BadRequest();
            }

            _context.Entry(topic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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

        // POST: api/Topics
        [HttpPost]
        public async Task<IActionResult> PostTopic([FromBody] Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Topic.Add(topic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTopic", new { id = topic.Id }, topic);
        }

        // DELETE: api/Topics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topic = await _context.Topic.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            _context.Topic.Remove(topic);
            await _context.SaveChangesAsync();

            return Ok(topic);
        }

        private bool TopicExists(int id)
        {
            return _context.Topic.Any(e => e.Id == id);
        }
    }
}