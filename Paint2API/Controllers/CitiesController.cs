using System;
using System.Collections.Generic;
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
    public class CitiesController : ControllerBase
    {
        private readonly PaintContext _context;
        private readonly ILogger<CitiesController> _logger;

        public CitiesController(PaintContext context, ILogger<CitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            try
            {
                if (_context.Cities == null)
                {
                    _logger.LogWarning("City was null");
                    return NotFound();
                }
                _logger.LogInformation("Cities was GetCities");
                return await _context.Cities.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"City was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int? id)
        {
            try
            {
                if (_context.Cities == null)
                {
                    _logger.LogWarning("City was null");
                    return NotFound();
                }
                var city = await _context.Cities.FindAsync(id);

                if (city == null)
                {
                    return NotFound();
                }
                _logger.LogInformation("Cities was GetCity");
                return city;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"City was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int? id, City city)
        {
            try
            {
                if (id != city.IdCity)
                {
                    _logger.LogWarning("City was null");
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    _context.Entry(city).State = EntityState.Modified;
                    _logger.LogInformation("Cities was PutCity");
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"Cities is not valid");
                    return Problem("Cities is not valid");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"City was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            try
            {
                if (_context.Cities == null)
                {
                    _logger.LogWarning("City was null");
                    return Problem("Entity set 'PaintContext.Cities'  is null.");
                }

                if (ModelState.IsValid)
                {
                    _context.Cities.Add(city);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Cities was PostCity");
                    return CreatedAtAction("GetCity", new { id = city.IdCity }, city);
                }
                else
                {
                    _logger.LogWarning($"Cities is not valid");
                    return Problem("Cities is not valid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"City was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // DELETE: api/Cities/5
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int? id)
        {
            try
            {
                if (_context.Cities == null)
                {
                    _logger.LogWarning("City was null");
                    return NotFound();
                }
                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    _logger.LogWarning("City was null");
                    return NotFound();
                }

                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cities was DeleteCity");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"City was critical error {ex}");
                return Problem("Critical error");
            }
        }

        private bool CityExists(int? id)
        {
            return (_context.Cities?.Any(e => e.IdCity == id)).GetValueOrDefault();
        }
    }
}
