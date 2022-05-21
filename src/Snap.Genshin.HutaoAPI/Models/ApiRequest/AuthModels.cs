// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Snap.HutaoAPI.Models.ApiRequest
{
    public class LoginModel
    {
        [Required]
        public Guid AppId { get; set; }

        [Required]
        public string Secret { get; set; } = null!;
    }

    public class RegisterModel
    {
        [Required]
        public string AppName { get; set; } = null!;

        [Required]
        public string Signature { get; set; } = null!;

        [Required, RegularExpression(@"\d{6}")]
        public string Code { get; set; } = null!;
    }

    public class EmailVerifyModel
    {
        [MaxLength(40)]
        public string AppName { get; set; } = null!;
    }
}
