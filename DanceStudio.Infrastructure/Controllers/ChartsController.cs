using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DanceStudio.Domain.Model;
using DanceStudio.Infrastructure;

namespace DanceStudio.Infrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartsController : ControllerBase
{
    private readonly DanceStudioContext _context;

    public ChartsController(DanceStudioContext context)
    {
        _context = context;
    }

    [HttpGet("groupsPerStyle")]
    public async Task<JsonResult> GetGroupsPerStyleAsync(CancellationToken cancellationToken)
    {
        var data = await _context.Groups
            .Include(g => g.Style)
            .GroupBy(g => g.Style!.Name ?? "Без стилю")
            .Select(g => new
            {
                Style = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync(cancellationToken);

        return new JsonResult(data);
    }
}