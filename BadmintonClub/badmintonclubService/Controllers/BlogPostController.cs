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
    public class BlogPostController : TableController<BlogPost>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            badmintonclubContext context = new badmintonclubContext();
            DomainManager = new EntityDomainManager<BlogPost>(context, Request);
        }

        // GET tables/BlogPost
        public IQueryable<BlogPost> GetAllBlogPost()
        {
            return Query(); 
        }

        // GET tables/BlogPost/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<BlogPost> GetBlogPost(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/BlogPost/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<BlogPost> PatchBlogPost(string id, Delta<BlogPost> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/BlogPost
        public async Task<IHttpActionResult> PostBlogPost(BlogPost item)
        {
            BlogPost current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/BlogPost/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteBlogPost(string id)
        {
             return DeleteAsync(id);
        }
    }
}
