using SimpleProject.Models;
using SimpleProject.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;

var builder = WebApplication.CreateBuilder(args);

// Make the app run as a Windows Service
//builder.Host.UseWindowsService();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IGalleryService, GalleryService>();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHealthChecks("/health");
app.MapGet("/health/ready", () => Results.Ok("Ready"));
app.MapGet("/runtime-user", () =>
{
    return Results.Ok(new
    {
        User = Environment.UserName,
        IsRoot = Environment.UserName == "root"
    });
});
app.Run();
