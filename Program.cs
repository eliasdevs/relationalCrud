using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using TestJoin1.DataAccess;
//use serilog to log events

var builder = WebApplication.CreateBuilder(args);
//configurar selilog
builder.Host.UseSerilog((hostBuilderCtx, loggerConf) =>
{
    loggerConf.WriteTo.Console().
                WriteTo.Debug().
                ReadFrom.Configuration(hostBuilderCtx.Configuration);
});
// Add services to the container.
//Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllers();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TestJoinContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

var app = builder.Build();
//2. SUPPORT CULTURES
var supportedCulteres = new[] { "en-US", "es-ES"};
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCulteres[0])
    .AddSupportedCultures(supportedCulteres)
    .AddSupportedUICultures(supportedCulteres);
//3. Add localization
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//Tell app to use Serilog
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
