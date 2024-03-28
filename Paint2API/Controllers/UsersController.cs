using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoPartsAPI.Hash;
using AutoPartsAPI.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Paint2API.Models;

namespace Paint2API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PaintContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(PaintContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetUserWithRole")]
        public IActionResult GetUserWithRole()
        {
            try
            {
                if (_context.Users == null)
                {
                    _logger.LogWarning("User was null");
                    return NotFound();
                }

                var user = _context.Users.Include(a => a.Roles).ToList();
                _logger.LogInformation("Users was GetUserWithRole");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"User was critical error {ex}");
                return Problem("Critical error");
            }
        }

        [HttpGet("RoleByLogin/{Login}")]
        public async Task<ActionResult<int>> GetRoleByLogin(string Login)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Login == Login);
                if (user == null)
                {
                    _logger.LogWarning("Invalid Login");
                    return BadRequest("Invalid Login");
                }
                _logger.LogInformation("Users was GetRoleByLogin");
                return user.RoleId!;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"User was critical error {ex}");
                return Problem("Critical error");
            }
        }

        [HttpGet("VerificationbyLogin/{Login}")]
        public async Task<ActionResult<bool>> VerificationbyLogin(string Login)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Login == Login);
                if (user == null)
                {
                    _logger.LogWarning("Invalid Login");
                    return BadRequest("Invalid Login");
                }
                _logger.LogInformation("Users was VerificationbyLogin");
                return user.Verification;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"User was critical error {ex}");
                return Problem("Critical error");
            }
        }

        [HttpGet("UserIDByLogin/{Login}")]
        public async Task<ActionResult<int>> UserIDByLogin(string Login)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Login == Login);
                if (user == null)
                {
                    _logger.LogWarning("Invalid Login");
                    return BadRequest("Invalid Login");
                }
                _logger.LogInformation("Users was UserIDByLogin");
                return user.IdUser!;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"User was critical error {ex}");
                return Problem("Critical error");
            }
        }

        [HttpGet("{Login}/{Password}")]
        public async Task<ActionResult<string>> GetTokenAuth(string Login, string Password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Login == Login);

                if (user == null)
                {
                    _logger.LogWarning("Invalid Login");
                    return BadRequest("Invalid Login");
                }

                byte[] saltbytes = Convert.FromBase64String(user.Salt!);
                byte[] PasswordBytes = Encoding.UTF8.GetBytes(Password);

                Password = Convert.ToBase64String(HashWithSalt.GenerateSaltedHash(PasswordBytes, saltbytes));

                if (user.Password != Password)
                {
                    _logger.LogWarning("Invalid Password");
                    return BadRequest("Invalid Password");
                }

                string token;
                Models.Token trueToken = new Token();

                trueToken = await _context.Tokens!.FirstOrDefaultAsync(p => p.UserId == user.IdUser) ?? trueToken;

                if (trueToken == null)
                {
                    _logger.LogInformation("Token generated");
                    token = NewToken.GetToken(Login, user.RoleId.ToString()!);
                    Models.Token tokk = new Models.Token();
                    tokk.Token1 = token;
                    tokk.UserId = user.IdUser;
                    _context.Tokens.Add(tokk);
                    await _context.SaveChangesAsync();
                }
                else
                    token = trueToken.Token1!;
                _logger.LogInformation($"User was Auth with login {Login}");
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"User was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                if (_context.Users == null)
                {
                    _logger.LogWarning("User was null");
                    return NotFound();
                }
                _logger.LogInformation("Users was GetUsers");
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"User was critical error {ex}");
                return Problem("Critical error");
            }
        }
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int? id)
        {
            try
            {
                if (_context.Users == null)
                {
                    _logger.LogWarning("User was null");
                    return NotFound();
                }
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User was null");
                    return NotFound();
                }
                _logger.LogInformation("Users was GetUser");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"User was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int? id, User user)
        {
            if (id != user.IdUser)
            {
                _logger.LogWarning("User was not found");
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Users was PutUser");
            }
            else
            {
                _logger.LogWarning($"User is not valid");
                return Problem("User is not valid");
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                byte[] Salt = GetSalt(20);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(user.Password!);
                user.Salt = Convert.ToBase64String(Salt);
                user.Password = Convert.ToBase64String(HashWithSalt.GenerateSaltedHash(passwordBytes, Salt));

                if (_context.Users == null)
                {
                    _logger.LogWarning("User was null");
                    return Problem("Entity set 'Paint.Users'  is null.");
                }

                if (ModelState.IsValid)
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"User was PostUser");
                    return CreatedAtAction("GetUser", new { id = user.IdUser }, user);
                }
                else
                {
                    _logger.LogWarning($"User is not valid");
                    return Problem("User is not valid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"User was critical error {ex}");
                return Problem("Critical error");
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            try
            {
                if (_context.Users == null)
                {
                    _logger.LogWarning("User was null");
                    return NotFound();
                }
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User was null");
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"User was DeleteUser");
                }
                else
                {
                    _logger.LogWarning($"User is not valid");
                    return Problem("User is not valid");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"User was critical error {ex}");
                return Problem("Critical error");
            }
        }

        private bool UserExists(int? id)
        {
            return (_context.Users?.Any(e => e.IdUser == id)).GetValueOrDefault();
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
