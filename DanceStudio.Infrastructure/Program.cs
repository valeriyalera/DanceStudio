using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using DanceStudio.Infrastructure;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Додаємо підключення до PostgreSQL (основна БД)
builder.Services.AddDbContext<DanceStudioContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppConnection")));

// Identity контекст (окрема БД для логінів/паролів)
builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityConnection")));

// Додаємо Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

// Налаштування максимального розміру файлу для імпорту
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

// Реєстрація фабрики для імпорту/експорту стилів
builder.Services.AddScoped<IDataPortServiceFactory<Style>, StyleDataPortServiceFactory>();

var app = builder.Build();

// Ініціалізація ролей і адміністратора
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleInitializer.InitializeAsync(userManager, roleManager);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // ← ДОДАТИ для Identity
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