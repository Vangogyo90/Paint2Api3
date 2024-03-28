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
    public class TokensController : ControllerBase
    {
        private readonly PaintContext _context;

        public TokensController(PaintContext context)
        {
            _context = context;
        }

        // GET: api/Tokens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Token>>> GetTokens()
        {
          if (_context.Tokens == null)
          {
              return NotFound();
          }
            return await _context.Tokens.ToListAsync();
        }

        // GET: api/Tokens/5
        [Authorize(Roles = "1")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Token>> GetToken(int? id)
        {
          if (_context.Tokens == null)
          {
              return NotFound();
          }
            var token = await _context.Tokens.FindAsync(id);

            if (token == null)
            {
                return NotFound();
            }

            return token;
        }

        [Authorize(Roles = "1")]
        [HttpGet("GetData/{idUser}")]
        public IActionResult GetData(int idUser)
        {
            if (_context.Tokens == null)
            {
                return NotFound();
            }

            IQueryable<Token> data = _context.Tokens.Include(a => a.Users!.Roles);
            if (idUser > 0)
            {
                data = data.Where(datas => datas.UserId != idUser);
            }

            return Ok(data.ToList());
        }

        [Authorize(Roles = "1")]
        [HttpGet("GetDataByID/{idUser}")]
        public IActionResult GetDataByID(int idUser)
        {
            if (_context.Tokens == null)
            {
                return NotFound();
            }

            IQueryable<Token> data = _context.Tokens.Include(a => a.Users!.Roles);
            if (idUser > 0)
            {
                data = data.Where(datas => datas.UserId == idUser);
            }

            return Ok(data.ToList());
        }

        // PUT: api/Tokens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToken(int? id, Token token)
        {
            if (id != token.IdToken)
            {
                return BadRequest();
            }

            _context.Entry(token).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TokenExists(id))
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

        // POST: api/Tokens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<Token>> PostToken(Token token)
        {
          if (_context.Tokens == null)
          {
              return Problem("Entity set 'PaintContext.Tokens'  is null.");
          }
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToken", new { id = token.IdToken }, token);
        }

        // DELETE: api/Tokens/5
        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToken(int? id)
        {
            if (_context.Tokens == null)
            {
                return NotFound();
            }
            var token = await _context.Tokens.FindAsync(id);
            if (token == null)
            {
                return NotFound();
            }

            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TokenExists(int? id)
        {
            return (_context.Tokens?.Any(e => e.IdToken == id)).GetValueOrDefault();
        }
    }
}
