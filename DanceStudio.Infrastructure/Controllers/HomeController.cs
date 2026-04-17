
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DanceStudio.Domain.Model;

namespace DanceStudio.Infrastructure.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> ForceLoginAsAdmin()
    {
        var user = await _userManager.FindByEmailAsync("admin@dance.com");
        if (user != null)
        {
            // Додаємо SecurityStamp, якщо його немає
            if (string.IsNullOrEmpty(user.SecurityStamp))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                await _userManager.UpdateAsync(user);
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        return Content("Адміністратор не знайдений");
    }

    public async Task<IActionResult> TestRoles()
    {
        var message = "";
        if (User.Identity.IsAuthenticated)
        {
            message = $"Ви увійшли як: {User.Identity.Name}<br/>";
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                message += $"Ролі: {string.Join(", ", roles)}";
            }
        }
        else
        {
            message = "Ви не увійшли в систему. Перейдіть на /Account/Login для входу";
        }
        return Content(message);
    }
}
