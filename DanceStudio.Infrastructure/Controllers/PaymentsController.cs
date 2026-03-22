
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

public class PaymentsController : Controller
{
    private readonly DanceStudioContext _context;

    public PaymentsController(DanceStudioContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var payments = await _context.Payments.ToListAsync();
        return View(payments);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
