using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class SubscriptionsController : Controller
{
    private readonly DanceStudioContext _context;

    public SubscriptionsController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: Subscriptions
    public async Task<IActionResult> Index()
    {
        var subscriptions = await _context.Subscriptions
            .Include(s => s.User)              // зв'язок з User
            .Include(s => s.Payment)           // зв'язок з Payment (1-1)
            .Include(s => s.SubscriptionType)  // зв'язок з SubscriptionType (1-1)
            .ToListAsync();
        
        return View(subscriptions);
    }

    // GET: Subscriptions/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Users = await _context.Users.ToListAsync();
        ViewBag.Payments = await _context.Payments.ToListAsync();
        ViewBag.SubscriptionTypes = await _context.Set<SubscriptionType>().ToListAsync();
        return View();
    }

    // POST: Subscriptions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Subscription subscription)
    {
        if (ModelState.IsValid)
        {
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Users = await _context.Users.ToListAsync();
        ViewBag.Payments = await _context.Payments.ToListAsync();
        ViewBag.SubscriptionTypes = await _context.Set<SubscriptionType>().ToListAsync();
        return View(subscription);
    }

    // GET: Subscriptions/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        if (subscription == null)
        {
            return NotFound();
        }
        
        ViewBag.Users = await _context.Users.ToListAsync();
        ViewBag.Payments = await _context.Payments.ToListAsync();
        ViewBag.SubscriptionTypes = await _context.Set<SubscriptionType>().ToListAsync();
        return View(subscription);
    }

    // POST: Subscriptions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Subscription subscription)
    {
        if (id != subscription.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(subscription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(subscription.Id))
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
        ViewBag.Payments = await _context.Payments.ToListAsync();
        ViewBag.SubscriptionTypes = await _context.Set<SubscriptionType>().ToListAsync();
        return View(subscription);
    }

    // GET: Subscriptions/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var subscription = await _context.Subscriptions
            .Include(s => s.User)
            .Include(s => s.Payment)
            .Include(s => s.SubscriptionType)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (subscription == null)
        {
            return NotFound();
        }

        return View(subscription);
    }

    // GET: Subscriptions/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var subscription = await _context.Subscriptions
            .Include(s => s.User)
            .Include(s => s.Payment)
            .Include(s => s.SubscriptionType)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (subscription == null)
        {
            return NotFound();
        }

        return View(subscription);
    }

    // POST: Subscriptions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        if (subscription != null)
        {
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool SubscriptionExists(int id)
    {
        return _context.Subscriptions.Any(e => e.Id == id);
    }
}
