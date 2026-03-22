using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class UsersController : Controller
{
    private readonly DanceStudioContext _context;

    public UsersController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: Users
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users
            .Include(u => u.Bookings)        // зв'язок з Bookings
            .Include(u => u.Subscriptions)   // зв'язок з Subscriptions
            .ToListAsync();
        
        return View(users);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        if (ModelState.IsValid)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
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
        return View(user);
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var user = await _context.Users
            .Include(u => u.Bookings)
                .ThenInclude(b => b.Schedule)
            .Include(u => u.Subscriptions)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}