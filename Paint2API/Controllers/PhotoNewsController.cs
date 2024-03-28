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
    public class PhotoNewsController : ControllerBase
    {
        private readonly PaintContext _context;

        public PhotoNewsController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/PhotoNews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhotoNews>>> GetPhotoNews()
        {
          if (_context.PhotoNews == null)
          {
              return NotFound();
          }
            return await _context.PhotoNews.ToListAsync();
        }

        // GET: api/PhotoNews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoNews>> GetPhotoNews(int? id)
        {
          if (_context.PhotoNews == null)
          {
              return NotFound();
          }
            var photoNews = await _context.PhotoNews.FindAsync(id);

            if (photoNews == null)
            {
                return NotFound();
            }

            return photoNews;
        }

        [HttpGet("GetPhotoNewsByNewsID/{id}")]
        public IActionResult GetPhotoNewsByNewsID(int? id)
        {
            if (_context.PhotoNews == null)
            {
                return NotFound();
            }
            IQueryable<PhotoNews> data = _context.PhotoNews.Include(a => a.Newses);

            if (id > 0)
            {
                data = data.Where(a => a.NewsId == id);
            }

            return Ok(data.ToList());
        }

        // PUT: api/PhotoNews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "4")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhotoNews(int? id, PhotoNews photoNews)
        {
            if (id != photoNews.IdPhotoNews)
            {
                return BadRequest();
            }

            _context.Entry(photoNews).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoNewsExists(id))
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

        // POST: api/PhotoNews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "4")]
        [HttpPost]
        public async Task<ActionResult<PhotoNews>> PostPhotoNews(PhotoNews photoNews)
        {
          if (_context.PhotoNews == null)
          {
              return Problem("Entity set 'PaintContext.PhotoNews'  is null.");
          }
            _context.PhotoNews.Add(photoNews);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PhotoNewsExists(photoNews.IdPhotoNews))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPhotoNews", new { id = photoNews.IdPhotoNews }, photoNews);
        }

        // DELETE: api/PhotoNews/5
        [Authorize(Roles = "4")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhotoNews(int? id)
        {
            if (_context.PhotoNews == null)
            {
                return NotFound();
            }
            var photoNews = await _context.PhotoNews.FindAsync(id);
            if (photoNews == null)
            {
                return NotFound();
            }

            _context.PhotoNews.Remove(photoNews);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "4")]
        [HttpDelete("DeletePhotoNewsByNewsID/{id}")]
        public async Task<IActionResult> DeletePhotoNewsByNewsID(int? id)
        {
            if (_context.PhotoNews == null)
            {
                return NotFound();
            }
            IQueryable<PhotoNews> data = _context.PhotoNews.Include(a => a.Newses);
            data = data.Where(a => a.NewsId == id);

            foreach (PhotoNews photoNews in data)
            {
                _context.PhotoNews.Remove(photoNews);
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhotoNewsExists(int? id)
        {
            return (_context.PhotoNews?.Any(e => e.IdPhotoNews == id)).GetValueOrDefault();
        }
    }
}
