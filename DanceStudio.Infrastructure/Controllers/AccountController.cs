using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure.ViewModels;

namespace DanceStudio.Infrastructure.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    Console.WriteLine($"=== СПРОБА ВХОДУ: {model.Email} ===");
    
    if (ModelState.IsValid)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        Console.WriteLine($"РЕЗУЛЬТАТ: {result.Succeeded}");
        
        if (result.Succeeded)
        {
            Console.WriteLine("ПЕРЕНАПРАВЛЕННЯ НА HOME");
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Неправильний логін або пароль");
    }
    return View(model);
}

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> CreateAdmin()
    {
        var email = "admin@dance.com";
        var password = "Admin_123";
        
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new AppUser { UserName = email, Email = email, FirstName = "Admin", LastName = "Admin" };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return Content("Адміністратор створений! Пароль: Admin_123");
            }
            return Content($"Помилка: {string.Join(", ", result.Errors)}");
        }
        return Content("Адміністратор вже існує");
    }
}