using Microsoft.EntityFrameworkCore;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Configurations;

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
