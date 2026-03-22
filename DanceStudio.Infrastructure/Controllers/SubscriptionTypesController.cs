using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class SubscriptionTypesController : Controller
{
    private readonly DanceStudioContext _context;

    public SubscriptionTypesController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: SubscriptionTypes
    public async Task<IActionResult> Index()
    {
        var subscriptionTypes = await _context.Set<SubscriptionType>()
            .Include(st => st.Subscriptions)  // зв'язок з Subscriptions
            .ToListAsync();
        
        return View(subscriptionTypes);
    }

    // GET: SubscriptionTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SubscriptionTypes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubscriptionType subscriptionType)
    {
        if (ModelState.IsValid)
        {
            _context.Set<SubscriptionType>().Add(subscriptionType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(subscriptionType);
    }

    // GET: SubscriptionTypes/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var subscriptionType = await _context.Set<SubscriptionType>().FindAsync(id);
        if (subscriptionType == null)
        {
            return NotFound();
        }
        return View(subscriptionType);
    }

    // POST: SubscriptionTypes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SubscriptionType subscriptionType)
    {
        if (id != subscriptionType.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(subscriptionType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionTypeExists(subscriptionType.Id))
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
        return View(subscriptionType);
    }

    // GET: SubscriptionTypes/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var subscriptionType = await _context.Set<SubscriptionType>()
            .Include(st => st.Subscriptions)
                .ThenInclude(s => s.User)
            .FirstOrDefaultAsync(st => st.Id == id);
        
        if (subscriptionType == null)
        {
            return NotFound();
        }

        return View(subscriptionType);
    }

    // GET: SubscriptionTypes/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var subscriptionType = await _context.Set<SubscriptionType>()
            .FirstOrDefaultAsync(st => st.Id == id);
        
        if (subscriptionType == null)
        {
            return NotFound();
        }

        return View(subscriptionType);
    }

    // POST: SubscriptionTypes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var subscriptionType = await _context.Set<SubscriptionType>().FindAsync(id);
        if (subscriptionType != null)
        {
            _context.Set<SubscriptionType>().Remove(subscriptionType);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool SubscriptionTypeExists(int id)
    {
        return _context.Set<SubscriptionType>().Any(e => e.Id == id);
    }
}