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
    public class ColorsController : ControllerBase
    {
        private readonly PaintContext _context;
        private readonly ILogger<ColorsController> _logger;

        public ColorsController(PaintContext context, ILogger<ColorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Colors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors()
        {
            try
            {
                if (_context.Colors == null)
                {
                    _logger.LogWarning("Color was null");
                    return NotFound();
                }
                _logger.LogInformation("Color was GetColors");
                return await _context.Colors.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Color was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(int? id)
        {
            try
            {
                if (_context.Colors == null)
                {
                    _logger.LogWarning("Color was null");
                    return NotFound();
                }
                var color = await _context.Colors.FindAsync(id);

                if (color == null)
                {
                    _logger.LogWarning("Color was null");
                    return NotFound();
                }
                _logger.LogInformation("Color was GetColor");
                return color;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Color was critical error {ex}");
                return Problem("Critical error");
            }
        }

        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            try
            {
                if (_context.Colors == null)
                {
                    _logger.LogWarning("Color was null");
                    return NotFound();
                }

                List<Color> data = _context.Colors
                    .Include(a => a.TypeApplications)
                    .Include(a => a.TempPulverizations)
                    .Include(a => a.Shines)
                    .Include(a => a.TypeSurfaces)
                    .Include(a => a.RalCatalog)
                    .ToList();
                _logger.LogInformation("Color was GetData");
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Color was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // PUT: api/Colors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "3")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(int? id, Color color)
        {
            try
            {
                if (id != color.IdColor)
                {
                    _logger.LogWarning("Color was null");
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    _context.Entry(color).State = EntityState.Modified;
                    _logger.LogInformation("Color was PutColor");
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"Color is not valid");
                    return Problem("Color is not valid");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Color was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // POST: api/Colors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "3")]
        [HttpPost]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {
            try
            {
                if (_context.Colors == null)
                {
                    _logger.LogWarning("Color was null");
                    return Problem("Entity set 'PaintContext.Colors'  is null.");
                }

                if (ModelState.IsValid)
                {
                    _context.Colors.Add(color);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Color was PostColor");
                    return CreatedAtAction("GetColor", new { id = color.IdColor }, color);
                }
                else
                {
                    _logger.LogWarning($"Color is not valid");
                    return Problem("Color is not valid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Color was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // DELETE: api/Colors/5
        [Authorize(Roles = "3")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int? id)
        {
            try
            {
                if (_context.Colors == null)
                {
                    _logger.LogWarning("Color was null");
                    return NotFound();
                }
                var color = await _context.Colors.FindAsync(id);
                if (color == null)
                {
                    return NotFound();
                }

                _context.Colors.Remove(color);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Color was DeleteColor");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Color was critical error {ex}");
                return Problem("Critical error");
            }
        }

        private bool ColorExists(int? id)
        {
            return (_context.Colors?.Any(e => e.IdColor == id)).GetValueOrDefault();
        }
    }
}
