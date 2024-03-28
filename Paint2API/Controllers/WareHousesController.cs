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
    public class WareHousesController : ControllerBase
    {
        private readonly PaintContext _context;

        public WareHousesController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/WareHouses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WareHouse>>> GetWareHouses()
        {
          if (_context.WareHouses == null)
          {
              return NotFound();
          }
            return await _context.WareHouses.ToListAsync();
        }

        // GET: api/WareHouses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WareHouse>> GetWareHouse(int? id)
        {
          if (_context.WareHouses == null)
          {
              return NotFound();
          }
            var wareHouse = await _context.WareHouses.FindAsync(id);

            if (wareHouse == null)
            {
                return NotFound();
            }

            return wareHouse;
        }

        // PUT: api/WareHouses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWareHouse(int? id, WareHouse wareHouse)
        {
            if (id != wareHouse.IdWareHouse)
            {
                return BadRequest();
            }

            _context.Entry(wareHouse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WareHouseExists(id))
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

        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            if (_context.WareHouses == null)
            {
                return NotFound();
            }

            List<WareHouse> data = _context.WareHouses.Include(a => a.City).ToList();

            return Ok(data);
        }

        // POST: api/WareHouses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<WareHouse>> PostWareHouse(WareHouse wareHouse)
        {
          if (_context.WareHouses == null)
          {
              return Problem("Entity set 'PaintContext.WareHouses'  is null.");
          }
            _context.WareHouses.Add(wareHouse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWareHouse", new { id = wareHouse.IdWareHouse }, wareHouse);
        }

        // DELETE: api/WareHouses/5
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWareHouse(int? id)
        {
            if (_context.WareHouses == null)
            {
                return NotFound();
            }
            var wareHouse = await _context.WareHouses.FindAsync(id);
            if (wareHouse == null)
            {
                return NotFound();
            }

            _context.WareHouses.Remove(wareHouse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WareHouseExists(int? id)
        {
            return (_context.WareHouses?.Any(e => e.IdWareHouse == id)).GetValueOrDefault();
        }
    }
}
