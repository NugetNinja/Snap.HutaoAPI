// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI;

/// <summary>
/// 应用程序扩展
/// </summary>
public static class AppExtensions
{
    /// <summary>
    /// 在错误的环境下退出
    /// </summary>
    /// <param name="builder">构建器</param>
    /// <returns>可继续操作的构建器</returns>
    public static WebApplicationBuilder ExitOnWrongEnvironment(this WebApplicationBuilder builder)
    {
#if DEBUG
        if (!builder.Environment.IsDevelopment())
        {
            Console.WriteLine("panic: only in Development envirenment can you run DEBUG verison.");
            Environment.Exit(Environment.ExitCode);
        }
#endif

#if RELEASE
        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine("panic: only in Production envirenment can you run RELEASE verison.");
            Environment.Exit(Environment.ExitCode);
        }
#endif
        return builder;
    }
}
