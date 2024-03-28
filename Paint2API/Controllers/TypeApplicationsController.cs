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
    public class TypeApplicationsController : ControllerBase
    {
        private readonly PaintContext _context;

        public TypeApplicationsController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/TypeApplications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeApplication>>> GetTypeApplications()
        {
          if (_context.TypeApplications == null)
          {
              return NotFound();
          }
            return await _context.TypeApplications.ToListAsync();
        }

        // GET: api/TypeApplications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeApplication>> GetTypeApplication(int? id)
        {
          if (_context.TypeApplications == null)
          {
              return NotFound();
          }
            var typeApplication = await _context.TypeApplications.FindAsync(id);

            if (typeApplication == null)
            {
                return NotFound();
            }

            return typeApplication;
        }

        // PUT: api/TypeApplications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeApplication(int? id, TypeApplication typeApplication)
        {
            if (id != typeApplication.IdTypeApplication)
            {
                return BadRequest();
            }

            _context.Entry(typeApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeApplicationExists(id))
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

        // POST: api/TypeApplications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<TypeApplication>> PostTypeApplication(TypeApplication typeApplication)
        {
          if (_context.TypeApplications == null)
          {
              return Problem("Entity set 'PaintContext.TypeApplications'  is null.");
          }
            _context.TypeApplications.Add(typeApplication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeApplication", new { id = typeApplication.IdTypeApplication }, typeApplication);
        }

        // DELETE: api/TypeApplications/5
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeApplication(int? id)
        {
            if (_context.TypeApplications == null)
            {
                return NotFound();
            }
            var typeApplication = await _context.TypeApplications.FindAsync(id);
            if (typeApplication == null)
            {
                return NotFound();
            }

            _context.TypeApplications.Remove(typeApplication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeApplicationExists(int? id)
        {
            return (_context.TypeApplications?.Any(e => e.IdTypeApplication == id)).GetValueOrDefault();
        }
    }
}
