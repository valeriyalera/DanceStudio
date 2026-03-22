using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class SchedulesController : Controller
{
    private readonly DanceStudioContext _context;

    public SchedulesController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: Schedules
    public async Task<IActionResult> Index()
    {
        var schedules = await _context.Schedules
            .Include(s => s.Group)                    // зв'язок з Group (M:1)
                .ThenInclude(g => g.Style)            // група → стиль
            .Include(s => s.Group.Coach)              // група → тренер
            .Include(s => s.Group.AgeCategory)        // група → вікова категорія
            .Include(s => s.Bookings)                 // зв'язок з Bookings (1:M)
            .Include(s => s.Cancellations)            // зв'язок з Cancellations (1:M)
            .ToListAsync();
        
        return View(schedules);
    }

    // GET: Schedules/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Groups = await _context.Groups
            .Include(g => g.Style)
            .Include(g => g.Coach)
            .ToListAsync();
        ViewBag.Days = new List<string> { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
        return View();
    }

    // POST: Schedules/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Schedule schedule)
    {
        if (ModelState.IsValid)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Groups = await _context.Groups.ToListAsync();
        ViewBag.Days = new List<string> { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
        return View(schedule);
    }

    // GET: Schedules/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule == null)
        {
            return NotFound();
        }
        
        ViewBag.Groups = await _context.Groups
            .Include(g => g.Style)
            .Include(g => g.Coach)
            .ToListAsync();
        ViewBag.Days = new List<string> { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
        
        return View(schedule);
    }

    // POST: Schedules/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Schedule schedule)
    {
        if (id != schedule.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schedule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(schedule.Id))
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
        
        ViewBag.Groups = await _context.Groups.ToListAsync();
        ViewBag.Days = new List<string> { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
        return View(schedule);
    }

    // GET: Schedules/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Group)
                .ThenInclude(g => g.Style)
            .Include(s => s.Group.Coach)
            .Include(s => s.Group.AgeCategory)
            .Include(s => s.Bookings)
                .ThenInclude(b => b.User)
            .Include(s => s.Cancellations)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (schedule == null)
        {
            return NotFound();
        }

        return View(schedule);
    }

    // GET: Schedules/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Group)
                .ThenInclude(g => g.Style)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (schedule == null)
        {
            return NotFound();
        }

        return View(schedule);
    }

    // POST: Schedules/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ScheduleExists(int id)
    {
        return _context.Schedules.Any(e => e.Id == id);
    }
}