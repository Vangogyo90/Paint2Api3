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
    public class QuantityColorsController : ControllerBase
    {
        private readonly PaintContext _context;

        public QuantityColorsController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/QuantityColors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuantityColor>>> GetQuantityColors()
        {
          if (_context.QuantityColors == null)
          {
              return NotFound();
          }
            return await _context.QuantityColors.ToListAsync();
        }

        // GET: api/QuantityColors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuantityColor>> GetQuantityColor(int? id)
        {
          if (_context.QuantityColors == null)
          {
              return NotFound();
          }
            var quantityColor = await _context.QuantityColors.FindAsync(id);

            if (quantityColor == null)
            {
                return NotFound();
            }

            return quantityColor;
        }

        [HttpGet("GetDataByID/{id}")]
        public async Task<ActionResult<QuantityColor>> GetDataByID(int? id)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }
            var quantityColor = await _context.QuantityColors
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
                .FirstOrDefaultAsync(a => a.IdQuantityColors == id);

            if (quantityColor == null)
            {
                return NotFound();
            }

            return quantityColor;
        }

        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }

            List<QuantityColor> data = _context.QuantityColors
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
                .ToList();

            return Ok(data);
        }

        [HttpGet("PaginationStocks")]
        public IActionResult PaginationStocks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }

            var data = _context.QuantityColors
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
    .Join(_context.Discounts,
          qc => qc.ColorId,
          d => d.ColorId,
          (qc, d) => new
          {
              QuantityColor = qc,
              Discount = d
          });

            var totalItems = data.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                QuantityColor = result.Select(x => x.QuantityColor).ToList(),
                Discounts = result.Select(x => x.Discount).ToList()
            });
        }

        [HttpGet("Pagination")]
        public IActionResult Pagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }

            var data = _context.QuantityColors
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
    .GroupJoin(_context.Discounts,
        qc => qc.ColorId,
        d => d.ColorId,
        (qc, discounts) => new
        {
            QuantityColor = qc,
            Discounts = discounts
        })
    .SelectMany(
        x => x.Discounts.DefaultIfEmpty(),
        (qc, discount) => new
        {
            QuantityColor = qc.QuantityColor,
            Discount = discount
        });

            var totalItems = data.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                QuantityColor = result.Select(x => x.QuantityColor).ToList(),
                Discounts = result.Select(x => x.Discount).ToList()
            });
        }

        [HttpGet("UpPriceStocks")]
        public IActionResult UpPriceStocks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }

            var data = _context.QuantityColors
                .OrderBy(a => a.Price_For_KG)
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
    .Join(_context.Discounts,
          qc => qc.ColorId,
          d => d.ColorId,
          (qc, d) => new
          {
              QuantityColor = qc,
              Discount = d
          });

            var totalItems = data.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                QuantityColor = result.Select(x => x.QuantityColor).ToList(),
                Discounts = result.Select(x => x.Discount).ToList()
            });
        }

        [HttpGet("DownPriceStocks")]
        public IActionResult DownPriceStocks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }

            var data = _context.QuantityColors
                .OrderByDescending(a => a.Price_For_KG)
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
    .Join(_context.Discounts,
          qc => qc.ColorId,
          d => d.ColorId,
          (qc, d) => new
          {
              QuantityColor = qc,
              Discount = d
          });

            var totalItems = data.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                QuantityColor = result.Select(x => x.QuantityColor).ToList(),
                Discounts = result.Select(x => x.Discount).ToList()
            });
        }

        [HttpGet("UpPrice")]
        public IActionResult UpPrice([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }

            var data = _context.QuantityColors
                .OrderBy(a => a.Price_For_KG)
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
                    .GroupJoin(_context.Discounts,
        qc => qc.ColorId,
        d => d.ColorId,
        (qc, discounts) => new
        {
            QuantityColor = qc,
            Discounts = discounts
        })
    .SelectMany(
        x => x.Discounts.DefaultIfEmpty(),
        (qc, discount) => new
        {
            QuantityColor = qc.QuantityColor,
            Discount = discount
        });

            var totalItems = data.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                QuantityColor = result.Select(x => x.QuantityColor).ToList(),
                Discounts = result.Select(x => x.Discount).ToList()
            });
        }

        [HttpGet("DownPrice")]
        public IActionResult DownPrice([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }

            var data = _context.QuantityColors
                .OrderByDescending(a => a.Price_For_KG)
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
                                    .GroupJoin(_context.Discounts,
        qc => qc.ColorId,
        d => d.ColorId,
        (qc, discounts) => new
        {
            QuantityColor = qc,
            Discounts = discounts
        })
    .SelectMany(
        x => x.Discounts.DefaultIfEmpty(),
        (qc, discount) => new
        {
            QuantityColor = qc.QuantityColor,
            Discount = discount
        });

            var totalItems = data.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                QuantityColor = result.Select(x => x.QuantityColor).ToList(),
                Discounts = result.Select(x => x.Discount).ToList()
            });
        }

        [HttpGet("SearchStocks")]
        public IActionResult SearchStocks(string searchQuery, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }

            var data = _context.QuantityColors
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
    .Join(_context.Discounts,
          qc => qc.ColorId,
          d => d.ColorId,
          (qc, d) => new
          {
              QuantityColor = qc,
              Discount = d
          }).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                data = data.Where(item =>
                    item.QuantityColor.Price_For_KG.ToString().ToLower().Any(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors!.Shines!.NameShine.ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors!.TypeSurfaces!.NameTypeSurface.ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors.TypeApplications!.NameTypeApplication.ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors.TempPulverizations!.Degree.ToString().ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors.TempPulverizations.Time.ToString().ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.WareHouses!.City!.NameCity.ToString().ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors.RalCatalog!.ColorRal.ToString().ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors.RalCatalog.NameRal.ToString().ToLower().All(searchQuery.ToLower().Contains)
                ).ToList();
            }

            var totalItems = data.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                QuantityColor = result.Select(x => x.QuantityColor).ToList(),
                Discounts = result.Select(x => x.Discount).ToList()
            });
        }

        [HttpGet("Search")]
        public IActionResult Search(string searchQuery, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }

            var data = _context.QuantityColors
                .Include(a => a.Colors)
                .Include(a => a.Colors!.TypeApplications)
                .Include(a => a.Colors!.TempPulverizations)
                .Include(a => a.Colors!.Shines)
                .Include(a => a.Colors!.TypeSurfaces)
                .Include(a => a.Colors!.RalCatalog)
                .Include(a => a.WareHouses)
                .Include(a => a.WareHouses!.City)
                                    .GroupJoin(_context.Discounts,
        qc => qc.ColorId,
        d => d.ColorId,
        (qc, discounts) => new
        {
            QuantityColor = qc,
            Discounts = discounts
        })
    .SelectMany(
        x => x.Discounts.DefaultIfEmpty(),
        (qc, discount) => new
        {
            QuantityColor = qc.QuantityColor,
            Discount = discount
        }).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                data = data.Where(item =>
                    item.QuantityColor.Colors!.RalCatalog!.NameRal.ToString().ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Price_For_KG.ToString().ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors.Shines!.NameShine.ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors.TypeSurfaces!.NameTypeSurface.ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors.TypeApplications!.NameTypeApplication.ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.WareHouses!.City!.NameCity.ToString().ToLower().All(searchQuery.ToLower().Contains) |
                    item.QuantityColor.Colors.RalCatalog.ColorRal.ToString().ToLower().All(searchQuery.ToLower().Contains)
                ).ToList();
            }

            var totalItems = data.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                QuantityColor = result.Select(x => x.QuantityColor).ToList(),
                Discounts = result.Select(x => x.Discount).ToList()
            });
        }

        // PUT: api/QuantityColors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "5")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuantityColor(int? id, QuantityColor quantityColor)
        {
            if (id != quantityColor.IdQuantityColors)
            {
                return BadRequest();
            }

            _context.Entry(quantityColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuantityColorExists(id))
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

        // POST: api/QuantityColors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "5")]
        [HttpPost]
        public async Task<ActionResult<QuantityColor>> PostQuantityColor(QuantityColor quantityColor)
        {
          if (_context.QuantityColors == null)
          {
              return Problem("Entity set 'PaintContext.QuantityColors'  is null.");
          }
            _context.QuantityColors.Add(quantityColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuantityColor", new { id = quantityColor.IdQuantityColors }, quantityColor);
        }

        // DELETE: api/QuantityColors/5
        [Authorize(Roles = "5")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuantityColor(int? id)
        {
            if (_context.QuantityColors == null)
            {
                return NotFound();
            }
            var quantityColor = await _context.QuantityColors.FindAsync(id);
            if (quantityColor == null)
            {
                return NotFound();
            }

            _context.QuantityColors.Remove(quantityColor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuantityColorExists(int? id)
        {
            return (_context.QuantityColors?.Any(e => e.IdQuantityColors == id)).GetValueOrDefault();
        }
    }
}
