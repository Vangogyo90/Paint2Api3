using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
    public class DeliveriesController : ControllerBase
    {
        private readonly PaintContext _context;
        private readonly ILogger<DeliveriesController> _logger;

        public DeliveriesController(PaintContext context, ILogger<DeliveriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDeliveries()
        {
            if (_context.Deliveries == null)
            {
                return NotFound();
            }
            return await _context.Deliveries.ToListAsync();
        }

        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            try
            {
                if (_context.Deliveries == null)
                {
                    _logger.LogWarning("Deliveries was null");
                    return NotFound();
                }

                List<Delivery> data = _context.Deliveries
                    .Include(a => a.Cites)
                    .Include(a => a.Users)
                    .Include(a => a.StatusDeliveres)
                    .ToList();
                _logger.LogInformation("Deliveries was GetData");
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Deliveries was critical error {ex}");
                return Problem("Critical error");
            }
        }

        [HttpGet("GetDataByUser")]
        public IActionResult GetDataByUser(int idUser)
        {
            try
            {
                if (_context.Deliveries == null)
                {
                    _logger.LogWarning("Deliveries was null");
                    return NotFound();
                }

                IQueryable<Delivery> data = _context.Deliveries
                    .Include(a => a.Cites)
                    .Include(a => a.Users)
                    .Include(a => a.StatusDeliveres);

                if (idUser > 0)
                {
                    data = data.Where(delivery => delivery.UserId == idUser);
                }
                _logger.LogInformation("Deliveries was GetDataByUser");
                return Ok(data.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Deliveries was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // GET: api/Deliveries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery>> GetDelivery(int? id)
        {
            try
            {
                if (_context.Deliveries == null)
                {
                    _logger.LogWarning("Deliveries was null");
                    return NotFound();
                }
                var delivery = await _context.Deliveries.FindAsync(id);

                if (delivery == null)
                {
                    _logger.LogWarning("Deliveries was null");
                    return NotFound();
                }
                else
                    delivery.Adress = HashWithSalt.Encrypt(delivery.Adress!);
                _logger.LogInformation("Deliveries was GetDataByUser");
                return delivery;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Deliveries was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // PUT: api/Deliveries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDelivery(int? id, Delivery delivery)
        {
            try
            {
                if (id != delivery.IdDelivery)
                {
                    _logger.LogWarning("Deliveries was null");
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    _context.Entry(delivery).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Deliveries was PutDelivery");
                }
                else
                {
                    _logger.LogWarning($"Deliveries is not valid");
                    return Problem("Deliveries is not valid");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Deliveries was critical error {ex}");
                return Problem("Critical error");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Delivery>> PostDelivery(Delivery delivery)
        {
            try
            {
                if (_context.Deliveries == null)
                {
                    _logger.LogWarning("Deliveries was null");
                    return Problem("Entity set 'PaintContext.Deliveries'  is null.");
                }

                delivery.Salt = Convert.ToBase64String(GetSalt(20));
                delivery.Adress = HashWithSalt.Decrypt(delivery.Adress!);


                if (ModelState.IsValid)
                {
                    _context.Deliveries.Add(delivery);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Deliveries was PostDelivery");

                    return CreatedAtAction("GetDelivery", new { id = delivery.IdDelivery }, delivery);
                }
                else
                {
                    _logger.LogWarning($"Deliveries is not valid");
                    return Problem("Deliveries is not valid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Deliveries was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // DELETE: api/Deliveries/5
        [Authorize(Roles = "2")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(int? id)
        {
            try
            {
                if (_context.Deliveries == null)
                {
                    _logger.LogWarning("Deliveries was null");
                    return NotFound();
                }

                var delivery = await _context.Deliveries.FindAsync(id);
                if (delivery == null)
                {
                    _logger.LogWarning("Deliveries was null");
                    return NotFound();
                }

                _context.Deliveries.Remove(delivery);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deliveries was DeleteDelivery");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Deliveries was critical error {ex}");
                return Problem("Critical error");
            }
        }

        private bool DeliveryExists(int? id)
        {
            return (_context.Deliveries?.Any(e => e.IdDelivery == id)).GetValueOrDefault();
        }

        public static byte[] GetSalt(int length)
        {
            byte[] salt = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
