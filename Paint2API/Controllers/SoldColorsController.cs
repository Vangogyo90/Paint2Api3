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
    public class SoldColorsController : ControllerBase
    {
        private readonly PaintContext _context;

        public SoldColorsController(PaintContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "2")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SoldColor>>> GetSoldColors()
        {
          if (_context.SoldColors == null)
          {
              return NotFound();
          }
            return await _context.SoldColors.ToListAsync();
        }

        [Authorize(Roles = "2")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SoldColor>> GetSoldColor(int? id)
        {
          if (_context.SoldColors == null)
          {
              return NotFound();
          }
            var soldColor = await _context.SoldColors.FindAsync(id);

            if (soldColor == null)
            {
                return NotFound();
            }

            return soldColor;
        }

        [Authorize(Roles = "2")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSoldColor(int? id, SoldColor soldColor)
        {
            if (id != soldColor.ID_Sold_Color)
            {
                return BadRequest();
            }

            _context.Entry(soldColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SoldColorExists(id))
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

        [Authorize(Roles = "2")]
        [HttpPost]
        public async Task<ActionResult<SoldColor>> PostSoldColor(SoldColor soldColor)
        {
          if (_context.SoldColors == null)
          {
              return Problem("Entity set 'PaintContext.SoldColors'  is null.");
          }
            _context.SoldColors.Add(soldColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSoldColor", new { id = soldColor.ID_Sold_Color }, soldColor);
        }

        [Authorize(Roles = "2")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSoldColor(int? id)
        {
            if (_context.SoldColors == null)
            {
                return NotFound();
            }
            var soldColor = await _context.SoldColors.FindAsync(id);
            if (soldColor == null)
            {
                return NotFound();
            }

            _context.SoldColors.Remove(soldColor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SoldColorExists(int? id)
        {
            return (_context.SoldColors?.Any(e => e.ID_Sold_Color == id)).GetValueOrDefault();
        }
    }
}
