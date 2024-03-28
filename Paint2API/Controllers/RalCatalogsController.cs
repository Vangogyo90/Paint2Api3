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
    public class RalCatalogsController : ControllerBase
    {
        private readonly PaintContext _context;

        public RalCatalogsController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/RalCatalogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RalCatalog>>> GetRalCatalogs()
        {
          if (_context.RalCatalogs == null)
          {
              return NotFound();
          }
            return await _context.RalCatalogs.ToListAsync();
        }

        // GET: api/RalCatalogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RalCatalog>> GetRalCatalog(int? id)
        {
          if (_context.RalCatalogs == null)
          {
              return NotFound();
          }
            var ralCatalog = await _context.RalCatalogs.FindAsync(id);

            if (ralCatalog == null)
            {
                return NotFound();
            }

            return ralCatalog;
        }

        // PUT: api/RalCatalogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRalCatalog(int? id, RalCatalog ralCatalog)
        {
            if (id != ralCatalog.IdRalCatalog)
            {
                return BadRequest();
            }

            _context.Entry(ralCatalog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RalCatalogExists(id))
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

        // POST: api/RalCatalogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<RalCatalog>> PostRalCatalog(RalCatalog ralCatalog)
        {
          if (_context.RalCatalogs == null)
          {
              return Problem("Entity set 'PaintContext.RalCatalogs'  is null.");
          }
            _context.RalCatalogs.Add(ralCatalog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRalCatalog", new { id = ralCatalog.IdRalCatalog }, ralCatalog);
        }

        // DELETE: api/RalCatalogs/5
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRalCatalog(int? id)
        {
            if (_context.RalCatalogs == null)
            {
                return NotFound();
            }
            var ralCatalog = await _context.RalCatalogs.FindAsync(id);
            if (ralCatalog == null)
            {
                return NotFound();
            }

            _context.RalCatalogs.Remove(ralCatalog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RalCatalogExists(int? id)
        {
            return (_context.RalCatalogs?.Any(e => e.IdRalCatalog == id)).GetValueOrDefault();
        }
    }
}
