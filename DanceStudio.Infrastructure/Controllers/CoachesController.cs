
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class CoachesController : Controller
{
    private readonly DanceStudioContext _context;

    public CoachesController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: Coaches
    public async Task<IActionResult> Index()
    {
        var coaches = await _context.Coaches
            .Include(c => c.Bookings)
            .Include(c => c.Cancellations)
            .ToListAsync();
        
        return View(coaches);
    }

    // GET: Coaches/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Coaches/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Coach coach)
    {
        if (ModelState.IsValid)
        {
            _context.Coaches.Add(coach);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(coach);
    }

    // GET: Coaches/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var coach = await _context.Coaches.FindAsync(id);
        if (coach == null)
        {
            return NotFound();
        }
        return View(coach);
    }

    // POST: Coaches/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Coach coach)
    {
        if (id != coach.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(coach);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoachExists(coach.Id))
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
        return View(coach);
    }

    // GET: Coaches/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var coach = await _context.Coaches
            .Include(c => c.Bookings)
                .ThenInclude(b => b.User)
            .Include(c => c.Bookings)
                .ThenInclude(b => b.Schedule)
            .Include(c => c.Cancellations)
                .ThenInclude(ca => ca.Schedule)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (coach == null)
        {
            return NotFound();
        }

        return View(coach);
    }

    // GET: Coaches/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var coach = await _context.Coaches.FindAsync(id);
        if (coach == null)
        {
            return NotFound();
        }

        return View(coach);
    }

    // POST: Coaches/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var coach = await _context.Coaches.FindAsync(id);
        if (coach != null)
        {
            _context.Coaches.Remove(coach);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool CoachExists(int id)
    {
        return _context.Coaches.Any(e => e.Id == id);
    }
}

