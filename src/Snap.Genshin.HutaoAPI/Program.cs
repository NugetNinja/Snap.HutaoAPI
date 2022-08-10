﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Snap.HutaoAPI;
using Snap.HutaoAPI.Configurations;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Job;
using Snap.HutaoAPI.Models.Identity;
using Snap.HutaoAPI.Services;
using Snap.HutaoAPI.Services.Abstraction;
using Snap.HutaoAPI.Services.ParallelCalculation;
using Snap.HutaoAPI.Services.StatisticCalculation;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.ExitOnWrongEnvironment();

var services = builder.Services;

services
    .AddControllers()
    .Services
    .AddMemoryCache()
    .AddDbContext<ApplicationDbContext>(optionBuilder =>
    {
        var dbType = builder.Environment.IsDevelopment() ? "LocalDb" : "ProductDb";
        var connectionString = builder.Configuration.GetConnectionString(dbType);

        optionBuilder
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .ConfigureWarnings(b => b.Log(
                (RelationalEventId.CommandExecuted, LogLevel.Debug),
                (CoreEventId.ContextInitialized, LogLevel.Debug)));
    })
    .AddScoped<IStatisticsProvider, StatisticsProvider>()
    .AddGenshinStatistics(config => config
        .AddCalculator<OverviewDataCalculator>()
        .AddCalculator<AvatarParticipationCalculator>()
        .AddCalculator<AvatarParticipation2Calculator>()
        .AddCalculator<TeamCollocationCalculator>()
        .AddCalculator<WeaponUsageCalculator>()
        .AddCalculator<AvatarReliquaryUsageCalculator>()
        .AddCalculator<ActivedConstellationNumCalculator>()
        .AddCalculator<TeamCombinationForFloorAndLevelCalculator>()
        .AddCalculator<TeamCombinationForFloorCalculator>())
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            ValidateIssuerSigningKey = true,
            ValidAudience = jwtConfig.GetValue<string>("Audience"),
            ValidIssuer = jwtConfig.GetValue<string>("Issuer"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("SecurityKey"))),
        };
    })
    .Services

    // 鉴权策略
    .AddAuthorization(options =>
    {
        options.AddPolicy(IdentityPolicyNames.CommonUser, policy =>
        {
            policy.RequireClaim(ClaimTypes.NameIdentifier);
            policy.RequireClaim("TokenType", "AccessToken");
        });
        options.AddPolicy(IdentityPolicyNames.Administrator, policy =>
        {
            policy.RequireClaim(ClaimTypes.NameIdentifier);
            policy.RequireClaim("Administrator", "sg-admin");
        });
        options.AddPolicy(IdentityPolicyNames.RefreshTokenOnly, policy =>
        {
            policy.RequireClaim(ClaimTypes.NameIdentifier);
            policy.RequireClaim("TokenType", "RefreshToken");
        });
    })
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new()
        {
            Version = "1.0.0.0",
            Title = "记录交互",
            Description = "提交记录，查询提交状态",
        });
        c.SwaggerDoc("v2", new()
        {
            Version = "1.0.0.0",
            Title = "数据详情",
            Description = "获取详细的纵深数据",
        });
        c.SwaggerDoc("v4", new()
        {
            Version = "1.0.0.0",
            Title = "数据详情2",
            Description = "获取更详细的纵深数据",
        });
        c.SwaggerDoc("v3", new()
        {
            Version = "1.0.0.0",
            Title = "物品信息",
            Description = "提交与获取物品Id映射",
        });
        c.SwaggerDoc("v5", new()
        {
            Version = "1.0.0.0",
            Title = "角色展柜",
            Description = "获取玩家角色展柜中的信息",
        });

        // We only have one executable file so it's fine.
        string xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    })

    // 计划任务
    .AddQuartz(config =>
    {
        config
            .UseMicrosoftDependencyInjectionJobFactory();

        config
            .ScheduleJob<StatisticsRefreshJob>(trigger =>
            {
                trigger
                    .StartNow()
                    .WithCronSchedule("0 5 */1 * * ?");
            })
            .ScheduleJob<StatisticsClearJob>(trigger =>
            {
                trigger
                    .StartNow()
                    .WithCronSchedule("0 0 4 1,16 * ?");
            });
    })
    .AddTransient<StatisticsRefreshJob>()
    .AddTransient<StatisticsClearJob>()
    .AddQuartzServer(options =>
    {
        options.WaitForJobsToComplete = true;
    })

    // CORS Policy
    .AddCors(options => options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowCredentials();
    }));

WebApplication app = builder.Build();

app
    .UseSwagger()
    .UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/v1/swagger.json", "记录交互 API");
        option.SwaggerEndpoint("/swagger/v2/swagger.json", "数据详情 API");
        option.SwaggerEndpoint("/swagger/v4/swagger.json", "数据详情2 API");
        option.SwaggerEndpoint("/swagger/v3/swagger.json", "物品信息 API");
        option.SwaggerEndpoint("/swagger/v5/swagger.json", "角色展柜 API");
    })
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

app.Run();