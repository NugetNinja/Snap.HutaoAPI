using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Snap.Genshin.Website.Configurations;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services.StatisticCalculation;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Environment Check
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
#endregion

#region Service Injections

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    if (builder.Environment.IsDevelopment())
        opt.UseMySql(builder.Configuration.GetConnectionString("LocalDb"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("LocalDb")));
    else
        opt.UseMySql(builder.Configuration.GetConnectionString("ProductDb"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ProductDb")));
});

builder.Services.AddGenshinStatisticsService(opt =>
{
    opt.AddCalculator<OverviewDataCalculator>()
       .AddCalculator<AvatorParticipationCaltulator>()
       .AddCalculator<TeamCollocationCalculator>()
       .AddCalculator<WeaponUsageCalculator>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("SecurityKey")))
        };
    });

builder.Services.AddTokenFactory(options =>
{
    var config = builder.Configuration.GetSection("Jwt");
    options.Issuer = config.GetValue<string>("Issuer");
    options.Audience = config.GetValue<string>("Audience");
    options.SigningKey = config.GetValue<string>("SecurityKey");
    options.AccessTokenExpire = config.GetValue<int>("AccessTokenExpire");
    options.RefreshTokenExpire = config.GetValue<int>("RefreshTokenExpire");
    options.RefreshTokenBefore = config.GetValue<int>("RefreshTokenBefore");
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

#endregion

var app = builder.Build();

#region Pipeline Configuration

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
