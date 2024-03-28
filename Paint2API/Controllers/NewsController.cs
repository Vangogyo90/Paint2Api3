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
    public class NewsController : ControllerBase
    {
        private readonly PaintContext _context;

        public NewsController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
          if (_context.News == null)
          {
              return NotFound();
          }
            return await _context.News.ToListAsync();
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int? id)
        {
          if (_context.News == null)
          {
              return NotFound();
          }
            var news = await _context.News.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        [HttpGet("NewsPhoto/{id}")]
        public async Task<ActionResult<NewsPhoto>> GetNewsWithPhoto(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(n => n.IdNews == id);

            if (news == null)
            {
                return NotFound();
            }

            var photos = await _context.PhotoNews
                .Where(p => p.NewsId == id)
                .ToListAsync();

            var newsPhoto = new NewsPhoto
            {
                News = news,
                PhotoNews = photos
            };

            return newsPhoto;
        }

        // PUT: api/News/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "4")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int? id, News news)
        {
            if (id != news.IdNews)
            {
                return BadRequest();
            }

            _context.Entry(news).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
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

        // POST: api/News
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "4")]
        [HttpPost]
        public async Task<ActionResult<News>> PostNews(News news)
        {
          if (_context.News == null)
          {
              return Problem("Entity set 'PaintContext.News'  is null.");
          }
            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news.IdNews }, news);
        }

        // DELETE: api/News/5
        [Authorize(Roles = "4")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int? id)
        {
            if (_context.News == null)
            {
                return NotFound();
            }
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsExists(int? id)
        {
            return (_context.News?.Any(e => e.IdNews == id)).GetValueOrDefault();
        }
    }
}
