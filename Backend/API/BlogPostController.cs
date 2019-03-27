using System;
using System.Threading.Tasks;
using Backend.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.API
{
    [Route("tables/BlogPost")]
    public class BlogPostController : TableController<BlogPost>
    {
        // Private Properties
        private readonly BadmintonClubDBDataContext _db;

        // Constructors
        public BlogPostController(BadmintonClubDBDataContext db) => _db = db;

        // Overridden Methods
        public override async Task<IActionResult> GetAllAsync() =>
            Json(await _db.BlogPosts
                .Include(bp => bp.Publisher)
                    .ThenInclude(u => u.Statistics)
                .ToArrayAsync());

        public override async Task<IActionResult> CreateAsync([FromBody] BlogPost created)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            BlogPost post = (await _db
                .AddAsync(new BlogPost(created))).Entity;
            await _db.SaveChangesAsync();

            return Json(post);
        }

        public override async Task<IActionResult> GetAsync(string id)
        {
            BlogPost post = await _db.BlogPosts
                .Include(bp => bp.Publisher)
                .SingleOrDefaultAsync(bp => bp.Id == new Guid(id));

            if (post == null)
                return NotFound();

            return Json(post);
        }

        public override async Task<IActionResult> UpdateAsync(string id, [FromBody] BlogPost updated)
        {
            BlogPost post = await _db.BlogPosts
                .Include(bp => bp.Publisher)
                .SingleOrDefaultAsync(bp => bp.Id == new Guid(id));

            if (post == null)
                return NotFound();

            if (post.Id != updated.Id || !ModelState.IsValid)
                return BadRequest();

            post.Update(updated);

            await _db.SaveChangesAsync();

            return Json(post);
        }

        public override async Task<IActionResult> DeleteAsync(string id)
        {
            BlogPost post = await _db.BlogPosts
                .FindAsync(new Guid(id));

            if (post == null)
                return NotFound();

            _db.BlogPosts.Remove(post);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}