
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;
using DanceStudio.Infrastructure.Services;

namespace DanceStudio.Infrastructure.Controllers;

public class GroupsController : Controller
{
    private readonly DanceStudioContext _context;

    public GroupsController(DanceStudioContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var groups = await _context.Groups
            .Include(g => g.Style)
            .Include(g => g.AgeCategory)
            .Include(g => g.Coach)  // ← підтягуємо тренера
            .ToListAsync();

        return View(groups);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Group group)
    {
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private readonly IDataPortServiceFactory<Group> _dataPortServiceFactory;

    public GroupsController(DanceStudioContext context, IDataPortServiceFactory<Group> dataPortServiceFactory)
    {
        _context = context;
        _dataPortServiceFactory = dataPortServiceFactory;
    }

// GET: Groups/Import
public IActionResult Import()
{
    return View();
}

// POST: Groups/Import
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Import(IFormFile fileExcel, CancellationToken cancellationToken)
{
    if (fileExcel == null || fileExcel.Length == 0)
    {
        ModelState.AddModelError("", "Будь ласка, оберіть файл Excel.");
        return View();
    }

    var importService = _dataPortServiceFactory.GetImportService(fileExcel.ContentType);

    using var stream = fileExcel.OpenReadStream();
    await importService.ImportFromStreamAsync(stream, cancellationToken);

    return RedirectToAction(nameof(Index));
}

// GET: Groups/Export
public async Task<IActionResult> Export(CancellationToken cancellationToken)
{
    const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    var exportService = _dataPortServiceFactory.GetExportService(contentType);

    using var stream = new MemoryStream();
    await exportService.WriteToAsync(stream, cancellationToken);

    stream.Position = 0;
    return File(stream, contentType, $"groups_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx");
}

}

