
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class GroupsController : Controller
{
    private readonly DanceStudioContext _context;

    public GroupsController(DanceStudioContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var groups = await _context.Groups
            .Include(g => g.Style)
            .Include(g => g.AgeCategory)
            .Include(g => g.Coach)  // ← підтягуємо тренера
            .ToListAsync();
        
        return View(groups);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Group group)
    {
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
