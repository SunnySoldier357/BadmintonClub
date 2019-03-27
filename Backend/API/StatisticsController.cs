using System;
using System.Threading.Tasks;
using Backend.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.API
{
    [Route("tables/Statistics")]
    public class StatisticsController : TableController<Statistics>
    {
        // Private Properties
        private readonly BadmintonClubDBDataContext _db;

        // Constructors
        public StatisticsController(BadmintonClubDBDataContext db) => _db = db;

        // Overridden Methods
        public override async Task<IActionResult> GetAllAsync() =>
            Json(await _db.Statistics.ToArrayAsync());

        public override async Task<IActionResult> CreateAsync([FromBody] Statistics created)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Statistics statistics = (await _db
                .AddAsync(new Statistics(created))).Entity;
            await _db.SaveChangesAsync();

            return Json(statistics);
        }

        public override async Task<IActionResult> GetAsync(string id)
        {
            Statistics statistics = await _db.Statistics
                .FindAsync(new Guid(id));

            if (statistics == null)
                return NotFound();

            return Json(statistics);
        }

        public override async Task<IActionResult> UpdateAsync(string id, [FromBody] Statistics updated)
        {
            Statistics statistics = await _db.Statistics
                .FindAsync(new Guid(id));

            if (statistics == null)
                return NotFound();
            // TODO: Continue
            return Json(statistics);
        }

        public override async Task<IActionResult> DeleteAsync(string id)
        {
            Statistics statistics = await _db.Statistics
                .FindAsync(new Guid(id));

            if (statistics == null)
                return NotFound();

            _db.Statistics.Remove(statistics);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}