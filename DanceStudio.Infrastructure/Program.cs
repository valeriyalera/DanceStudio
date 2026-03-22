using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Додаємо підключення до PostgreSQL
builder.Services.AddDbContext<DanceStudioContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

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


if (app.Environment.IsDevelopment())
{
    var url = "http://localhost:5075"; 
    
    Task.Delay(1000).ContinueWith(_ =>
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось открыть браузер: {ex.Message}");
        }
    });
}

app.Run();