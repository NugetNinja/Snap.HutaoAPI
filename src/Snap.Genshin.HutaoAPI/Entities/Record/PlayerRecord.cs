// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record
{
    /// <summary>
    /// 玩家记录
    /// </summary>
    public class PlayerRecord
    {
        /// <summary>
        /// 玩家记录 id from db
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long InnerId { get; set; }

        /// <summary>
        /// 记录上传日期
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UploadTime { get; set; }

        // TODO: 记录上传用户

        /// <summary>
        /// 外键
        /// </summary>
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; } = null!;

        /// <summary>
        /// 外键 记录了上传用户的innerid
        /// </summary>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// 外键 深渊层数
        /// </summary>
        public IList<SpiralAbyssLevel> SpiralAbyssLevels { get; set; } = null!;
    }
}
