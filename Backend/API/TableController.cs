using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API
{
    public abstract class TableController<T> : Controller
    {
        // Public Methods
        [HttpGet]
        [Route("")]
        public abstract Task<IActionResult> GetAllAsync();

        [HttpPost]
        [Route("")]
        public abstract Task<IActionResult> CreateAsync([FromBody] T created);

        [HttpGet]
        [Route("{id}")]
        public abstract Task<IActionResult> GetAsync(string id);

        [HttpPatch]
        [Route("{id}")]
        public abstract Task<IActionResult> UpdateAsync(string id, [FromBody] T updated);

        [HttpDelete]
        [Route("{id}")]
        public abstract Task<IActionResult> DeleteAsync(string id);
    }
}