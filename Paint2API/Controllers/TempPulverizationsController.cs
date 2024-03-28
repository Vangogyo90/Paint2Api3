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
    public class TempPulverizationsController : ControllerBase
    {
        private readonly PaintContext _context;

        public TempPulverizationsController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/TempPulverizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TempPulverization>>> GetTempPulverizations()
        {
          if (_context.TempPulverizations == null)
          {
              return NotFound();
          }
            return await _context.TempPulverizations.ToListAsync();
        }

        // GET: api/TempPulverizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TempPulverization>> GetTempPulverization(int? id)
        {
          if (_context.TempPulverizations == null)
          {
              return NotFound();
          }
            var tempPulverization = await _context.TempPulverizations.FindAsync(id);

            if (tempPulverization == null)
            {
                return NotFound();
            }

            return tempPulverization;
        }

        // PUT: api/TempPulverizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTempPulverization(int? id, TempPulverization tempPulverization)
        {
            if (id != tempPulverization.IdTempPulverization)
            {
                return BadRequest();
            }

            _context.Entry(tempPulverization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TempPulverizationExists(id))
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

        // POST: api/TempPulverizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<TempPulverization>> PostTempPulverization(TempPulverization tempPulverization)
        {
          if (_context.TempPulverizations == null)
          {
              return Problem("Entity set 'PaintContext.TempPulverizations'  is null.");
          }
            _context.TempPulverizations.Add(tempPulverization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTempPulverization", new { id = tempPulverization.IdTempPulverization }, tempPulverization);
        }

        // DELETE: api/TempPulverizations/5
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTempPulverization(int? id)
        {
            if (_context.TempPulverizations == null)
            {
                return NotFound();
            }
            var tempPulverization = await _context.TempPulverizations.FindAsync(id);
            if (tempPulverization == null)
            {
                return NotFound();
            }

            _context.TempPulverizations.Remove(tempPulverization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TempPulverizationExists(int? id)
        {
            return (_context.TempPulverizations?.Any(e => e.IdTempPulverization == id)).GetValueOrDefault();
        }
    }
}
