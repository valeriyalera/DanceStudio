using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;
using DanceStudio.Infrastructure.Services;
using ClosedXML.Excel;
namespace DanceStudio.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin,Coach,Client")]
public class DanceClassesController : Controller
{
    private readonly DanceStudioContext _context;
    private readonly IDataPortServiceFactory<Style> _dataPortServiceFactory;

    public DanceClassesController(DanceStudioContext context, IDataPortServiceFactory<Style> dataPortServiceFactory)
    {
        _context = context;
        _dataPortServiceFactory = dataPortServiceFactory;
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
        ModelState.Remove("Groups");

        if (!ModelState.IsValid)
            return View(style);

        _context.Styles.Add(style);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: DanceClasses/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var style = await _context.Styles.FindAsync(id);
        if (style == null)
            return NotFound();

        return View(style);
    }

    // POST: DanceClasses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Style style)
    {
        if (id != style.Id)
            return NotFound();

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
                    return NotFound();
                else
                    throw;
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
            return NotFound();

        return View(style);
    }

    // GET: DanceClasses/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var style = await _context.Styles
            .FirstOrDefaultAsync(s => s.Id == id);

        if (style == null)
            return NotFound();

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

    // ==================== ІМПОРТ ТА ЕКСПОРТ ====================

    // GET: DanceClasses/Import
    public IActionResult Import()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Import(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Будь ласка, оберіть файл Excel.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RowsUsed().Skip(1);

            // Отримуємо максимальний ID, якщо є записи
            int maxId = _context.Styles.Any() ? _context.Styles.Max(s => s.Id) : 0;

            foreach (var row in rows)
            {
                var styleName = row.Cell(2).GetString();
                if (!string.IsNullOrWhiteSpace(styleName))
                {
                    maxId++;
                    _context.Styles.Add(new Style { Id = maxId, Name = styleName });
                }
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Дані успішно імпортовано!";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ПОМИЛКА: {ex.Message}");
            TempData["Error"] = $"Помилка імпорту: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
    // GET: DanceClasses/Export
    public async Task<IActionResult> Export(CancellationToken cancellationToken)
    {
        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var exportService = _dataPortServiceFactory.GetExportService(contentType);

        var stream = new MemoryStream();
        await exportService.WriteToAsync(stream, cancellationToken);
        stream.Position = 0;

        return File(stream, contentType, $"styles_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx");
    }



    private bool StyleExists(int id)
    {
        return _context.Styles.Any(e => e.Id == id);
    }
}