using System;
using System.Threading.Tasks;
using Backend.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.API
{
    [Route("tables/Match")]
    public class MatchController : TableController<Match>
    {
        // Private Properties
        private readonly BadmintonClubDBDataContext _db;

        // Constructors
        public MatchController(BadmintonClubDBDataContext db) => _db = db;

        // Overridden Methods
        public override async Task<IActionResult> GetAllAsync() => 
            Json(await _db.Matches
                .Include(m => m.Opponent)
                .Include(m => m.Player)
                .Include(m => m.User.Id)
                .ToArrayAsync());

        public override async Task<IActionResult> CreateAsync([FromBody] Match created)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Match match = (await _db
                .AddAsync(new Match(created))).Entity;
            await _db.SaveChangesAsync();

            return Json(match);
        }

        public override async Task<IActionResult> GetAsync(string id)
        {
            Match match = await _db.Matches
                .Include(m => m.Opponent)
                .Include(m => m.Player)
                .Include(m => m.User.Id)
                .SingleOrDefaultAsync(m => m.Id == new Guid(id));

            if (match == null)
                return NotFound();

            return Json(match);
        }

        public override async Task<IActionResult> UpdateAsync(string id, [FromBody] Match updated)
        {
            Match match = await _db.Matches
                .Include(m => m.Opponent)
                .Include(m => m.Player)
                .Include(m => m.User.Id)
                .SingleOrDefaultAsync(m => m.Id == new Guid(id));

            if (match == null)
                return NotFound();
            
            if (match.Id != updated.Id || !ModelState.IsValid)
                return BadRequest();

            match.Update(updated);

            await _db.SaveChangesAsync();

            return Json(match);
        }

        public override async Task<IActionResult> DeleteAsync(string id)
        {
            Match match = await _db.Matches
                .FindAsync(new Guid(id));
            
            if (match == null)
                return NotFound();

            _db.Matches.Remove(match);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}