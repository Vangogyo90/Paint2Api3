using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paint2API.Models;

namespace Paint2API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBacksController : ControllerBase
    {
        private readonly PaintContext _context;

        public FeedBacksController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/FeedBacks
        [Authorize(Roles = "4")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedBack>>> GetFeedBacks()
        {
          if (_context.FeedBacks == null)
          {
              return NotFound();
          }
            return await _context.FeedBacks.ToListAsync();
        }

        [Authorize(Roles = "4")]
        [HttpGet("GetFeedBacksByCategory/{category}")]
        public async Task<ActionResult<IEnumerable<FeedBack>>> GetFeedBacksByCategory(string category)
        {
            if (_context.FeedBacks == null)
            {
                return NotFound();
            }

            if (category.Equals("All"))
                return await _context.FeedBacks.ToListAsync();
            else
                return await _context.FeedBacks.Where(a => a.Category.Equals(category)).ToListAsync();
        }

        // GET: api/FeedBacks/5
        [Authorize(Roles = "4")]
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedBack>> GetFeedBack(int? id)
        {
          if (_context.FeedBacks == null)
          {
              return NotFound();
          }
            var feedBack = await _context.FeedBacks.FindAsync(id);

            if (feedBack == null)
            {
                return NotFound();
            }

            return feedBack;
        }

        // PUT: api/FeedBacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "4")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedBack(int? id, FeedBack feedBack)
        {
            if (id != feedBack.IdFeedBack)
            {
                return BadRequest();
            }

            _context.Entry(feedBack).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedBackExists(id))
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

        // POST: api/FeedBacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FeedBack>> PostFeedBack(FeedBack feedBack)
        {
          if (_context.FeedBacks == null)
          {
              return Problem("Entity set 'PaintContext.FeedBacks'  is null.");
          }

            feedBack.End = false;
            _context.FeedBacks.Add(feedBack);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeedBack", new { id = feedBack.IdFeedBack }, feedBack);
        }

        // DELETE: api/FeedBacks/5
        [Authorize(Roles = "4")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedBack(int? id)
        {
            if (_context.FeedBacks == null)
            {
                return NotFound();
            }
            var feedBack = await _context.FeedBacks.FindAsync(id);
            if (feedBack == null)
            {
                return NotFound();
            }

            _context.FeedBacks.Remove(feedBack);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeedBackExists(int? id)
        {
            return (_context.FeedBacks?.Any(e => e.IdFeedBack == id)).GetValueOrDefault();
        }
    }
}
