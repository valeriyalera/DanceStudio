using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class AgeCategoriesController : Controller
{
    private readonly DanceStudioContext _context;

    public AgeCategoriesController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: AgeCategories
    public async Task<IActionResult> Index()
    {
        var ageCategories = await _context.AgeCategories
            .Include(a => a.Groups)              // зв'язок з Groups (1:M)
                .ThenInclude(g => g.Style)       // Group → Style
            .Include(a => a.Groups)
                .ThenInclude(g => g.Coach)       // Group → Coach
            .ToListAsync();
        
        return View(ageCategories);
    }

    // GET: AgeCategories/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AgeCategories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AgeCategory ageCategory)
    {
        if (ModelState.IsValid)
        {
            _context.AgeCategories.Add(ageCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(ageCategory);
    }

    // GET: AgeCategories/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var ageCategory = await _context.AgeCategories.FindAsync(id);
        if (ageCategory == null)
        {
            return NotFound();
        }
        return View(ageCategory);
    }

    // POST: AgeCategories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AgeCategory ageCategory)
    {
        if (id != ageCategory.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ageCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgeCategoryExists(ageCategory.Id))
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
        return View(ageCategory);
    }

    // GET: AgeCategories/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var ageCategory = await _context.AgeCategories
            .Include(a => a.Groups)              // всі групи цієї категорії
                .ThenInclude(g => g.Style)       // стиль групи
            .Include(a => a.Groups)
                .ThenInclude(g => g.Coach)       // тренер групи
            .Include(a => a.Groups)
                .ThenInclude(g => g.Schedules)   // розклад групи
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (ageCategory == null)
        {
            return NotFound();
        }

        return View(ageCategory);
    }

    // GET: AgeCategories/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var ageCategory = await _context.AgeCategories
            .Include(a => a.Groups)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (ageCategory == null)
        {
            return NotFound();
        }

        return View(ageCategory);
    }

    // POST: AgeCategories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ageCategory = await _context.AgeCategories.FindAsync(id);
        if (ageCategory != null)
        {
            _context.AgeCategories.Remove(ageCategory);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool AgeCategoryExists(int id)
    {
        return _context.AgeCategories.Any(e => e.Id == id);
    }
}