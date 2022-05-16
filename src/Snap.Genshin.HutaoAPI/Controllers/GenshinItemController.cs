// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Models.Utility;
using System.ComponentModel.DataAnnotations;

namespace Snap.HutaoAPI.Controllers
{
    /// <summary>
    /// Item controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class GenshinItemController : ControllerBase
    {
        private const string AvatarKey = "Avatar";
        private const string WeaponKey = "Weapon";
        private const string ReliquaryKey = "Reliquary";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">dbcontext from di</param>
        /// <param name="cache">cache from di</param>
        public GenshinItemController(ApplicationDbContext dbContext, IMemoryCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        private readonly ApplicationDbContext dbContext;

        private readonly IMemoryCache cache;

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
                var isInCache = this.cache.TryGetValue($"_ITEM_UPLOAD_{uploadItem.Id}_{type}_", out _);

                if (isInCache)
                {
                    continue;
                }

                if (dbContext.GenshinItems.Any(x => uploadItem.Id == x.Id && x.Type == type))
                {
                    continue;
                }

                dbContext.GenshinItems.Add(new()
                {
                    Id = uploadItem.Id,
                    Name = uploadItem.Name,
                    Url = uploadItem.Url,
                    Type = type,
                });
                dbContext.SaveChanges();

                this.cache.Set($"_ITEM_UPLOAD_{uploadItem.Id}_{type}_", uploadItem);

                var isTypeInCache = this.cache.TryGetValue($"_ITEM_TYPE_UPLOAD_{type}_", out HashSet<ItemInfo> s);
                if (!isTypeInCache || !s.Any())
                {
                    s = new()
                    {
                        uploadItem,
                    };
                }
                else
                {
                    s.Add(uploadItem);
                }
            }
        }

        // TODO 缓存优化
        private IEnumerable<ItemInfo> ReadItemsFromDb(string type)
        {
            return this.cache.TryGetValue($"_ITEM_TYPE_UPLOAD_{type}_", out HashSet<ItemInfo> item) ?
                item.ToList() :
                dbContext.GenshinItems
                .Where(item => item.Type == type)
                .Select(item => new ItemInfo(item.Id, item.Name, item.Url));
        }
    }
}
