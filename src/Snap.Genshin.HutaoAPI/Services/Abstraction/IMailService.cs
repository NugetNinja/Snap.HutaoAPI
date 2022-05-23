// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Models.Identity;

namespace Snap.HutaoAPI.Services.Abstraction
{
    /// <summary>
    /// Mail service
    /// </summary>
    public interface IMailService
    {
        void SendEmail(IMail mail);
    }
}
