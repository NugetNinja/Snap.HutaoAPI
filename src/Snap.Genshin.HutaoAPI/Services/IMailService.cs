// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Models.Utility;

namespace Snap.HutaoAPI.Services
{
    /// <summary>
    /// Mail service
    /// </summary>
    public interface IMailService
    {
        void SendEmail(IMail mail);
    }
}
