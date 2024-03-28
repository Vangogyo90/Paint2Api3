using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoPartsAPI.Hash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paint2API.Models;

namespace Paint2API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly PaintContext _context;

        public DiscountsController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/Discounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discount>>> GetDiscounts()
        {
          if (_context.Discounts == null)
          {
              return NotFound();
          }
            return await _context.Discounts.ToListAsync();
        }

        // GET: api/Discounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Discount>> GetDiscount(int? id)
        {
          if (_context.Discounts == null)
          {
              return NotFound();
          }
            var discount = await _context.Discounts.FindAsync(id);

            if (discount == null)
            {
                return NotFound();
            }

            return discount;
        }

        [HttpGet("GetDiscountByColor/{id}")]
        public async Task<ActionResult<Discount>> GetDiscountByColor(int? id)
        {
            if (_context.Discounts == null)
            {
                return NotFound();
            }
            var discount = await _context.Discounts
                .FirstOrDefaultAsync(a => a.ColorId == id);

            if (discount == null)
            {
                return NotFound();
            }

            return discount;
        }

        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            if (_context.Discounts == null)
            {
                return NotFound();
            }

            List<Discount> data = _context.Discounts
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .ToList();

            return Ok(data);
        }

        [Authorize(Roles = "3")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscount(int? id, Discount discount)
        {
            if (id != discount.IdDiscount)
            {
                return BadRequest();
            }

            _context.Entry(discount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscountExists(id))
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

        // POST: api/Discounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "3")]
        [HttpPost]
        public async Task<ActionResult<Discount>> PostDiscount(Discount discount)
        {
          if (_context.Discounts == null)
          {
              return Problem("Entity set 'PaintContext.Discounts'  is null.");
          }
            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDiscount", new { id = discount.IdDiscount }, discount);
        }

        // DELETE: api/Discounts/5
        [Authorize(Roles = "3")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int? id)
        {
            if (_context.Discounts == null)
            {
                return NotFound();
            }
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DiscountExists(int? id)
        {
            return (_context.Discounts?.Any(e => e.IdDiscount == id)).GetValueOrDefault();
        }
    }
}
