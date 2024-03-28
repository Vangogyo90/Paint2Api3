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
    public class StatusDeliveriesController : ControllerBase
    {
        private readonly PaintContext _context;

        public StatusDeliveriesController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/StatusDeliveries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusDelivery>>> GetStatusDeliveries()
        {
          if (_context.StatusDeliveries == null)
          {
              return NotFound();
          }
            return await _context.StatusDeliveries.ToListAsync();
        }

        // GET: api/StatusDeliveries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusDelivery>> GetStatusDelivery(int? id)
        {
          if (_context.StatusDeliveries == null)
          {
              return NotFound();
          }
            var statusDelivery = await _context.StatusDeliveries.FindAsync(id);

            if (statusDelivery == null)
            {
                return NotFound();
            }

            return statusDelivery;
        }

        // PUT: api/StatusDeliveries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatusDelivery(int? id, StatusDelivery statusDelivery)
        {
            if (id != statusDelivery.IdStatusOrder)
            {
                return BadRequest();
            }

            _context.Entry(statusDelivery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusDeliveryExists(id))
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

        // POST: api/StatusDeliveries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<StatusDelivery>> PostStatusDelivery(StatusDelivery statusDelivery)
        {
          if (_context.StatusDeliveries == null)
          {
              return Problem("Entity set 'PaintContext.StatusDeliveries'  is null.");
          }
            _context.StatusDeliveries.Add(statusDelivery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatusDelivery", new { id = statusDelivery.IdStatusOrder }, statusDelivery);
        }

        // DELETE: api/StatusDeliveries/5
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatusDelivery(int? id)
        {
            if (_context.StatusDeliveries == null)
            {
                return NotFound();
            }
            var statusDelivery = await _context.StatusDeliveries.FindAsync(id);
            if (statusDelivery == null)
            {
                return NotFound();
            }

            _context.StatusDeliveries.Remove(statusDelivery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusDeliveryExists(int? id)
        {
            return (_context.StatusDeliveries?.Any(e => e.IdStatusOrder == id)).GetValueOrDefault();
        }
    }
}
