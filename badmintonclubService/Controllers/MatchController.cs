using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using badmintonclubService.DataObjects;
using badmintonclubService.Models;

namespace badmintonclubService.Controllers
{
    public class MatchController : TableController<Match>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            badmintonclubContext context = new badmintonclubContext();
            DomainManager = new EntityDomainManager<Match>(context, Request);
        }

        // GET tables/Match
        public IQueryable<Match> GetAllMatch()
        {
            return Query(); 
        }

        // GET tables/Match/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Match> GetMatch(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Match/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Match> PatchMatch(string id, Delta<Match> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Match
        public async Task<IHttpActionResult> PostMatch(Match item)
        {
            Match current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Match/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteMatch(string id)
        {
             return DeleteAsync(id);
        }
    }
}
