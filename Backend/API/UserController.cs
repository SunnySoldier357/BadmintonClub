using System;
using System.Threading.Tasks;
using Backend.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.API
{
    [Route("tables/User")]
    public class UserController : TableController<User>
    {
        // Private Properties
        private readonly BadmintonClubDBDataContext _db;

        // Constructors
        public UserController(BadmintonClubDBDataContext db) => _db = db;

        // Overridden Methods
        public override async Task<IActionResult> GetAllAsync() =>
            Json(await _db.Users
                .Include(u => u.Matches)
                    .ThenInclude(m => m.Opponent)
                .Include(u => u.Matches)
                    .ThenInclude(m => m.Player)
                .ToArrayAsync());

        public override async Task<IActionResult> CreateAsync([FromBody] User created)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            User user = (await _db
                .AddAsync(new User(created))).Entity;
            await _db.SaveChangesAsync();

            return Json(user);
        }

        public override async Task<IActionResult> GetAsync(string id)
        {
            User user = await _db.Users
                .Include(u => u.Matches)
                    .ThenInclude(m => m.Opponent)
                .Include(u => u.Matches)
                    .ThenInclude(m => m.Player)
                .SingleOrDefaultAsync(u => u.Id == new Guid(id));

            if (null == user)
                return NotFound();

            return Json(user);
        }

        public override async Task<IActionResult> UpdateAsync(string id, [FromBody] User updated)
        {
            User user = await _db.Users
                .Include(u => u.Matches)
                    .ThenInclude(m => m.Opponent)
                .Include(u => u.Matches)
                    .ThenInclude(m => m.Player)
                .SingleOrDefaultAsync(u => u.Id == new Guid(id));

            if (null == user)
                return NotFound();
            
            if (user.Id != updated.Id || !ModelState.IsValid)
                return BadRequest();

            user.Update(updated);

            await _db.SaveChangesAsync();

            return Json(user);
        }

        public override async Task<IActionResult> DeleteAsync(string id)
        {
            User user = await _db.Users
                .FindAsync(new Guid(id));
            
            if (null == user)
                return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}