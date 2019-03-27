using System;
using System.Threading.Tasks;
using Backend.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.API
{
    [Route("tables/Season")]
    public class SeasonController : TableController<Season>
    {
        // Private Properties
        private readonly BadmintonClubDBDataContext _db;

        // Constructors
        public SeasonController(BadmintonClubDBDataContext db) => _db = db;

        // Overridden Methods
        public override async Task<IActionResult> GetAllAsync() =>
            Json(await _db.Seasons
                .Include(s => s.User)
                .Include(s => s.Matches)
                .Include(s => s.Statistics)
                .ToArrayAsync());

        public override async Task<IActionResult> CreateAsync([FromBody] Season created)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Season season = (await _db
                .AddAsync(new Season(created))).Entity;
            await _db.SaveChangesAsync();

            return Json(season);
        }

        public override async Task<IActionResult> GetAsync(string id)
        {
            Season season = await _db.Seasons
                .Include(s => s.User)
                .Include(s => s.Matches)
                .Include(s => s.Statistics)
                .SingleOrDefaultAsync(s => s.Id == new Guid(id));

            if (season == null)
                return NotFound();

            return Json(season);
        }

        public override async Task<IActionResult> UpdateAsync(string id, [FromBody] Season updated)
        {
            Season season = await _db.Seasons
                .Include(s => s.User)
                .Include(s => s.Matches)
                .Include(s => s.Statistics)
                .SingleOrDefaultAsync(s => s.Id == new Guid(id));
            
            if (season == null)
                return NotFound();
            
            if (season.Id != updated.Id || !ModelState.IsValid)
                return BadRequest();

            season.Update(updated);

            await _db.SaveChangesAsync();

            return Json(season);
        }

        public override async Task<IActionResult> DeleteAsync(string id)
        {
            Season season = await _db.Seasons
                .FindAsync(new Guid(id));

            if (season == null)
                return NotFound();

            _db.Seasons.Remove(season);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}