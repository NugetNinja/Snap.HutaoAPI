using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models;
using System.ComponentModel.DataAnnotations;

namespace Snap.Genshin.Website.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GenshinItemsController : ControllerBase
    {
        public GenshinItemsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext; 
        }

        private readonly ApplicationDbContext dbContext;

        public record ItemInfo([Required]int Id, [Required]string Name, [Required]string Url);
        public record UploadModel(IEnumerable<ItemInfo> Avatars, IEnumerable<ItemInfo> Weapons, IEnumerable<ItemInfo> Reliquaries);

        const string AvatarKey = "Avatar";
        const string WeaponKey = "Weapon";
        const string ReliquaryKey = "Reliquary";

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromBody]UploadModel request)
        {
            if (string.IsNullOrEmpty(Request.Headers.Authorization)) return Unauthorized();
            AddItemToDb(request.Avatars, AvatarKey);
            AddItemToDb(request.Weapons, WeaponKey);
            AddItemToDb(request.Reliquaries, ReliquaryKey);

            await dbContext.SaveChangesAsync();

            return this.Success($"数据上传成功");
        }

        [HttpGet("[action]")]
        public IActionResult Avatars() => this.Success("角色数据查询成功", ReadItemsFromDb(AvatarKey));

        [HttpGet("[action]")]
        public IActionResult Weapons() => this.Success("武器数据查询成功", ReadItemsFromDb(WeaponKey));

        [HttpGet("[action]")]
        public IActionResult Reliquaries() => this.Success("圣遗物数据查询成功", ReadItemsFromDb(ReliquaryKey));

        private void AddItemToDb(IEnumerable<ItemInfo> items, string type)
        {
            foreach (var item in items)
            {
                if (dbContext.GenshinItems.Any(x => item.Id == x.Id && x.Type == type)) continue;
                dbContext.GenshinItems.Add(new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Url = item.Url,
                    Type = type
                });
            }
        }

        // TODO 缓存优化
        private IEnumerable<ItemInfo> ReadItemsFromDb(string type)
            => dbContext.GenshinItems.Where(item => item.Type == type).Select(item => new ItemInfo(item.Id, item.Name, item.Url));
    }
}
