using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Entities.Record;
using Snap.Genshin.Website.Models;
using Snap.Genshin.Website.Models.Utility;

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

        /// <summary>
        /// 检查用户是否上传了当期记录
        /// </summary>
        /// <param name="uid">用户uid</param>
        /// <returns>结果</returns>
        [HttpGet("[Action]/{uid}")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ApiExplorerSettings(GroupName = "v1")]
        public IActionResult CheckRecord([FromRoute] string uid)
        {
            var playerQuery = dbContext.Players.Where(player => player.Uid == uid);
            if (!playerQuery.Any())
            {
                return this.Success("查询成功", new { PeriodUploaded = false });
            }

            var player = playerQuery.Single();
            var recordQuery = dbContext.PlayerRecords.Where(record => record.PlayerId == player.InnerId);

            return this.Success("查询成功", new { PeriodUploaded = recordQuery.Any() });
        }

        /// <summary>
        /// 上传记录
        /// </summary>
        /// <param name="record">记录</param>
        /// <returns>结果</returns>
        [HttpPost("Upload")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> UploadRecord([FromBody] Models.SnapGenshin.PlayerRecord record)
        {
            Player? player = dbContext.Players
                .Where(player => player.Uid == record.Uid)
                .Include(player => player.Avatars)
                .SingleOrDefault();

            if (player is null)
            {
                player = new Player()
                {
                    Uid = record.Uid,
                    Avatars = new List<AvatarDetail>(),
                };
                dbContext.Players.Add(player);
            }

            player.Avatars.Clear();

            IEnumerable<AvatarDetail>? newAvatars = record.PlayerAvatars
                .Select(avatar => new AvatarDetail()
                {
                    AvatarId = avatar.Id,
                    AvatarLevel = avatar.Level,
                    WeaponId = avatar.Weapon.Id,
                    WeaponLevel = avatar.Weapon.Level,
                    AffixLevel = avatar.Weapon.AffixLevel,
                    ActivedConstellationNum = avatar.ActivedConstellationNum,
                    ReliquarySets = avatar.ReliquarySets
                    .Select(r => new ReliquarySetDetail()
                    {
                        Id = r.Id,
                        Count = r.Count,
                        UnionId = $"{r.Id}-{r.Count}",
                    }).ToList(),
                });
            player.Avatars = newAvatars.ToList();

            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            // 删除旧记录
            PlayerRecord? oldPlayerRecord = dbContext.PlayerRecords
                .Where(record => record.PlayerId == player.InnerId)
                .FirstOrDefault();

            if (oldPlayerRecord is not null)
            {
                dbContext.PlayerRecords.Remove(oldPlayerRecord);
            }

            // 插入新记录
            dbContext.PlayerRecords.Add(new PlayerRecord()
            {
                PlayerId = player.InnerId,
                SpiralAbyssLevels = record.PlayerSpiralAbyssesLevels
                .Select(level => new SpiralAbyssLevel
                {
                    FloorIndex = level.FloorIndex,
                    LevelIndex = level.LevelIndex,
                    Star = level.Star,
                    Battles = level.Battles
                    .Select(battle => new SpiralAbyssBattle
                    {
                        Avatars = battle.AvatarIds
                            .Select(avatar => new SpiralAbyssAvatar { AvatarId = avatar })
                            .ToList(),
                        BattleIndex = battle.BattleIndex,
                    }).ToList(),
                }).ToList(),
            });

            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return this.Success($"UID：{record.Uid}的数据上传成功");
        }
    }
}
