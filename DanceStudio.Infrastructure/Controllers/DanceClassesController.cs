// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using DanceStudio.Domain.Model;
// using DanceStudio.Infrastructure;

// namespace DanceStudio.Infrastructure.Controllers;

// public class DanceClassesController : Controller
// {
//     private readonly DanceStudioContext _context;

//     public DanceClassesController(DanceStudioContext context)
//     {
//         _context = context;
//     }

//     // GET: DanceClasses
//     public async Task<IActionResult> Index()
//     {
//         var styles = await _context.Styles
//             .Include(s => s.Groups)  // зв'язок з Groups
//             .ToListAsync();
        
//         return View(styles);
//     }

//     // GET: DanceClasses/Create
//     public IActionResult Create()
//     {
//         return View();
//     }

//     // POST: DanceClasses/Create
//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Create(Style style)
//     {
//         if (ModelState.IsValid)
//         {
//             _context.Styles.Add(style);
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//         return View(style);
//     }

//     // GET: DanceClasses/Edit/5
//     public async Task<IActionResult> Edit(int id)
//     {
//         var style = await _context.Styles.FindAsync(id);
//         if (style == null)
//         {
//             return NotFound();
//         }
//         return View(style);
//     }

//     // POST: DanceClasses/Edit/5
//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Edit(int id, Style style)
//     {
//         if (id != style.Id)
//         {
//             return NotFound();
//         }

//         if (ModelState.IsValid)
//         {
//             try
//             {
//                 _context.Update(style);
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!StyleExists(style.Id))
//                 {
//                     return NotFound();
//                 }
//                 else
//                 {
//                     throw;
//                 }
//             }
//             return RedirectToAction(nameof(Index));
//         }
//         return View(style);
//     }

//     // GET: DanceClasses/Details/5
//     public async Task<IActionResult> Details(int id)
//     {
//         var style = await _context.Styles
//             .Include(s => s.Groups)           // зв'язок з Groups
//                 .ThenInclude(g => g.AgeCategory)  // групи → вікова категорія
//             .Include(s => s.Groups)
//                 .ThenInclude(g => g.Coach)        // групи → тренер
//             .FirstOrDefaultAsync(s => s.Id == id);
        
//         if (style == null)
//         {
//             return NotFound();
//         }

//         return View(style);
//     }

//     // GET: DanceClasses/Delete/5
//     public async Task<IActionResult> Delete(int id)
//     {
//         var style = await _context.Styles
//             .FirstOrDefaultAsync(s => s.Id == id);
        
//         if (style == null)
//         {
//             return NotFound();
//         }

//         return View(style);
//     }

//     // POST: DanceClasses/Delete/5
//     [HttpPost, ActionName("Delete")]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> DeleteConfirmed(int id)
//     {
//         var style = await _context.Styles.FindAsync(id);
//         if (style != null)
//         {
//             _context.Styles.Remove(style);
//             await _context.SaveChangesAsync();
//         }
//         return RedirectToAction(nameof(Index));
//     }

//     private bool StyleExists(int id)
//     {
//         return _context.Styles.Any(e => e.Id == id);
//     }
// }


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class DanceClassesController : Controller
{
    private readonly DanceStudioContext _context;

    public DanceClassesController(DanceStudioContext context)
    {
        _context = context;
    }

    // GET: DanceClasses
    public async Task<IActionResult> Index()
    {
        var styles = await _context.Styles
            .Include(s => s.Groups)
            .ToListAsync();
        
        return View(styles);
    }

    // GET: DanceClasses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DanceClasses/Create
 
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Style style)
{
    if (ModelState.IsValid)
    {
        _context.Styles.Add(style);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    return View(style);
}
    // GET: DanceClasses/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var style = await _context.Styles.FindAsync(id);
        if (style == null)
        {
            return NotFound();
        }
        return View(style);
    }

    // POST: DanceClasses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Style style)
    {
        if (id != style.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(style);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StyleExists(style.Id))
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
        return View(style);
    }

    // GET: DanceClasses/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var style = await _context.Styles
            .Include(s => s.Groups)
                .ThenInclude(g => g.AgeCategory)
            .Include(s => s.Groups)
                .ThenInclude(g => g.Coach)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (style == null)
        {
            return NotFound();
        }

        return View(style);
    }

    // GET: DanceClasses/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var style = await _context.Styles
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (style == null)
        {
            return NotFound();
        }

        return View(style);
    }

    // POST: DanceClasses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var style = await _context.Styles.FindAsync(id);
        if (style != null)
        {
            _context.Styles.Remove(style);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool StyleExists(int id)
    {
        return _context.Styles.Any(e => e.Id == id);
    }
}
