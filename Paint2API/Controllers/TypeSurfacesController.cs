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
    public class TypeSurfacesController : ControllerBase
    {
        private readonly PaintContext _context;

        public TypeSurfacesController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/TypeSurfaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeSurface>>> GetTypeSurfaces()
        {
          if (_context.TypeSurfaces == null)
          {
              return NotFound();
          }
            return await _context.TypeSurfaces.ToListAsync();
        }

        // GET: api/TypeSurfaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeSurface>> GetTypeSurface(int? id)
        {
          if (_context.TypeSurfaces == null)
          {
              return NotFound();
          }
            var typeSurface = await _context.TypeSurfaces.FindAsync(id);

            if (typeSurface == null)
            {
                return NotFound();
            }

            return typeSurface;
        }

        // PUT: api/TypeSurfaces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeSurface(int? id, TypeSurface typeSurface)
        {
            if (id != typeSurface.IdTypeSurface)
            {
                return BadRequest();
            }

            _context.Entry(typeSurface).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeSurfaceExists(id))
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

        // POST: api/TypeSurfaces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<TypeSurface>> PostTypeSurface(TypeSurface typeSurface)
        {
          if (_context.TypeSurfaces == null)
          {
              return Problem("Entity set 'PaintContext.TypeSurfaces'  is null.");
          }
            _context.TypeSurfaces.Add(typeSurface);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeSurface", new { id = typeSurface.IdTypeSurface }, typeSurface);
        }

        // DELETE: api/TypeSurfaces/5
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeSurface(int? id)
        {
            if (_context.TypeSurfaces == null)
            {
                return NotFound();
            }
            var typeSurface = await _context.TypeSurfaces.FindAsync(id);
            if (typeSurface == null)
            {
                return NotFound();
            }

            _context.TypeSurfaces.Remove(typeSurface);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeSurfaceExists(int? id)
        {
            return (_context.TypeSurfaces?.Any(e => e.IdTypeSurface == id)).GetValueOrDefault();
        }
    }
}
