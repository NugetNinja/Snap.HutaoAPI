using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.SnapGenshin;

namespace Snap.Genshin.Website.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        public RecordController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly ApplicationDbContext dbContext;

        [HttpPost]
        public async Task<IActionResult> UploadRecord([FromBody]PlayerRecord record)
        {
            await Task.CompletedTask;
            return this.Ok(record);
        }
    }
}
