// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Extension;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Snap.HutaoAPI.Controllers;

/// <summary>
/// 物品接口控制器
/// </summary>
[Route("[controller]")]
[ApiController]
public class GenshinItemController : ControllerBase
{
    private const string AvatarKey = "Avatar";
    private const string WeaponKey = "Weapon";
    private const string ReliquaryKey = "Reliquary";

    private readonly ApplicationDbContext dbContext;
    private readonly IMemoryCache cache;

    /// <summary>
    /// 构造一个新的物品接口控制器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="cache">内存缓存</param>
    public GenshinItemController(ApplicationDbContext dbContext, IMemoryCache cache)
    {
        this.dbContext = dbContext;
        this.cache = cache;
    }

    /// <summary>
    /// 上传角色与武器与圣遗物映射数据
    /// </summary>
    /// <param name="request">请求数据</param>
    /// <returns>上传的结果</returns>
    [HttpPost("[action]")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ApiExplorerSettings(GroupName = "v3")]
    public async Task<IActionResult> Upload([FromBody] UploadItemWrapper request)
    {
        if (request.Validate())
        {
            AddItemToDb(request.Avatars.DistinctBy(item => item.Id), AvatarKey);
            AddItemToDb(request.Weapons.DistinctBy(item => item.Id), WeaponKey);
            AddItemToDb(request.Reliquaries.DistinctBy(item => item.Id), ReliquaryKey);

            await dbContext.SaveChangesAsync();

            return this.Success("数据上传成功");
        }

        return this.Fail("数据包含无效的Id");
    }

    /// <summary>
    /// 获取角色映射列表
    /// </summary>
    /// <returns>角色映射列表</returns>
    [HttpGet("[action]")]
    [ApiExplorerSettings(GroupName = "v3")]
    public IActionResult Avatars()
    {
        return this.Success("角色数据查询成功", ReadItemsFromCacheOrDb(AvatarKey));
    }

    /// <summary>
    /// 获取武器映射列表
    /// </summary>
    /// <returns>武器映射列表</returns>
    [HttpGet("[action]")]
    [ApiExplorerSettings(GroupName = "v3")]
    public IActionResult Weapons()
    {
        return this.Success("武器数据查询成功", ReadItemsFromCacheOrDb(WeaponKey));
    }

    /// <summary>
    /// 获取圣遗物映射列表
    /// </summary>
    /// <returns>圣遗物映射列表</returns>
    [HttpGet("[action]")]
    [ApiExplorerSettings(GroupName = "v3")]
    public IActionResult Reliquaries()
    {
        return this.Success("圣遗物数据查询成功", ReadItemsFromCacheOrDb(ReliquaryKey));
    }

    private void AddItemToDb(IEnumerable<SimpleItemInfo> uploadedItems, string type)
    {
        foreach (SimpleItemInfo? uploadItem in uploadedItems)
        {
            string idTypeKey = $"_ITEM_UPLOAD_{uploadItem.Id}_{type}_";

            // 缓存了该物品
            if (cache.Has(idTypeKey))
            {
                continue;
            }

            // 数据库中存在该物品
            if (dbContext.GenshinItems.Any(x => uploadItem.Id == x.Id && x.Type == type))
            {
                continue;
            }

            dbContext.GenshinItems.Add(uploadItem.Complexify(type));
            cache.Set(idTypeKey, uploadItem);

            string typeKey = $"_ITEM_TYPE_UPLOAD_{type}_";
            bool isTypeInCache = cache.TryGetValue(typeKey, out HashSet<SimpleItemInfo>? typeHashSet);
            if (isTypeInCache && typeHashSet!.Any())
            {
                typeHashSet!.Add(uploadItem);
            }
            else
            {
                HashSet<SimpleItemInfo> cacheSet = InitializeTypeAsCached(type);

                // try put in first item
                cacheSet.Add(uploadItem);
                cache.Set(typeKey, cacheSet);
            }
        }

        dbContext.SaveChanges();
    }

    private List<SimpleItemInfo> ReadItemsFromCacheOrDb(string type)
    {
        return cache.TryGetValue($"_ITEM_TYPE_UPLOAD_{type}_", out HashSet<SimpleItemInfo> item)
            ? item.ToList()
            : InitializeTypeAsCached(type).ToList();
    }

    private HashSet<SimpleItemInfo> InitializeTypeAsCached(string type)
    {
        IEnumerable<SimpleItemInfo> cacheItems = dbContext.GenshinItems
            .Where(item => item.Type == type)
            .Select(item => SimpleItemInfo.Simplify(item));
        return cache.Set($"_ITEM_TYPE_UPLOAD_{type}_", new HashSet<SimpleItemInfo>(cacheItems));
    }

    /// <summary>
    /// 简单物品信息
    /// record 允许此类存入 <see cref="HashSet{T}"/> 时互斥
    /// </summary>
    public record SimpleItemInfo
    {
        /// <summary>
        /// 构造一个新的简单物品信息
        /// </summary>
        /// <param name="id">物品id</param>
        /// <param name="name">名称</param>
        /// <param name="url">图片链接</param>
        public SimpleItemInfo(int id, string name, string url)
        {
            Id = id;
            Name = name;
            Url = url;
        }

        /// <summary>
        /// 物品id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        /// 转换到简单物品信息
        /// </summary>
        /// <param name="itemInfo">数据库中的物品信息</param>
        /// <returns>简单物品信息</returns>
        public static SimpleItemInfo Simplify(ItemInfo itemInfo)
        {
            return new SimpleItemInfo(itemInfo.Id, itemInfo.Name, itemInfo.Url);
        }

        /// <summary>
        /// 转换到数据库物品信息
        /// </summary>
        /// <param name="type">物品类型</param>
        /// <returns>复杂物品信息</returns>
        public ItemInfo Complexify(string type)
        {
            return new()
            {
                Id = Id,
                Name = Name,
                Url = Url,
                Type = type,
            };
        }
    }

    /// <summary>
    /// 上传物品包装器
    /// </summary>
    public class UploadItemWrapper
    {
        /// <summary>
        /// 角色
        /// </summary>
        public IEnumerable<SimpleItemInfo> Avatars { get; set; } = default!;

        /// <summary>
        /// 武器
        /// </summary>
        public IEnumerable<SimpleItemInfo> Weapons { get; set; } = default!;

        /// <summary>
        /// 圣遗物
        /// </summary>
        public IEnumerable<SimpleItemInfo> Reliquaries { get; set; } = default!;

        /// <summary>
        /// 验证有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool Validate()
        {
            if (Avatars is null || Weapons is null || Reliquaries is null)
            {
                return false;
            }

            // 8 位 Id
            if (Avatars.Any(x => x.Id.Place() != 8)
                || Weapons.Any(x => x.Id.Place() != 5)
                || Reliquaries.Any(x => x.Id.Place() != 7))
            {
                return false;
            }

            return true;
        }
    }
}