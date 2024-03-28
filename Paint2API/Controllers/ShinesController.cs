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
    public class ShinesController : ControllerBase
    {
        private readonly PaintContext _context;

        public ShinesController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/Shines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shine>>> GetShines()
        {
          if (_context.Shines == null)
          {
              return NotFound();
          }
            return await _context.Shines.ToListAsync();
        }

        // GET: api/Shines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shine>> GetShine(int? id)
        {
          if (_context.Shines == null)
          {
              return NotFound();
          }
            var shine = await _context.Shines.FindAsync(id);

            if (shine == null)
            {
                return NotFound();
            }

            return shine;
        }

        // PUT: api/Shines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShine(int? id, Shine shine)
        {
            if (id != shine.IdShine)
            {
                return BadRequest();
            }

            _context.Entry(shine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShineExists(id))
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

        // POST: api/Shines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<Shine>> PostShine(Shine shine)
        {
          if (_context.Shines == null)
          {
              return Problem("Entity set 'PaintContext.Shines'  is null.");
          }
            _context.Shines.Add(shine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShine", new { id = shine.IdShine }, shine);
        }

        // DELETE: api/Shines/5
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShine(int? id)
        {
            if (_context.Shines == null)
            {
                return NotFound();
            }
            var shine = await _context.Shines.FindAsync(id);
            if (shine == null)
            {
                return NotFound();
            }

            _context.Shines.Remove(shine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShineExists(int? id)
        {
            return (_context.Shines?.Any(e => e.IdShine == id)).GetValueOrDefault();
        }
    }
}
