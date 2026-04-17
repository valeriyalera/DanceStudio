using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace DanceStudio.Infrastructure.Controllers;

[Authorize(Roles = "Admin,Coach")]
public class GroupsController : Controller
{
    private readonly DanceStudioContext _context;

    public GroupsController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: Groups
    public async Task<IActionResult> Index()
    {
        var groups = await _context.Groups
            .Include(g => g.Style)
            .Include(g => g.AgeCategory)
            .Include(g => g.Coach)
            .ToListAsync();

        return View(groups);
    }

    // GET: Groups/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Groups/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Group group)
    {
        if (ModelState.IsValid)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(group);
    }

    // GET: Groups/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    // POST: Groups/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Group group)
    {
        if (id != group.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(group);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(group.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(group);
    }

    // GET: Groups/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var group = await _context.Groups
            .Include(g => g.Style)
            .Include(g => g.AgeCategory)
            .Include(g => g.Coach)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    // GET: Groups/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var group = await _context.Groups
            .Include(g => g.Style)
            .Include(g => g.AgeCategory)
            .Include(g => g.Coach)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    // POST: Groups/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group != null)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool GroupExists(int id)
    {
        return _context.Groups.Any(e => e.Id == id);
    }
}