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
    public class ColorDeliveriesController : ControllerBase
    {
        private readonly PaintContext _context;
        private readonly ILogger<ColorDeliveriesController> _logger;

        public ColorDeliveriesController(PaintContext context, ILogger<ColorDeliveriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ColorDeliveries
        [Authorize(Roles = "2")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorDelivery>>> GetColorDeliveries()
        {
            try
            {
                if (_context.ColorDeliveries == null)
                {
                    _logger.LogWarning("ColorDelivery was null");
                    return NotFound();
                }
                _logger.LogInformation("ColorDelivery was GetColorDeliveries");
                return await _context.ColorDeliveries.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ColorDelivery was critical error {ex}");
                return Problem("Critical error");
            }
        }

        [HttpGet("GetData/{deliveryID}")]
        public IActionResult GetData(int deliveryID)
        {
            try
            {
                if (_context.ColorDeliveries == null)
                {
                    _logger.LogWarning("ColorDelivery was null");
                    return NotFound();
                }

                IQueryable<ColorDelivery> data = _context.ColorDeliveries
                    .Include(a => a.Colors!.TypeApplications)
                    .Include(a => a.Colors!.TempPulverizations)
                    .Include(a => a.Colors!.Shines)
                    .Include(a => a.Colors!.TypeSurfaces)
                    .Include(a => a.Colors!.RalCatalog);
                _logger.LogInformation("ColorDelivery was GetData");

                if (deliveryID > 0)
                {
                    data = data.Where(delivery => delivery.DeliveryId == deliveryID);
                }
                else
                {
                    _logger.LogWarning("ColorDelivery was null");
                    return NotFound();
                }

                var result = data.ToList()
        .GroupBy(a => a.ColorId)
        .Select(g => new ColorDelivery
        {
            ColorId = g.Key,
            Quantity = g.Count(),
            Colors = g.First().Colors,
            IdColorDelivery = g.First().IdColorDelivery,
            DeliveryId = g.First().DeliveryId,
            Deliverys = g.First().Deliverys
        });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ColorDelivery was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // GET: api/ColorDeliveries/5
        [Authorize(Roles = "6")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ColorDelivery>> GetColorDelivery(int? id)
        {
            try
            {
                if (_context.ColorDeliveries == null)
                {
                    _logger.LogWarning("ColorDelivery was null");
                    return NotFound();
                }
                var colorDelivery = await _context.ColorDeliveries.FindAsync(id);

                if (colorDelivery == null)
                {
                    _logger.LogWarning("ColorDelivery was null");
                    return NotFound();
                }
                _logger.LogInformation("ColorDelivery was GetColorDelivery");
                return colorDelivery;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ColorDelivery was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // PUT: api/ColorDeliveries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "2")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColorDelivery(int? id, ColorDelivery colorDelivery)
        {
            try
            {
                if (id != colorDelivery.IdColorDelivery)
                {
                    _logger.LogWarning("ColorDelivery was null");
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    _context.Entry(colorDelivery).State = EntityState.Modified;
                    _logger.LogInformation("ColorDelivery was PutColorDelivery");
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"ColorDelivery is not valid");
                    return Problem("ColorDelivery is not valid");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ColorDelivery was critical error {ex}");
                return Problem("Critical error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ColorDelivery>> PostColorDelivery(ColorDelivery colorDelivery)
        {
            try
            {
                if (_context.ColorDeliveries == null)
                {
                    _logger.LogWarning("ColorDelivery was null");
                    return Problem("Entity set 'PaintContext.ColorDeliveries'  is null.");
                }
                if (ModelState.IsValid)
                {
                    _context.ColorDeliveries.Add(colorDelivery);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("ColorDelivery was PostColorDelivery");
                    return CreatedAtAction("GetColorDelivery", new { id = colorDelivery.IdColorDelivery }, colorDelivery);
                }
                else
                {
                    _logger.LogWarning($"ColorDelivery is not valid");
                    return Problem("ColorDelivery is not valid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ColorDelivery was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // DELETE: api/ColorDeliveries/5
        [Authorize(Roles = "2")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColorDelivery(int? id)
        {
            try
            {
                if (_context.ColorDeliveries == null)
                {
                    _logger.LogWarning("ColorDelivery was null");
                    return NotFound();
                }
                var colorDelivery = await _context.ColorDeliveries.FindAsync(id);
                if (colorDelivery == null)
                {
                    _logger.LogWarning("ColorDelivery was null");
                    return NotFound();
                }

                _context.ColorDeliveries.Remove(colorDelivery);
                await _context.SaveChangesAsync();
                _logger.LogInformation("ColorDelivery was DeleteColorDelivery");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ColorDelivery was critical error {ex}");
                return Problem("Critical error");
            }
        }

        private bool ColorDeliveryExists(int? id)
        {
            return (_context.ColorDeliveries?.Any(e => e.IdColorDelivery == id)).GetValueOrDefault();
        }
    }
}
