// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Models.Utility;
using System.ComponentModel.DataAnnotations;

namespace Snap.HutaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GenshinItemsController : ControllerBase
    {
        private const string AvatarKey = "Avatar";
        private const string WeaponKey = "Weapon";
        private const string ReliquaryKey = "Reliquary";

        public GenshinItemsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly ApplicationDbContext dbContext;

        public record ItemInfo([Required] int Id, [Required] string Name, [Required] string Url);

        public record UploadModel(IEnumerable<ItemInfo> Avatars, IEnumerable<ItemInfo> Weapons, IEnumerable<ItemInfo> Reliquaries);

        /// <summary>
        /// 上传角色与武器与圣遗物映射数据
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <returns>上传的结果</returns>
        [HttpPost("[action]")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ApiExplorerSettings(GroupName = "v3")]
        public async Task<IActionResult> Upload([FromBody] UploadModel request)
        {
            AddItemToDb(request.Avatars.DistinctBy(item => item.Id), AvatarKey);
            AddItemToDb(request.Weapons.DistinctBy(item => item.Id), WeaponKey);
            AddItemToDb(request.Reliquaries.DistinctBy(item => item.Id), ReliquaryKey);

            await dbContext.SaveChangesAsync();

            return this.Success($"数据上传成功");
        }

        /// <summary>
        /// 获取角色映射列表
        /// </summary>
        /// <returns>角色映射列表</returns>
        [HttpGet("[action]")]
        [ApiExplorerSettings(GroupName = "v3")]
        public IActionResult Avatars()
        {
            return this.Success("角色数据查询成功", ReadItemsFromDb(AvatarKey));
        }

        /// <summary>
        /// 获取武器映射列表
        /// </summary>
        /// <returns>武器映射列表</returns>
        [HttpGet("[action]")]
        [ApiExplorerSettings(GroupName = "v3")]
        public IActionResult Weapons()
        {
            return this.Success("武器数据查询成功", ReadItemsFromDb(WeaponKey));
        }

        /// <summary>
        /// 获取圣遗物映射列表
        /// </summary>
        /// <returns>圣遗物映射列表</returns>
        [HttpGet("[action]")]
        [ApiExplorerSettings(GroupName = "v3")]
        public IActionResult Reliquaries()
        {
            return this.Success("圣遗物数据查询成功", ReadItemsFromDb(ReliquaryKey));
        }

        private void AddItemToDb(IEnumerable<ItemInfo> uploadedItems, string type)
        {
            foreach (ItemInfo? uploadItem in uploadedItems)
            {
                if (dbContext.GenshinItems.Any(x => uploadItem.Id == x.Id && x.Type == type))
                {
                    continue;
                }

                dbContext.GenshinItems.Add(new()
                {
                    Id = uploadItem.Id,
                    Name = uploadItem.Name,
                    Url = uploadItem.Url,
                    Type = type
                });
            }
        }

        // TODO 缓存优化
        private IEnumerable<ItemInfo> ReadItemsFromDb(string type)
        {
            return dbContext.GenshinItems
                .Where(item => item.Type == type)
                .Select(item => new ItemInfo(item.Id, item.Name, item.Url));
        }
    }
}
