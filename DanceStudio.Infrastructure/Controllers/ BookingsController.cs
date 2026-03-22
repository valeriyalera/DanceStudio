using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class BookingsController : Controller
{
    private readonly DanceStudioContext _context;

    public BookingsController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: Bookings
    public async Task<IActionResult> Index()
    {
        var bookings = await _context.Bookings
            .Include(b => b.User)              // зв'язок з User (M:1)
            .Include(b => b.Schedule)          // зв'язок з Schedule (M:1)
                .ThenInclude(s => s.Group)     // Schedule → Group
                    .ThenInclude(g => g.Style) // Group → Style
            .Include(b => b.Schedule.Group.Coach)  // Group → Coach
            .Include(b => b.Schedule.Group.AgeCategory) // Group → AgeCategory
            .Include(b => b.Coach)             // зв'язок з Coach (M:1)
            .ToListAsync();
        
        return View(bookings);
    }

    // GET: Bookings/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Users = await _context.Users.ToListAsync();
        ViewBag.Schedules = await _context.Schedules
            .Include(s => s.Group)
                .ThenInclude(g => g.Style)
            .Include(s => s.Group.Coach)
            .ToListAsync();
        ViewBag.Coaches = await _context.Coaches.ToListAsync();
        ViewBag.Statuses = new List<string> { "Заплановано", "Підтверджено", "Скасовано", "Відвідано" };
        
        return View();
    }

    // POST: Bookings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Booking booking)
    {
        if (ModelState.IsValid)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Users = await _context.Users.ToListAsync();
        ViewBag.Schedules = await _context.Schedules.ToListAsync();
        ViewBag.Coaches = await _context.Coaches.ToListAsync();
        ViewBag.Statuses = new List<string> { "Заплановано", "Підтверджено", "Скасовано", "Відвідано" };
        return View(booking);
    }

    // GET: Bookings/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
        {
            return NotFound();
        }
        
        ViewBag.Users = await _context.Users.ToListAsync();
        ViewBag.Schedules = await _context.Schedules
            .Include(s => s.Group)
            .ToListAsync();
        ViewBag.Coaches = await _context.Coaches.ToListAsync();
        ViewBag.Statuses = new List<string> { "Заплановано", "Підтверджено", "Скасовано", "Відвідано" };
        
        return View(booking);
    }

    // POST: Bookings/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Booking booking)
    {
        if (id != booking.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(booking.Id))
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
        
        ViewBag.Users = await _context.Users.ToListAsync();
        ViewBag.Schedules = await _context.Schedules.ToListAsync();
        ViewBag.Coaches = await _context.Coaches.ToListAsync();
        ViewBag.Statuses = new List<string> { "Заплановано", "Підтверджено", "Скасовано", "Відвідано" };
        return View(booking);
    }

    // GET: Bookings/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Schedule)
                .ThenInclude(s => s.Group)
                    .ThenInclude(g => g.Style)
            .Include(b => b.Schedule.Group.Coach)
            .Include(b => b.Schedule.Group.AgeCategory)
            .Include(b => b.Coach)
            .FirstOrDefaultAsync(b => b.Id == id);
        
        if (booking == null)
        {
            return NotFound();
        }

        return View(booking);
    }

    // GET: Bookings/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Schedule)
            .FirstOrDefaultAsync(b => b.Id == id);
        
        if (booking == null)
        {
            return NotFound();
        }

        return View(booking);
    }

    // POST: Bookings/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool BookingExists(int id)
    {
        return _context.Bookings.Any(e => e.Id == id);
    }
}