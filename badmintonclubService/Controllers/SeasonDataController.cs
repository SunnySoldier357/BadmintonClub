﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using badmintonclubService.DataObjects;
using badmintonclubService.Models;

namespace badmintonclubService.Controllers
{
    public class SeasonDataController : TableController<SeasonData>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            badmintonclubContext context = new badmintonclubContext();
            DomainManager = new EntityDomainManager<SeasonData>(context, Request);
        }

        // GET tables/SeasonData
        public IQueryable<SeasonData> GetAllSeasonData()
        {
            return Query(); 
        }

        // GET tables/SeasonData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<SeasonData> GetSeasonData(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/SeasonData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<SeasonData> PatchSeasonData(string id, Delta<SeasonData> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/SeasonData
        public async Task<IHttpActionResult> PostSeasonData(SeasonData item)
        {
            SeasonData current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/SeasonData/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSeasonData(string id)
        {
             return DeleteAsync(id);
        }
    }
}
