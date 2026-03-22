using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class CancellationsController : Controller
{
    private readonly DanceStudioContext _context;

    public CancellationsController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: Cancellations
    public async Task<IActionResult> Index()
    {
        var cancellations = await _context.Cancellations
            .Include(c => c.Coach)              // зв'язок з Coach (M:1)
            .Include(c => c.Schedule)           // зв'язок з Schedule (M:1)
                .ThenInclude(s => s.Group)      // Schedule → Group
                    .ThenInclude(g => g.Style)  // Group → Style
            .Include(c => c.Schedule.Group.Coach) // Group → Coach
            .ToListAsync();
        
        return View(cancellations);
    }

    // GET: Cancellations/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Coaches = await _context.Coaches.ToListAsync();
        ViewBag.Schedules = await _context.Schedules
            .Include(s => s.Group)
                .ThenInclude(g => g.Style)
            .ToListAsync();
        
        ViewBag.Reasons = new List<string> { "Хвороба", "Особисті обставини", "Технічні причини", "Відпустка", "Інше" };
        
        return View();
    }

    // POST: Cancellations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cancellation cancellation)
    {
        if (ModelState.IsValid)
        {
            _context.Cancellations.Add(cancellation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Coaches = await _context.Coaches.ToListAsync();
        ViewBag.Schedules = await _context.Schedules.ToListAsync();
        ViewBag.Reasons = new List<string> { "Хвороба", "Особисті обставини", "Технічні причини", "Відпустка", "Інше" };
        return View(cancellation);
    }

    // GET: Cancellations/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var cancellation = await _context.Cancellations.FindAsync(id);
        if (cancellation == null)
        {
            return NotFound();
        }
        
        ViewBag.Coaches = await _context.Coaches.ToListAsync();
        ViewBag.Schedules = await _context.Schedules
            .Include(s => s.Group)
            .ToListAsync();
        ViewBag.Reasons = new List<string> { "Хвороба", "Особисті обставини", "Технічні причини", "Відпустка", "Інше" };
        
        return View(cancellation);
    }

    // POST: Cancellations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cancellation cancellation)
    {
        if (id != cancellation.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cancellation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CancellationExists(cancellation.Id))
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
        
        ViewBag.Coaches = await _context.Coaches.ToListAsync();
        ViewBag.Schedules = await _context.Schedules.ToListAsync();
        ViewBag.Reasons = new List<string> { "Хвороба", "Особисті обставини", "Технічні причини", "Відпустка", "Інше" };
        return View(cancellation);
    }

    // GET: Cancellations/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var cancellation = await _context.Cancellations
            .Include(c => c.Coach)
            .Include(c => c.Schedule)
                .ThenInclude(s => s.Group)
                    .ThenInclude(g => g.Style)
            .Include(c => c.Schedule.Group.Coach)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (cancellation == null)
        {
            return NotFound();
        }

        return View(cancellation);
    }

    // GET: Cancellations/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var cancellation = await _context.Cancellations
            .Include(c => c.Coach)
            .Include(c => c.Schedule)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (cancellation == null)
        {
            return NotFound();
        }

        return View(cancellation);
    }

    // POST: Cancellations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cancellation = await _context.Cancellations.FindAsync(id);
        if (cancellation != null)
        {
            _context.Cancellations.Remove(cancellation);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool CancellationExists(int id)
    {
        return _context.Cancellations.Any(e => e.Id == id);
    }
}